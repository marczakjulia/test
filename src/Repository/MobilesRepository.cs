using Microsoft.Data.SqlClient;
using Models;

namespace Repository;

public class MobilesRepository : IMobilesRepository
{
    private readonly string _connectionString;

    public MobilesRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<PhoneNumberDto> GetAllPhoneNumbers()
    {
        var result = new List<PhoneNumberDto>();
        const string sql = "SELECT * FROM PhoneNumber";
        using SqlConnection conn = new(_connectionString);
        SqlCommand cmd = new(sql, conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new PhoneNumberDto()
            {
                Id = reader.GetInt32(0),
                Number = reader.GetString(3)
            });
        }
        return result;
    }

    public PhoneNumber? GetByPhoneNumber(string phoneNumber)
    {
        const string sql = "SELECT * FROM PhoneNumber WHERE Number = @Number";
        using SqlConnection conn = new(_connectionString);
        SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Number", phoneNumber);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new PhoneNumber
            {
                Id = reader.GetInt32(0),
                OperatorId = reader.GetInt32(1),
                ClientId = reader.GetInt32(2),
                Number = reader.GetString(3)
            };
        }
        return null;
    }

    public Client? GetClientByEmail(string email)
    {
        const string sql = "SELECT * FROM Client WHERE Email = @Email";
        using SqlConnection conn = new(_connectionString);
        SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Email", email);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Client
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                City = reader.IsDBNull(3) ? null : reader.GetString(3)
            };
        }
        return null;
    }

    public int CreateClient(Client client)
    {
        const string sql = "INSERT INTO Client (FullName, Email, City) OUTPUT INSERTED.ID VALUES (@Name, @Email, @City)";
        using SqlConnection conn = new(_connectionString);
        SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Name", client.FullName);
        cmd.Parameters.AddWithValue("@Email", client.Email);
        cmd.Parameters.AddWithValue("@City", (object?)client.City ?? DBNull.Value);
        conn.Open();
        return (int)cmd.ExecuteScalar()!;
    }

    public void UpdateClient(Client client)
    {
        const string sql = "UPDATE Client SET FullName = @Name, City = @City WHERE Email = @Email";
        using SqlConnection conn = new(_connectionString);
        SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Name", client.FullName);
        cmd.Parameters.AddWithValue("@City", (object?)client.City ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Email", client.Email);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public int GetOperatorIdByName(string name)
    {
        const string sql = "SELECT Id FROM Operator WHERE Name = @Name";
        using SqlConnection conn = new(_connectionString);
        SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@Name", name);
        conn.Open();
        var result = cmd.ExecuteScalar();
        if (result == null) 
            throw new Exception("Operator not found");
        
        return (int)result;
    }

    public Client? GetClientById(int clientId)
    {
        const string sql = "SELECT Id, FullName, Email, City FROM Client WHERE Id = @Id";
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", clientId);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Client
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                City = reader.GetString(3)
            };
        }
        return null;
    }

    public Operator? GetOperatorById(int operatorId)
    {
        const string sql = "SELECT Id, Name FROM [Operator] WHERE Id = @Id";
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", operatorId);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Operator
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }
        return null;
    }

    public void CreatePhoneNumber(PhoneNumber number)
    {
        const string sql = "INSERT INTO PhoneNumber (OperatorId, ClientId, Number) VALUES (@OperatorId, @ClientId, @Number)";
        using SqlConnection conn = new(_connectionString);
        SqlCommand cmd = new(sql, conn);
        cmd.Parameters.AddWithValue("@OperatorId", number.OperatorId);
        cmd.Parameters.AddWithValue("@ClientId", number.ClientId);
        cmd.Parameters.AddWithValue("@Number", number.Number);
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}