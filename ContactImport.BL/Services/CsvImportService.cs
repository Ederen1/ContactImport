using System.Data;
using ContactImport.Models;
using ContactImport.Services;
using CsvHelper;
using CsvHelper.Configuration;

namespace ContactImport.BL.Services;

public class CsvImportService : ICsvImportService
{
    private readonly CsvConfiguration _csvConfig;

    public CsvImportService(CsvConfiguration csvConfig)
    {
        _csvConfig = csvConfig;
    }

    public async Task<IEnumerable<ContactModel>> ReadFileAsync(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, _csvConfig);

        csv.GetRecords<ContactModel>();
        var records = csv.GetRecordsAsync<ContactModel>();
        if (records is null)
            throw new DataException("Invalid data");
        
        return await records.ToListAsync();
    }
}