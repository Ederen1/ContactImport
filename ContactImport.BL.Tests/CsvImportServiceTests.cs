using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactImport.BL.Services;
using ContactImport.BL.Validators;
using CsvHelper;
using CsvHelper.Configuration;
using Xunit;

namespace ContactImport.BL.Tests;

public class CsvImportServiceTests : UnitTestBase
{
    private CsvImportService _service = new(new CsvConfiguration(CultureInfo.InvariantCulture) {Delimiter = ";"},
        new ContactModelValidator());

    private StreamReader ToReader(string data)
    {
        return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(data)));
    }

    [Fact]
    public async Task ReadStreamAsyncEmptyTest()
    {
        using var reader = ToReader("");
        var models = await _service.ReadCsvAsync(reader);
        Assert.Empty(models);
    }

    [Fact]
    public async Task ReadStreamAsyncOnlyHeaderTest()
    {
        using var reader = ToReader("Jméno;Příjmení;RČ;Adresa;Telefon 1;Telefon 2");
        var models = await _service.ReadCsvAsync(reader);
        Assert.Empty(models);
    }

    [Fact]
    public async Task ReadStreamAsyncMalformedHeaderTest()
    {
        await Assert.ThrowsAsync<HeaderValidationException>(async () =>
        {
            using var reader = ToReader("Jméno;Příjmen;RČ;Adresa;Telefon 1;Telefon 2");
            await _service.ReadCsvAsync(reader);
        });
    }

    [Fact]
    public async Task ReadStreamAsyncOneRecordTest()
    {
        using var stream = ToReader(@"Jméno;Příjmení;RČ;Adresa;Telefon 1;Telefon 2
Jiří;Mikát;9001019101;Babická 1 Brno;646545371;965360082");
        var models = (await _service.ReadCsvAsync(stream)).ToList();
        Assert.Single(models);
        Assert.Equal("9001019101", models.First().RC);
    }

    [Fact]
    public async Task ReadStreamAsyncMultipleRecordsTest()
    {
        using var stream = ToReader(@"Jméno;Příjmení;RČ;Adresa;Telefon 1;Telefon 2
Jiří;Mikát;9001019101;Babická 1 Brno;646545371;965360082
Antonín;Janošec;9001019102;Babičkova 2 Brno;952414971;997922951
Jan;Buš;9001019103;Bačovského 3 Brno;761868826;391020782
Pavel;Hříbek;9001019104;Bakalovo nábřeží 4 Brno;845250401;422500508
Michaela;Olšanská;9001019105;Balbínova 5 Brno;403675746;817680713
Jaroslava;Rosová;9001019106;Banskobystrická 6 Brno;310443389;485517660");
        var models = (await _service.ReadCsvAsync(stream)).ToList();
        Assert.Equal(6, models.Count);
        Assert.Equal("9001019101", models.First().RC);
        Assert.Equal("9001019104", models[3].RC);
    }

    [Fact]
    public async Task ReadStreamAsyncValidationErrorTest()
    {
        using var stream = ToReader(@"Jméno;Příjmení;RČ;Adresa;Telefon 1;Telefon 2
Jiří;Mikát;9005641019101;Babická 1 Brno;646545371;965360082");
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await _service.ReadCsvAsync(stream));
    }
}