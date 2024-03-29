﻿using CsvHelper.Configuration.Attributes;

namespace ContactImport.BL.Models;

public record ContactModel
{
    [Optional] [Name("Jméno")] public string? Name { get; init; }
    [Name("Příjmení")] public string Surname { get; init; }
    [Optional] [Name("RČ")] public string? RC { get; init; }
    [Optional] [Name("Adresa")] public string? Address { get; init; }
    [Optional] [Name("Telefon 1")] public string? Number1 { get; set; }
    [Optional] [Name("Telefon 2")] public string? Number2 { get; set; }
    [Optional] [Name("Telefon 3")] public string? Number3 { get; set; }
}