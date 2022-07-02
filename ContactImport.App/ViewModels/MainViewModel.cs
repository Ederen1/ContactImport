using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContactImport.BL.Services;
using ContactImport.BL.Validators;
using ContactImport.Models;
using ContactImport.Services;
using FluentValidation;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace ContactImport.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ICsvImportService _importService;
    private readonly IContactService _contactService;
    private readonly IValidator<ContactModel> _contactValidator;
    public ICommand ImportCsvClickCommand { get; set; }

    public MainViewModel(ICsvImportService importService, IContactService contactService, IValidator<ContactModel> contactValidator)
    {
        _importService = importService;
        _contactService = contactService;
        _contactValidator = contactValidator;
        ImportCsvClickCommand = new AsyncRelayCommand(ImportCsvClick);
    }

    private async Task ImportCsvClick()
    {
        try
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != true)
                return;
            
            var contacts = (await _importService.ReadFileAsync(fileDialog.FileName)).ToList();
            foreach (var contact in contacts)
            {
                var res = await _contactValidator.ValidateAsync(contact);
                if (!res.IsValid)
                {
                    var message = "Validation error: \r\n";
                    res.Errors.ForEach(err => message += $"Property '{err.PropertyName}' has invalid value of '{err.AttemptedValue}'\r\n");
                    MessageBox.Show(message);
                    return;
                }
            }
            
            var (newContacts, updatedContacts) = await _contactService.ImportContacts(contacts);
            MessageBox.Show($"Successfully imported {newContacts} contacts, updated {updatedContacts}");
        }
        catch (InvalidDataException e)
        {
            MessageBox.Show($"Invalid data found in file\r\n{e.Message}");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}