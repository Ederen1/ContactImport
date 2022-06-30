using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ContactImport.BL.Services;
using ContactImport.DAL;
using ContactImport.Services;
using ContactImport.ViewModels;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContactImport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var context = _serviceProvider.GetService<AppDbContext>();
            context!.Database.EnsureCreated();

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow!.Show();

            base.OnStartup(e);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            // services.AddDbContext<AppDbContext>(builder =>
                // builder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
            services.AddSingleton<ICsvImportService, CsvImportService>();
            services.AddSingleton(_ => new CsvConfiguration(CultureInfo.InvariantCulture){Delimiter = ";"});
            services.AddTransient<MainViewModel>();
            services.AddScoped<IContactService, ContactService>();
            
            services.AddDbContext<AppDbContext>(builder => builder.UseInMemoryDatabase("TEST").UseLazyLoadingProxies());
        }
    }
}