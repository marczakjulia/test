namespace Models;

public class PhoneCreateRequest
{
        public string Operator { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public Client Client { get; set; } = null!;
}