namespace ContactImport.DAL.Entities;

public class ContactEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Surname { get; set; } = null!;
    public string? RodneCislo { get; set; }
    public string? Address { get; set; }
    public virtual List<PhoneNumberEntity> PhoneNumbers { get; set; } = new();
}