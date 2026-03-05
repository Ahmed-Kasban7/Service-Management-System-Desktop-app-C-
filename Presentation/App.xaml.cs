using Application.Common;
using Application.Repositories;
using Application.DTOs;
using Infrastructure;
using Infrastructure.Data;
using Presentation.View.Customer_View;
using System.Windows;
using Application.Services;

namespace Presentation;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            DatabaseInitializer.InitializeDatabase();
            var customerRepo = new CustomerRepository();
            var phoneRepo = new PhoneRepository();
            var brandRepo = new DeviceBrandRepository();
            var typeRepo = new DeviceTypeRepository();
            var specRepo = new DeviceSpecRepository();
            var deviceRepo = new DeviceRepository();

            var customerService = new CustomerService(customerRepo);
            var phoneService = new PhoneService(phoneRepo);
            var BrandService = new DeviceBrandService(brandRepo);
            var TypeService = new DeviceTypeService(typeRepo);
            var SpecService = new DeviceSpecService(specRepo);
            var deviceServie = new DeviceService(customerRepo, deviceRepo);
        
            var customerWindow = new CustomerListView(customerService , phoneService, BrandService , TypeService , SpecService , deviceServie);

            customerWindow.Show();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                ex.InnerException?.Message ?? ex.Message,
                "خطأ حقيقي من SQL",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );

            Current.Shutdown();
        }
    }
}