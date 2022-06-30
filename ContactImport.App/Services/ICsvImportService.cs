using System.Collections.Generic;
using System.Threading.Tasks;
using ContactImport.Models;

namespace ContactImport.Services;

public interface ICsvImportService
{
    public Task<IEnumerable<ContactModel>> ReadFileAsync(string path);
}