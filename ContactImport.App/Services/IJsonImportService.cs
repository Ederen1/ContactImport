using System.Collections.Generic;
using ContactImport.DAL.Entities;
using ContactImport.Models;

namespace ContactImport.Services;

public interface IJsonImportService
{
    public IEnumerable<ContactModel> ImportFile(string path);
}