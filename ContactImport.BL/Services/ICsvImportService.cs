using ContactImport.BL.Models;

namespace ContactImport.BL.Services;

public interface ICsvImportService
{
    /// <summary>
    /// Parse CSV file specified in path an return a list of contacts it contained
    /// </summary>
    /// <param name="path">CSV file path</param>
    public Task<IEnumerable<ContactModel>> ReadCsvAsync(string path);

    public Task<IEnumerable<ContactModel>> ReadCsvAsync(TextReader reader);

}