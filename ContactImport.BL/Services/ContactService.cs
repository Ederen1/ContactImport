using AutoMapper;
using ContactImport.BL.Models;
using ContactImport.DAL;
using ContactImport.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactImport.BL.Services;

public class ContactService : IContactService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ContactService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<(int New, int Updated)> ImportContacts(IEnumerable<ContactModel> toImport)
    {
        int newContacts = 0, updatedContacts = 0;
        foreach (var model in toImport)
        {
            var inDb = await _dbContext.Contacts.FirstOrDefaultAsync(
                item => item.RC == model.RC);
            if (inDb is not null)
            {
                _dbContext.Entry(inDb).CurrentValues.SetValues(model);
                updatedContacts++;
            }
            else
            {
                _dbContext.Contacts.Add(_mapper.Map<ContactEntity>(model));
                newContacts++;
            }
        }

        await _dbContext.SaveChangesAsync();
        return (newContacts, updatedContacts);
    }
    
    public async Task<IEnumerable<ContactModel>> AllContacts()
    {
        var entities = await _dbContext.Contacts.ToListAsync();

        return _mapper.Map<IEnumerable<ContactModel>>(entities);
    }
}