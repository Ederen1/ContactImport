namespace ContactImport.DAL.Entities;

public class PhoneNumberEntity
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
}