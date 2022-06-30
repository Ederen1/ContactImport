using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

    public async Task ImportContacts(IEnumerable<ContactModel> models)
    {
        var entities = models.Select(model =>
        {
            var entity = new ContactEntity
            {
                Address = model.Adress,
                Name = model.Name,
                Surname = model.Surname,
                RodneCislo = model.RodneCislo,
            };

            if (model.Number1 is not null)
                entity.PhoneNumbers.Add(new PhoneNumberEntity {PhoneNumber = model.Number1});

            if (model.Number2 is not null)
                entity.PhoneNumbers.Add(new PhoneNumberEntity {PhoneNumber = model.Number2});

            if (model.Number3 is not null)
                entity.PhoneNumbers.Add(new PhoneNumberEntity {PhoneNumber = model.Number3});

            return entity;
        });

        await _dbContext.Contacts.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ContactModel>> AllContacts()
    {
        var entities = await _dbContext.Contacts.ToListAsync();
        var models = entities.Select(entity =>
        {
            var model = new ContactModel
            {
                Adress = entity.Address,
                Name = entity.Name,
                Surname = entity.Surname,
                RodneCislo = entity.RodneCislo,
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