using System.Data;
using ContactImport.BL.Models;
using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace ContactImport.BL.Services;

public class CsvImportService : ICsvImportService
{
    private readonly CsvConfiguration _csvConfig;
    private readonly IValidator<ContactModel> _contactValidator;

    public CsvImportService(CsvConfiguration csvConfig, IValidator<ContactModel> contactValidator)
    {
        _csvConfig = csvConfig;
        _contactValidator = contactValidator;
    }

    public async Task<IEnumerable<ContactModel>> ReadCsvAsync(string path)
    {
        using var reader = new StreamReader(path);
        return await ReadCsvAsync(reader);
    }

    public async Task<IEnumerable<ContactModel>> ReadCsvAsync(TextReader reader)
    {
        using var csv = new CsvReader(reader, _csvConfig);

        csv.GetRecords<ContactModel>();
        var records = csv.GetRecordsAsync<ContactModel>();
        if (records is null)
            throw new ValidationException("Invalid data");

        var ret = await records.ToListAsync();
        uint i = 1;
        foreach (var model in ret)
        {
            i++;
            var result = await _contactValidator.ValidateAsync(model);
            if (!result.IsValid)
            {
                var text = $"Error at line {i}: \r\n";
                text += string.Join("\r\n", result.Errors);
                throw new ValidationException(text);
            }
        }

        return ret;
    }
}