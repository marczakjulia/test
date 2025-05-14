using Application;
using Models;
using Repository;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("UniversityDatabase");


builder.Services.AddTransient<IMobilesRepository>(_ => new MobilesRepository(connectionString));
builder.Services.AddTransient<MobilesService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/mobiles", (MobilesService svc) =>
{
    try
    {
        return Results.Ok(svc.GetAllPhoneNumbers());
    }
    catch (Exception)
    {
        return Results.NotFound();
    }
});


app.MapGet("/api/mobiles/{number}", (string number, MobilesService svc) =>
{
    var phone = svc.GetPhoneNumber(number);
    return phone is null ? Results.NotFound() : Results.Ok(phone);
});


app.MapPost("/api/mobiles", (PhoneCreateRequest req, MobilesService svc) =>
{
    try
    {
        svc.CreateOrUpdateMobile(req.Operator, req.MobileNumber, req.Client);
        return Results.Ok();
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest();
    }
    catch (Exception ex)
    {
        return Results.NotFound();
    }
});

app.Run();