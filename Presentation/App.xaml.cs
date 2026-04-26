using Application.Common;
using Application.Repositories;
using Application.DTOs;
using Infrastructure;
using Infrastructure.Data;
using Presentation.View.Customer_View;
using System.Windows;
using Application.Features.PhoneManagement;
using Application.Features.BrandManagement;
using Application.Features.CustomerManagment;
using Application.Features.DeviceManagement;
using Application.Features.TypeManagement;
using Application.Features.SpecManagement;
using Application.Repositories;
using Application.Features.OrderManagement.Commands;
using Infrastructure.Repositories;
using Application.Features.DeviceManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.OrderManagement.Queries;
using Presentation.View.MainView;


namespace Presentation;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            DatabaseInitializer.InitializeDatabase();
            var phoneRepo = new PhoneRepository();
            var deviceRepo = new DeviceRepository();
            var customerRepo = new CustomerRepository(deviceRepo, phoneRepo);
            var brandRepo = new DeviceBrandRepository();
            var typeRepo = new DeviceTypeRepository();
            var specRepo = new DeviceSpecRepository();
            var orderRepo = new OrderRepository();

            var customerService = new CustomerService(customerRepo);
            var phoneService = new PhoneService(phoneRepo);
            var BrandService = new DeviceBrandService(brandRepo);
            var TypeService = new DeviceTypeService(typeRepo);
            var SpecService = new DeviceSpecService(specRepo);
            var deviceServie = new DeviceService(customerRepo, deviceRepo);
            var createOrderHandler = new CreateOrderHandler(orderRepo, customerRepo, deviceRepo);
            var  getCustomerDeviceHandler= new GetCustomerDevicesHandler(deviceRepo);
            var  getCustomerLookupHandler= new GetCustomersLookupHandler(customerRepo);
            var getoderPaged =new GetPagedOrderSummariesHandler(orderRepo);
            var  getorderPaged= new GetPagedOrderSummariesHandler(orderRepo);
            var getOrderDetails = new GetOrderFullDetailsHandler(orderRepo);
            var updateOrder = new UpdateOrderHandler(orderRepo);

            var mainWindow = new MainWindow(getOrderDetails , getorderPaged  , getCustomerLookupHandler , getCustomerDeviceHandler , createOrderHandler , updateOrder);
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.InnerException?.Message ?? ex.Message,
                "خطاء",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );

            Current.Shutdown();
        }
    }
}