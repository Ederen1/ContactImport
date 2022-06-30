using System.Collections.Generic;
using System.Threading.Tasks;
using ContactImport.Models;

namespace ContactImport.Services;

public interface IContactService
{
    public Task ImportContacts(IEnumerable<ContactModel> models);
    public Task<IEnumerable<ContactModel>> AllContacts();
}