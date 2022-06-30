using ContactImport.Models;

namespace ContactImport.BL.Services;

public interface IContactService
{
    public Task<ImportReport> ImportContacts(IEnumerable<ContactModel> models);
    public Task<IEnumerable<ContactModel>> AllContacts();
}