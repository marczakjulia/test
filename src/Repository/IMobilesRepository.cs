using Models;

namespace Repository;

public interface IMobilesRepository
{
    IEnumerable<PhoneNumberDto> GetAllPhoneNumbers();
    PhoneNumber? GetByPhoneNumber(string phoneNumber);
    Client? GetClientByEmail(string email);
    int CreateClient(Client client);
    void UpdateClient(Client client);
    int GetOperatorIdByName(string name);
    Operator? GetOperatorById(int operatorId);
    void CreatePhoneNumber(PhoneNumber number);
    Client? GetClientById(int clientId);
    
}