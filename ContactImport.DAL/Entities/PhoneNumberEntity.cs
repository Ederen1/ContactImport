namespace ContactImport.DAL.Entities;

public class PhoneNumberEntity
{
    public Guid PhoneNumberId { get; set; }
    public string PhoneNumber { get; set; } = null!;
}