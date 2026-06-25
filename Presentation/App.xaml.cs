using Application.Common;
using Application.DTOs;
using Application.Features.AppointmentManagement.Commands;
using Application.Features.AppointmentManagement.Queries;
using Application.Features.BrandManagement;
using Application.Features.BrandManagement.Commands;
using Application.Features.BrandManagement.Queries;
using Application.Features.CampaignManagement.Command;
using Application.Features.CampaignManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment;
using Application.Features.CustomerManagment.Commands;
using Application.Features.DeviceManagement;
using Application.Features.DeviceManagement.Commands;
using Application.Features.DeviceManagement.Queries;
using Application.Features.EmployeeManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Application.Features.PhoneManagement;
using Application.Features.PhoneManagement.Commands;
using Application.Features.PhoneManagement.Queries;
using Application.Features.SourceManagement;
using Application.Features.SpecManagement;
using Application.Features.SpecManagement.Commands;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement;
using Application.Features.TypeManagement.Commands;
using Application.Features.TypeManagement.Queries;
using Application.Features.VisitManagement;
using Application.Repositories;
using Application.Repositories;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Presentation.View.Customer_View;
using Presentation.View.MainView;
using System.Windows;


namespace Presentation;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            DatabaseInitializer.InitializeDatabase();
            AppointmentRepository.UpdateOverdueAppointments();

            var phoneRepo = new PhoneRepository();
            var deviceRepo = new DeviceRepository();
            var customerRepo = new CustomerRepository(deviceRepo, phoneRepo);
            var brandRepo = new DeviceBrandRepository();
            var typeRepo = new DeviceTypeRepository();
            var specRepo = new DeviceSpecRepository();
            var orderRepo = new OrderRepository();
            var employeeRepo = new EmployeeRepository();
            var appointmentRepo = new AppointmentRepository();
            var visitRepo = new VisitRepository();
            var campaignRepo = new CampaignRepository();
            var sourcesRepo = new SourceRepository();

            var phoneService = new PhoneService(phoneRepo);
           
            var SpecService = new DeviceSpecService(specRepo);
            var createOrderHandler = new CreateOrderHandler(orderRepo, customerRepo, deviceRepo);
            var  getCustomerDeviceHandler= new GetCustomerDevicesHandler(deviceRepo);
            var  getCustomerLookupHandler= new GetCustomersLookupHandler(customerRepo);
            var getoderPaged =new GetPagedOrderSummariesHandler(orderRepo);
            var  getorderPaged= new GetPagedOrderSummariesHandler(orderRepo);
            var getOrderDetails = new GetOrderFullDetailsHandler(orderRepo);
            var updateOrder = new UpdateOrderHandler(orderRepo);
            var createCustomerHandler = new CreateCustomerHandler(customerRepo , phoneRepo);
            var getAllBrandsHandler = new GetAllBrandsHandler(brandRepo);
            var getAllTypesHandler = new GetAllTypesHandler(typeRepo);
            var getSpecsByTypeIdHandler = new GetSpecsByTypeIdHandler(specRepo);
            var getPagedCustomerHandler = new GetPagedCustomerSummariesHandler(customerRepo);
            var SearchOrderPage = new SearchOrderPageHandler(orderRepo);
            var SearchCustomerPage = new SearchCustomerPageHandler(customerRepo);
            var GetCustomerBasicInfo = new GetCustomerBasicInfoHandler(customerRepo);
            var updateCusotmer = new UpdateCustomerHandler(customerRepo);
            var getCustomerPhones = new GetCustomerPhonesHandler(phoneRepo);
            var addPhone = new AddPhoneToCustomer(phoneRepo);
            var deletePhone = new DeletePhoneHandler(phoneRepo);
            var UpdatePhone = new UpdatePhoneHandler(phoneRepo);
            var customerDevices = new GetCustomerDevicesHandler(deviceRepo);
            var addDevice = new AddDeviceToCustomerHandler(deviceRepo);
            var updateDevice= new UpdateDeviceHandler(deviceRepo);
            var getDeviceOrders = new GetDeviceOrders(orderRepo);
            var deleteDevice= new DeleteDeviceHandler(deviceRepo, getDeviceOrders);
            var GetCustomerOrders = new GetCustomerOrdersHandler(orderRepo);
            var deleteCustomer = new DeleteCustomerHandler (customerRepo);
            var getEmployeesLookup = new GetEmployeesLookupHandler(employeeRepo);
            var createAppointment = new CreateAppointmentHandler(appointmentRepo);
            var getAppointments = new GetAppointmentsByOrderIdHandler(appointmentRepo);
            var updateAppointment = new UpdateAppointmentHandler (appointmentRepo);
            var getAppointment = new GetAppointmentByIdHandler(appointmentRepo);
            var cancleAppointment = new CancelAppointmentHandler(appointmentRepo);
            var createVisit = new CreateVisitHandler(visitRepo );
            var getAllEmployees = new GetAllEmployeesLookupHandler (employeeRepo);
            var getVisitDetatils = new GetVisitDetailsHandler (visitRepo);
            var createCampaign = new CreateCampaignHandler(campaignRepo);
            var getAllSources = new GetAllSourcesHandler(sourcesRepo);
            var getCampaignPage = new GetPagedCampaignSummariesHandler(campaignRepo);
            var searchCampaign = new SearchCampaignsPaged(campaignRepo);
            var CampaignDetails = new GetCampaignDetailsHandler(campaignRepo);
            var deleteCampaign = new DeleteCampaignHandler (campaignRepo);
            var updateCampaign = new UpdateCampaignHandler(campaignRepo);
            var getCampaignLookup = new GetCampaignLookupHandler(campaignRepo);
            var createBrand = new CreateBrandHandler (brandRepo);
            var deleteBrand = new DeleteBrandHandler(brandRepo);
            var updateBrand = new UpdateBrandHandler(brandRepo);
            var addType = new AddTypeHandler(typeRepo);
            var deleteType = new DeleteTypeHandler(typeRepo);
            var updateType = new UpdateTypeHandler(typeRepo);
            var getAllSpecs = new GetAllSpecsHandler(specRepo);

            var addSpec = new AddSpecHandler(specRepo);

            var DeleteSpec = new DeleteSpecHandler(specRepo);

            var updateSpec = new UpdateSpecHandler(specRepo);

            var mainWindow = new MainWindow(getOrderDetails , getorderPaged  , getCustomerLookupHandler ,
                getCustomerDeviceHandler , createOrderHandler , updateOrder
                ,createCustomerHandler , getAllBrandsHandler , getAllTypesHandler ,getSpecsByTypeIdHandler 
                , getPagedCustomerHandler , SearchOrderPage , SearchCustomerPage , GetCustomerBasicInfo , updateCusotmer , getCustomerPhones 
                , addPhone , deletePhone , UpdatePhone  , customerDevices , addDevice ,
                updateDevice , deleteDevice , getDeviceOrders , GetCustomerOrders , deleteCustomer , getEmployeesLookup , createAppointment , 
                getAppointments , updateAppointment , getAppointment , cancleAppointment , createVisit , getAllEmployees 
                , getVisitDetatils , createCampaign , getAllSources , getCampaignPage , 
                searchCampaign , CampaignDetails , deleteCampaign , updateCampaign
                , getCampaignLookup , createBrand , deleteBrand , updateBrand , addType , deleteType , updateType
                , getAllSpecs , addSpec , DeleteSpec , updateSpec);
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