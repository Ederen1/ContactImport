﻿namespace ContactImport.DAL.Entities;

public class ContactEntity
{
    public Guid ContactId { get; set; }
    public string? Name { get; set; }
    public string Surname { get; set; } = null!;
    public string? RodneCislo { get; set; }
    public string? Address { get; set; }
    public List<PhoneNumberEntity> PhoneNumbers = new();
}