using System.Collections.Generic;
using System.Data;
using System.IO;
using ContactImport.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace ContactImport.Services;

public class JsonImportService : IJsonImportService
{
    private readonly CsvConfiguration _csvConfig;

    public JsonImportService(CsvConfiguration csvConfig)
    {
        _csvConfig = csvConfig;
    }

    public IEnumerable<ContactModel> ImportFile(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, _csvConfig);

        var records = csv.GetRecords<ContactModel>();
        if (records is null)
            throw new DataException("Invalid data");
        
        return records;
    }
}