using ContactImport.Models;

namespace ContactImport.BL.Services;

public interface IContactService
{
    public Task ImportContacts(IEnumerable<ContactModel> models);
    public Task<IEnumerable<ContactModel>> AllContacts();
}