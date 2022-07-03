using Microsoft.EntityFrameworkCore;

namespace ContactImport.DAL.Entities;

[Index(nameof(RC))]
public class ContactEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string Surname { get; set; } = null!;
    public string? RC { get; set; }
    public string? Address { get; set; }
    public string? Number1 { get; set; }
    public string? Number2 { get; set; }
    public string? Number3 { get; set; }
}