using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContactImport.BL.Services;
using ContactImport.DAL;
using ContactImport.DAL.Entities;
using ContactImport.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactImport.Services;

public class ContactService : IContactService
{
    private AppDbContext _dbContext;

    public ContactService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(int New, int Updated)> ImportContacts(IEnumerable<ContactModel> toImport)
    {
        var entities = toImport.Select(model =>
        {
            var entity = new ContactEntity
            {
                Address = model.Address,
                Name = model.Name,
                Surname = model.Surname,
                RC = model.RC,
            };

            if (model.Number1 is not null)
                entity.PhoneNumbers.Add(new PhoneNumberEntity {PhoneNumber = model.Number1});

            if (model.Number2 is not null)
                entity.PhoneNumbers.Add(new PhoneNumberEntity {PhoneNumber = model.Number2});

            if (model.Number3 is not null)
                entity.PhoneNumbers.Add(new PhoneNumberEntity {PhoneNumber = model.Number3});

            return entity;
        });

        int newContacts = 0, updatedContacts = 0;
        foreach (var contactEntity in entities)
        {
            var inDb = await _dbContext.Contacts.FirstOrDefaultAsync(
                item => item.RC == contactEntity.RC);
            if (inDb is not null)
            {
                _dbContext.Contacts.Remove(inDb);
                updatedContacts++;
            }
            else
            {
                newContacts++;
            }

            _dbContext.Contacts.Add(contactEntity);
        }

        await _dbContext.SaveChangesAsync();
        return (newContacts, updatedContacts);
    }

    public async Task<IEnumerable<ContactModel>> AllContacts()
    {
        var entities = await _dbContext.Contacts.ToListAsync();
        var models = entities.Select(entity =>
        {
            var model = new ContactModel
            {
                Address = entity.Address,
                Name = entity.Name,
                Surname = entity.Surname,
                RC = entity.RC,
            };

            if (entity.PhoneNumbers.Count >= 1)
                model.Number1 = entity.PhoneNumbers[0].PhoneNumber;

            if (entity.PhoneNumbers.Count >= 2)
                model.Number2 = entity.PhoneNumbers[0].PhoneNumber;


            if (entity.PhoneNumbers.Count >= 3)
                model.Number3 = entity.PhoneNumbers[0].PhoneNumber;

            return model;
        });

        return models;
    }
}