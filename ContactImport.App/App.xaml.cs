﻿using System;
using System.Configuration;
using System.Globalization;
using System.Windows;
using AutoMapper;
using ContactImport.BL;
using ContactImport.BL.Models;
using ContactImport.BL.Services;
using ContactImport.BL.Validators;
using ContactImport.DAL;
using ContactImport.ViewModels;
using CsvHelper.Configuration;
using FluentValidation;
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
            try
            {
                SetupAndRun();
            }
            catch (Exception ex) when (Env.Debugging)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            base.OnStartup(e);
        }

        private void SetupAndRun()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var context = _serviceProvider.GetService<AppDbContext>();
            context!.Database.EnsureCreated();

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow!.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            services.AddDbContext<AppDbContext>(builder =>
                builder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
            services.AddSingleton<ICsvImportService, CsvImportService>();
            services.AddSingleton(_ => new CsvConfiguration(CultureInfo.InvariantCulture){Delimiter = ";"});
            services.AddTransient<MainViewModel>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IValidator<ContactModel>, ContactModelValidator>();
            services.AddAutoMapper(typeof(AutoMapperConfig));
            
            // services.AddDbContext<AppDbContext>(builder => builder.UseInMemoryDatabase("TEST").UseLazyLoadingProxies());
        }
    }
}