using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContactImport.BL.Models;
using ContactImport.BL.Services;
using FluentValidation;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace ContactImport.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ICsvImportService _importService;
    private readonly IContactService _contactService;
    public ICommand ImportCsvClickCommand { get; }
    
    public ICommand ReloadCommand { get; }
    public ObservableCollection<ContactModel> Contacts { get; } = new();

    public MainViewModel(ICsvImportService importService, IContactService contactService)
    {
        _importService = importService;
        _contactService = contactService;
        ImportCsvClickCommand = new AsyncRelayCommand(ImportCsvClick);
        ReloadCommand = new AsyncRelayCommand(Reload);
    }
    private const string MessageCaption = "Import status";

    private async Task ImportCsvClick()
    {
        try
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() != true)
                return;
            
            var contacts = (await _importService.ReadCsvAsync(fileDialog.FileName)).ToList();
            
            var (newContacts, updatedContacts) = await _contactService.ImportContacts(contacts);
            await Reload();

            MessageBox.Show($"Successfully imported {newContacts} contacts, updated {updatedContacts}", MessageCaption);
        }
        catch (ValidationException e) when (Env.Debugging)
        {
            MessageBox.Show(e.Message, MessageCaption);
        }
    }

    private async Task Reload()
    {
        var contacts = await _contactService.AllContacts();
        Contacts.Clear();
        foreach (var contactModel in contacts.OrderBy(item => item.Name).ThenBy(item => item.Surname))
            Contacts.Add(contactModel);
    }
}