using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactImport.Models;

public record ContactModel
{
    public ContactModel(Guid id, string surname, string? name, string? rodneCislo, string? adress, List<PhoneNumberModel> numbers)
    {
        this.Id = id;
        this.Surname = surname;
        this.Name = name;
        this.RodneCislo = rodneCislo;
        this.Adress = adress;
        this.Numbers = numbers;
    }
    
    public string? Adress { get; init; }
    public Guid Id { get; init; }
    public string Surname { get; init; }
    public string? Name { get; init; }
    public string? RodneCislo { get; init; }
    public List<PhoneNumberModel> Numbers { get; init; }
}