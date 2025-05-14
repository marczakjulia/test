using Models;

namespace Application;

public interface IMobilesService
{
    IEnumerable<PhoneNumberDto> GetAllPhoneNumbers();
    MobileDataResponse? GetPhoneNumber(string number);
    void CreateOrUpdateMobile(string operatorName, string mobileNumber, Client client);
  
}