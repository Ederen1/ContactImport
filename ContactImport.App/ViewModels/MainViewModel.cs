using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContactImport.Services;
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
            
            var models = await _importService.ReadFileAsync(fileDialog.FileName);
            await _contactService.ImportContacts(models);
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