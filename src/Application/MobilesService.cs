using Models;
using Repository;

namespace Application;

public class MobilesService: IMobilesService
{
    private readonly IMobilesRepository _repository;

    public MobilesService(IMobilesRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<PhoneNumberDto> GetAllPhoneNumbers() => _repository.GetAllPhoneNumbers();

    public MobileDataResponse? GetPhoneNumber(string number)
    {
        var phone = _repository.GetByPhoneNumber(number);
        if (phone == null) return null;

        var oper = _repository.GetOperatorById(phone.OperatorId)
                   ?? throw new Exception("Operator not found");
        var client = _repository.GetClientById(phone.ClientId)
                     ?? throw new Exception("Client not found");

        return new MobileDataResponse {
            Operator = oper.Name,
            MobileNumber = phone.Number,
            Client = new Client {
                Id = client.Id,
                FullName = client.FullName,
                Email = client.Email,
                City = client.City
            }
        };
    }


    public void CreateOrUpdateMobile(string operatorName, string mobileNumber, Client client)
    {
        if (!mobileNumber.StartsWith("+48"))
            throw new ArgumentException("Number must start with +48");

        int operatorId = _repository.GetOperatorIdByName(operatorName);
        int clientId;

        if (!string.IsNullOrEmpty(client.FullName))
        {
            var existing = _repository.GetClientByEmail(client.Email);
            if (existing == null)
            {
                clientId = _repository.CreateClient(client);
            }
            else
            {
                clientId = existing.Id;
                _repository.UpdateClient(client);
            }
        }
        else
        {
            var existing = _repository.GetClientByEmail(client.Email) ?? throw new Exception("Client not found");
            clientId = existing.Id;
        }

        _repository.CreatePhoneNumber(new PhoneNumber
        {
            OperatorId = operatorId,
            ClientId = clientId,
            Number = mobileNumber
        });
    }
}
