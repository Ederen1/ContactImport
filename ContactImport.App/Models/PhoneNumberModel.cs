using System;

namespace ContactImport.Models;

public record PhoneNumberModel
{
    public PhoneNumberModel(Guid id, string number)
    {
        Id = id;
        Number = number;
    }

    public Guid Id { get; init; }
    public string Number { get; init; }
}