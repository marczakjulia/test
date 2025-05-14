namespace Models;

public class PhoneNumber
{
    public int Id { get; set; }
    public int OperatorId { get; set; }
    public int ClientId { get; set; }
    public string Number { get; set; }
}