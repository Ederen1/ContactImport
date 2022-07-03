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
    public ICommand ImportCsvClickCommand { get; set; }

    public MainViewModel(ICsvImportService importService, IContactService contactService)
    {
        _importService = importService;
        _contactService = contactService;
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