using ContactImport.BL.Models;

namespace ContactImport.BL.Services;

public interface IContactService
{
    /// <summary>
    /// Convert ContactModels into entities and import them into database.
    /// If a contact with same RC exists then it is updated
    /// </summary>
    /// <returns>Count of newly imported and updated entities</returns>
    public Task<(int New, int Updated)> ImportContacts(IEnumerable<ContactModel> toImport);
    /// <summary>
    /// Gets all contacts stored in database and returns them as list of models
    /// </summary>
    public Task<IEnumerable<ContactModel>> AllContacts();

    Task DeleteAll();
}