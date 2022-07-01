using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactImport.Models;
using ContactImport.Services;
using Xunit;

namespace ContactImport.BL.Tests;

public class ContactServiceUnitTests : UnitTestBase
{
    [Fact]
    public async Task ImportContactsInsertOne()
    {
        var service = new ContactService(DbContext);
        var dummyModels = new List<ContactModel>
        {
            new()
            {
                Adress = "Adresa 1/1 Brno",
                Name = "Jiří",
                Surname = "Neuhort",
                RC = "9001019101",
                Number1 = "646545371",
            }
        };

        await service.ImportContacts(dummyModels);

        Assert.Single(DbContext.Contacts);
        Assert.Equal("Jiří", DbContext.Contacts.First().Name);
    }

    [Fact]
    public async Task ImportContactsInsertMultiple()
    {
        var service = new ContactService(DbContext);
        var dummyModels = new List<ContactModel>
        {
            new()
            {
                Adress = "Adresa 1/1 Brno",
                Name = "Jiří",
                Surname = "Neuhort",
                RC = "9001019101",
                Number1 = "646545371",
            },
            new()
            {
                Adress = "Adresa 1/1 Brno",
                Name = "Jiří",
                Surname = "Neuhort",
                RC = "9001019101",
                Number1 = "646545371",
            },
            new()
            {
                Adress = "Adresa 1/1 Brno",
                Name = "Jiří",
                Surname = "Neuhort",
                RC = "9001019101",
                Number1 = "646545371",
            }
        };

        await service.ImportContacts(dummyModels);

        Assert.Equal(3, DbContext.Contacts.Count());
    }

    [Fact]
    public async Task ImportContactInsertWithNumber()
    {
        var service = new ContactService(DbContext);
        var dummyModels = new List<ContactModel>
        {
            new()
            {
                Adress = "Adresa 1/1 Brno",
                Name = "Jiří",
                Surname = "Neuhort",
                RC = "9001019101",
                Number1 = "646545371",
            },
        };

        await service.ImportContacts(dummyModels);
        Assert.Single(DbContext.PhoneNumbers);
        Assert.Single(DbContext.Contacts.First().PhoneNumbers);
        Assert.Equal("646545371", DbContext.Contacts.First().PhoneNumbers.First().PhoneNumber);
    }
}