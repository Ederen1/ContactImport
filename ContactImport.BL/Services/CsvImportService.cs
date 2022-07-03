using System.Data;
using ContactImport.Models;
using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;

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

    public async Task<IEnumerable<ContactModel>> ReadFileAsync(string path)
    {
        using var reader = new StreamReader(path);
        return await ReadStreamAsync(reader);
    }

    public async Task<IEnumerable<ContactModel>> ReadStreamAsync(TextReader reader)
    {
        using var csv = new CsvReader(reader, _csvConfig);

        csv.GetRecords<ContactModel>();
        var records = csv.GetRecordsAsync<ContactModel>();
        if (records is null)
            throw new DataException("Invalid data");
        
        var ret = await records.ToListAsync();
        foreach (var model in ret)
        {
            await _contactValidator.ValidateAndThrowAsync(model);
        }

        return ret;
    }
}