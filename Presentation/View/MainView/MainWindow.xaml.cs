using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Features.AppointmentManagement.Commands;
using Application.Features.AppointmentManagement.Queries;
using Application.Features.AttachmentManagement.Commands;
using Application.Features.AttachmentManagement.Queries;
using Application.Features.BrandManagement.Commands;
using Application.Features.BrandManagement.Queries;
using Application.Features.CampaignManagement.Command;
using Application.Features.CampaignManagement.Queries;
using Application.Features.CompanySettingsManagement;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment.Commands;
using Application.Features.DepartmentManagement;
using Application.Features.DeviceManagement.Commands;
using Application.Features.DeviceManagement.Queries;
using Application.Features.EmployeeManagement.Commands;
using Application.Features.EmployeeManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Application.Features.PhoneManagement.Commands;
using Application.Features.PhoneManagement.Queries;
using Application.Features.SourceManagement;
using Application.Features.SpecManagement.Commands;
using Application.Features.SpecManagement.Queries;
using Application.Features.TreasuryManagement;
using Application.Features.TypeManagement.Commands;
using Application.Features.TypeManagement.Queries;
using Application.Features.VisitManagement;
using Microsoft.Extensions.Configuration;
using OpenTK;
using Presentation.View.Customer_View;
using Presentation.View.OrderView;
using Presentation.View.Settings_View;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Presentation.View.MainView
{
    public partial class MainWindow : Window
    {
        private readonly GetPagedOrderSummariesHandler _getPagedOrderSummariesHandler;
        private readonly GetOrderFullDetailsHandler _getOrderFullDetailsHandler;
        private readonly GetCustomersLookupHandler _getCustomersLookupHandler;
        private readonly GetCustomerDevicesHandler _getCustomerDevicesHandler;
        private readonly CreateOrderHandler _createOrderHandler;
        private readonly UpdateOrderHandler _updateOrderHandler;
        private CreateCustomerHandler _createCustomerHandler;
        private GetAllBrandsHandler _getAllBrandsHandler;
        private GetAllTypesHandler _getAllTypesHandler;
        private GetSpecsByTypeIdHandler _getSpecsByTypeIdHandler;
        private GetPagedCustomerSummariesHandler GetPagedCustomerSummariesHandler;
        private SearchOrderPageHandler _SearchOrderPageHandler;
        private SearchCustomerPageHandler _SearchCustomerPageHandler;
        private GetCustomerBasicInfoHandler _getCustomersBasicInfoHandler;
        private UpdateCustomerHandler _updateCustomerHandler;
        private GetCustomerPhonesHandler _getCustomerPhonesHandler;
        private AddPhoneToCustomer _addPhoneToCustomer;
        private DeleteCustomerPhoneHandler _deleteCustomerPhoneHandler;
        private UpdatePhoneHandler _updatePhoneHandler;
        private GetCustomerDevicesHandler customerDevicesHandler;
        private readonly AddDeviceToCustomerHandler _addDeviceHandler;
        private readonly UpdateDeviceHandler _updateDeviceHandler;
        private readonly DeleteDeviceHandler _deleteDeviceHandler;
        private readonly GetDeviceOrders _getDeviceOrders;
        private readonly GetCustomerOrdersHandler _getCustomerOrdersHandler;
        private readonly DeleteCustomerHandler _deleteCustomerHandler;
        private readonly GetEmployeesLookupHandler _getEmployeesLookupHandler;
        private CreateAppointmentHandler _createAppointmentHandler;
        private GetAppointmentsByOrderIdHandler _getAppointments;
        private UpdateAppointmentHandler _updateAppointmentHandler;
        private GetAppointmentByIdHandler _getAppointmentByIdHandler;
        private readonly CancelAppointmentHandler _cancelAppointmentHandler; 
        private readonly CreateVisitHandler _createVisitHandler;
        private readonly GetAllEmployeesLookupHandler _getAllEmployees;
        private readonly GetVisitDetailsHandler _getVisitDetails;
        private readonly CreateCampaignHandler _createCampaignHandler;
        private readonly GetAllSourcesHandler _getAllSources;
        private readonly GetPagedCampaignSummariesHandler _getPagedCampaignSummaries;
        private readonly SearchCampaignsPaged _searchCampaignsPaged;
        private readonly GetCampaignDetailsHandler _getCampaignDetails;
        private readonly DeleteCampaignHandler _deleteCampaign;
        private readonly UpdateCampaignHandler _updateCampaign;
        private readonly GetCampaignLookupHandler _getCampaignLookup;
        private readonly CreateBrandHandler _createBrandHandler;
        private readonly DeleteBrandHandler _deleteBrand;
        private readonly UpdateBrandHandler _updateBrandHandler;
        private readonly AddTypeHandler _addTypeHandler;
        private readonly DeleteTypeHandler deleteTypeHandler;
        private readonly UpdateTypeHandler _updateTypeHandler;
        private readonly GetAllSpecsHandler _getAllSpecs;

        private readonly AddSpecHandler _addSpecHandler;
        private readonly DeleteSpecHandler _deleteSpecHandler;

        private readonly UpdateSpecHandler _updateSpecHandler;
        private readonly GetBalanceHandler _getBalanceHandler;
        private readonly CreateEmployeeHandler _createEmployeeHandler;
        private readonly GetDepartmentsLookupHandler _getDepartmentsHandler;
        private readonly GetRolesByDepartmentHandler _getRolesHandler;

        private readonly GetPagedEmployeeSummariesHandler _getPagedEmployeeSummariesHandler;
        private readonly SearchEmployeeHandler _searchEmployeeHandler;
        private readonly GetEmployeeProfileHandler _getEmployeeProfileHandler;

        private readonly GetEmployeePhonesHandler _getEmployeePhones;
        private readonly AddPhoneToEmployee _addPhoneToEmployee;
        private readonly DeleteEmployeePhoneHandler _deleteEmployeePhone;
        private readonly GetEmployeeAttachmentsHandler _getEmployeeAttachments;
        private readonly AddAttachmentHandler _addAttachmentHandler;
        private readonly DeleteAttachmentHandler _deleteAttachmentHandler;

        private readonly GetCampaignCustomersCount _getCampaignCustomers;
        private readonly DeleteDepartmentHandler _deleteDepartment;
        private readonly UpdateDepartmentHandler _updateDepartment;
        private readonly AddDepartmentHandler _addDepartment;

        private readonly AddRoleHandler _addRoleHandler;
        private readonly DeleteRoleHandler _deleteRoleHandler;
        private readonly UpdateRoleHandler _updateRoleHandler;
        private readonly GetAllDepartmentRolesHandler _getAllDepartmentRolesHandler;

        private readonly AddSourceHandler _addSourceHandler;
        private readonly DeleteSourceHandler _deleteSourceHandler;
        private readonly UpdateSourceHandler _updateSourceHandler;
        private readonly GetCompanySettingsHandler _getCompanySettingsHandler;
        private readonly UpdateCompanySettingsHandler _updateCompanySettings;

        private readonly UpdateEmployeeHandler _updateEmployeeHandler;

        public MainWindow(GetOrderFullDetailsHandler getOrderFullDetails,
            GetPagedOrderSummariesHandler getPagedOrderSummaries, GetCustomersLookupHandler getCustomersLookup,
            GetCustomerDevicesHandler getCustomerDevicesHandler, CreateOrderHandler createOrderHandler
            , UpdateOrderHandler updateOrderHandler, CreateCustomerHandler createCustomer,
            GetAllBrandsHandler getAllBrands, GetAllTypesHandler getAllTypes, GetSpecsByTypeIdHandler getSpecsByTypeId,
            GetPagedCustomerSummariesHandler getPagedCustomer, SearchOrderPageHandler searchOrder,
            SearchCustomerPageHandler searchCustomerPage, GetCustomerBasicInfoHandler getCustomersBasicInfoHandler,
            UpdateCustomerHandler updateCustomerHandler, GetCustomerPhonesHandler getCustomerPhones,
            AddPhoneToCustomer addPhoneToCustomer,
            DeleteCustomerPhoneHandler deletePhone, UpdatePhoneHandler updatePhone,
            GetCustomerDevicesHandler getCustomerDevices
            , AddDeviceToCustomerHandler addDeviceToCustomer
            , UpdateDeviceHandler updateDevice, DeleteDeviceHandler deleteDevice,
            GetDeviceOrders getDeviceOrders, GetCustomerOrdersHandler getCustomerOrdersHandler ,
            DeleteCustomerHandler deleteCustomer ,  GetEmployeesLookupHandler getEmployees , 
            CreateAppointmentHandler createAppointment , GetAppointmentsByOrderIdHandler getAppointments ,
            UpdateAppointmentHandler updateAppointment , GetAppointmentByIdHandler 
            getAppointment , CancelAppointmentHandler cancelAppointment  , CreateVisitHandler createVisit ,
            GetAllEmployeesLookupHandler getAllEmployees , GetVisitDetailsHandler getVisitDetails ,
            CreateCampaignHandler createCampaign , GetAllSourcesHandler getAllSources ,
            GetPagedCampaignSummariesHandler getPagedCampaign , SearchCampaignsPaged searchCampaigns 
            , GetCampaignDetailsHandler detailsHandler , DeleteCampaignHandler deleteCampaign ,
            UpdateCampaignHandler updateCampaign , GetCampaignLookupHandler getCampaignLookup, 
            CreateBrandHandler createBrandHandler , DeleteBrandHandler deleteBrand , 
            UpdateBrandHandler updateBrand , AddTypeHandler addTypeHandler , 
            DeleteTypeHandler deleteType  , UpdateTypeHandler updateType , 
            GetAllSpecsHandler getAllSpecs , AddSpecHandler addSpecHandler , DeleteSpecHandler deleteSpec ,
            UpdateSpecHandler updateSpecHandler , GetBalanceHandler getBalanceHandler, CreateEmployeeHandler createEmployee,
            GetDepartmentsLookupHandler getDepartments,
            GetRolesByDepartmentHandler getRoles , GetPagedEmployeeSummariesHandler getPagedEmployee , 
            SearchEmployeeHandler searchEmployee ,
            GetEmployeeProfileHandler getEmployeeProfile , GetEmployeePhonesHandler getEmployeePhones ,
            AddPhoneToEmployee addPhoneToEmployee , DeleteEmployeePhoneHandler deleteEmployeePhone
            , GetEmployeeAttachmentsHandler getEmployeeAttachments , 
            AddAttachmentHandler addAttachmentHandler, DeleteAttachmentHandler deleteAttachment ,
            GetCampaignCustomersCount getCampaignCustomers
            , DeleteDepartmentHandler  deleteDepartment ,
            UpdateDepartmentHandler updateDepartment ,
            AddDepartmentHandler addDepartment , AddRoleHandler addRoleHandler 
            , DeleteRoleHandler deleteRoleHandler , UpdateRoleHandler updateRoleHandler ,
            GetAllDepartmentRolesHandler getAllDepartmentRolesHandler ,
            AddSourceHandler addSourceHandler, UpdateSourceHandler updateSource, 
            DeleteSourceHandler deleteSource , GetCompanySettingsHandler getCompanySettings , 
            UpdateCompanySettingsHandler updateCompanySettings  , UpdateEmployeeHandler updateEmployeeHandler)
        { 

             InitializeComponent();
            this.Closing += MainWindow_Closing;
            TxtTodayDate.Text = DateTime.Now.ToString("dddd، dd MMMM yyyy", new CultureInfo("ar-EG"));
            _getOrderFullDetailsHandler = getOrderFullDetails;
            _getPagedOrderSummariesHandler = getPagedOrderSummaries;
            _getCustomerDevicesHandler = getCustomerDevicesHandler;
            _createOrderHandler = createOrderHandler;
            _getCustomersLookupHandler = getCustomersLookup;
            _updateOrderHandler = updateOrderHandler;
            _createCustomerHandler = createCustomer;
            _getAllBrandsHandler = getAllBrands;
            _getAllTypesHandler = getAllTypes;
            _getSpecsByTypeIdHandler = getSpecsByTypeId;
            GetPagedCustomerSummariesHandler = getPagedCustomer;
            _SearchOrderPageHandler = searchOrder;
            _SearchCustomerPageHandler = searchCustomerPage;
            _getCustomersBasicInfoHandler = getCustomersBasicInfoHandler;
            _getCustomerPhonesHandler = getCustomerPhones;
            _addPhoneToCustomer = addPhoneToCustomer;
            _deleteCustomerPhoneHandler = deletePhone;
            _updatePhoneHandler = updatePhone;
            customerDevicesHandler = getCustomerDevices;
            _addDeviceHandler = addDeviceToCustomer;
            _updateDeviceHandler = updateDevice;
            _deleteDeviceHandler = deleteDevice;
            _getDeviceOrders = getDeviceOrders;
            _updateCustomerHandler = updateCustomerHandler;
            _getCustomerOrdersHandler = getCustomerOrdersHandler;
            _deleteCustomerHandler = deleteCustomer;

            _getEmployeesLookupHandler = getEmployees;
            _createAppointmentHandler = createAppointment;
            _getAppointments = getAppointments;
            _updateAppointmentHandler   = updateAppointment;
            _getAppointmentByIdHandler = getAppointment;
            _cancelAppointmentHandler = cancelAppointment;
            _createVisitHandler = createVisit;
            _getAllEmployees = getAllEmployees;
            _getVisitDetails = getVisitDetails;

            _createCampaignHandler = createCampaign;
            _getAllSources = getAllSources;
            _getPagedCampaignSummaries = getPagedCampaign;
            _searchCampaignsPaged = searchCampaigns;
            _getCampaignDetails = detailsHandler;
            _deleteCampaign = deleteCampaign;
            _updateCampaign = updateCampaign;
            _getCampaignLookup = getCampaignLookup;
            _createBrandHandler = createBrandHandler;
            _deleteBrand = deleteBrand;
            _updateBrandHandler = updateBrand;
            _addTypeHandler = addTypeHandler;
            deleteTypeHandler = deleteType;
            _updateTypeHandler = updateType;
            _getAllSpecs = getAllSpecs;

            _addSpecHandler = addSpecHandler;
            _deleteSpecHandler = deleteSpec;

            _updateSpecHandler = updateSpecHandler;

            _getBalanceHandler = getBalanceHandler;

            _createEmployeeHandler = createEmployee;
            _getDepartmentsHandler = getDepartments;
            _getRolesHandler = getRoles;

            _getPagedEmployeeSummariesHandler = getPagedEmployee;

            _searchEmployeeHandler = searchEmployee;
            _getEmployeeProfileHandler = getEmployeeProfile;
            _getEmployeePhones = getEmployeePhones;
            _addPhoneToEmployee = addPhoneToEmployee;
            _deleteEmployeePhone = deleteEmployeePhone;

             _getEmployeeAttachments = getEmployeeAttachments;
            _addAttachmentHandler = addAttachmentHandler;
            _deleteAttachmentHandler = deleteAttachment;
            _getCampaignCustomers = getCampaignCustomers;
            _addDepartment = addDepartment;
            _deleteDepartment = deleteDepartment;
            _updateDepartment = updateDepartment;

            _addRoleHandler = addRoleHandler;
            _deleteRoleHandler = deleteRoleHandler;
            _updateRoleHandler = updateRoleHandler;
            _getAllDepartmentRolesHandler = getAllDepartmentRolesHandler;
            _addSourceHandler = addSourceHandler;
            _updateSourceHandler = updateSource;
            _deleteSourceHandler = deleteSource;
            _getCompanySettingsHandler = getCompanySettings;
            _updateCompanySettings =    updateCompanySettings;
            _updateEmployeeHandler = updateEmployeeHandler;

            _createOrderHandler.AddOrderToList += OrdersControl.RefreshIfVisible;
            LoadSavedLogo();
            InitializeTabServices(DashboardControl);
        }

   
        public void OpenOrderDetailsFromCustomers(int orderId)
        {
            TabOrders.IsChecked = true;

            CustomersControl.Visibility = Visibility.Collapsed;
            SettingsControl.Visibility = Visibility.Collapsed;
            OrdersControl.Visibility = Visibility.Visible;

            SidePanelColumn.Width = new GridLength(0);

            OrdersControl.InitializeServices(_getPagedOrderSummariesHandler,
                _getOrderFullDetailsHandler, _updateOrderHandler, _SearchOrderPageHandler , 
                _getEmployeesLookupHandler , _createAppointmentHandler , _getAppointments ,
                _updateAppointmentHandler , _getAppointmentByIdHandler , _cancelAppointmentHandler , _createVisitHandler , _getAllEmployees , _getVisitDetails);

            OrdersControl.NavigateToOrderDetails(orderId, isComingFromDevices: true);
        }

     
        public void ReturnToDeviceHistory()
        {
            TabCustomers.IsChecked = true;

            OrdersControl.Visibility = Visibility.Collapsed;
            CustomersControl.Visibility = Visibility.Visible;

        }
    

        private void SwitchToTab(UserControl selectedContent, RadioButton selectedTab)
        {
            CustomersControl.Visibility = Visibility.Collapsed;
            OrdersControl.Visibility = Visibility.Collapsed;
            SettingsControl.Visibility = Visibility.Collapsed;
            CampaignsControl.Visibility = Visibility.Collapsed;
            DashboardControl.Visibility = Visibility.Collapsed;
            EmployeeControl.Visibility = Visibility.Collapsed;

            selectedContent.Visibility = Visibility.Visible;
            SidePanelColumn.Width = new GridLength(0);

            InitializeTabServices(selectedContent);
        }

        private void InitializeTabServices(UserControl content)
        {
            if (content == OrdersControl)
            {
                OrdersControl.InitializeServices(_getPagedOrderSummariesHandler, 
                    _getOrderFullDetailsHandler, _updateOrderHandler, _SearchOrderPageHandler
                    , _getEmployeesLookupHandler , _createAppointmentHandler , _getAppointments ,
                    _updateAppointmentHandler , _getAppointmentByIdHandler , _cancelAppointmentHandler, _createVisitHandler , _getAllEmployees , _getVisitDetails);

                OrdersControl.OrdersListPanel.Visibility = Visibility.Visible;

                if (OrdersControl.OrderDetailsHolder != null)
                {
                    OrdersControl.OrderDetailsHolder.Content = null;
                    OrdersControl.OrderDetailsHolder.Visibility = Visibility.Collapsed;
                }
            }

            if (content == CustomersControl)
            {
                CustomersControl.InitializeServices(_createCustomerHandler, _getAllBrandsHandler
                    , _getAllTypesHandler, _getSpecsByTypeIdHandler, GetPagedCustomerSummariesHandler,
                    _SearchCustomerPageHandler, _getCustomersBasicInfoHandler, _updateCustomerHandler,
                    _getCustomerPhonesHandler, _addPhoneToCustomer, _deleteCustomerPhoneHandler
                    , _updatePhoneHandler, customerDevicesHandler, _addDeviceHandler, 
                    _updateDeviceHandler, _deleteDeviceHandler, _getDeviceOrders ,
                    _getCustomerOrdersHandler , _deleteCustomerHandler , _getAllSources , _getCampaignLookup);
            }

            if(content == CampaignsControl)
            {
                CampaignsControl.InitializeServices(_createCampaignHandler , _getAllSources , 
                    _getPagedCampaignSummaries , _searchCampaignsPaged , _getCampaignDetails , _deleteCampaign , _updateCampaign , _getCampaignCustomers );
            }

            if(content == SettingsControl)
            {
                SettingsControl.InitializeServices(_getAllTypesHandler , _getAllBrandsHandler ,
                    _getSpecsByTypeIdHandler , _createBrandHandler , _deleteBrand , _updateBrandHandler ,
                    _addTypeHandler , deleteTypeHandler , _updateTypeHandler , _getAllSpecs , _addSpecHandler 
                    , _deleteSpecHandler , _updateSpecHandler , _deleteDepartment ,
                    _updateDepartment , _addDepartment , _getDepartmentsHandler , _addRoleHandler , 
                    _deleteRoleHandler  , _updateRoleHandler , _getAllDepartmentRolesHandler  
                    , _getAllSources , _addSourceHandler , _updateSourceHandler , _deleteSourceHandler , _updateCompanySettings , _getCompanySettingsHandler);
            }

            if(content == DashboardControl)
            {
                DashboardControl.InitializeServices(_getBalanceHandler);
            }

            if( content == EmployeeControl)
            {
                EmployeeControl.InitializeServices(_createEmployeeHandler , 
                    _getDepartmentsHandler ,_getRolesHandler ,
                    _getPagedEmployeeSummariesHandler , _searchEmployeeHandler , 
                    _getEmployeeProfileHandler , _getEmployeePhones , _addPhoneToEmployee , _updatePhoneHandler  ,
                    _deleteEmployeePhone , _getEmployeeAttachments , _addAttachmentHandler , _deleteAttachmentHandler , _updateEmployeeHandler );
            }
        }

        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as RadioButton;
            string target = btn.Tag.ToString();

            switch (target)
            {
                case "Customers":
                    SwitchToTab(CustomersControl, btn);
                    break;
                case "Orders":
                    SwitchToTab(OrdersControl, btn);
                    break;
                case "Settings":
                    SwitchToTab(SettingsControl, btn);
                    break;
                case "Campaign":
                    SwitchToTab(CampaignsControl, btn);
                    break;
                case "dashboard":
                    SwitchToTab(DashboardControl, btn);
                    break;
                case "Employees":
                    SwitchToTab(EmployeeControl, btn);
                    break;


            }
        }

        private void BtnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var createOrderWin = new CreateOrderWindow(_getCustomersLookupHandler,
                _getCustomerDevicesHandler, _createOrderHandler , _createCustomerHandler 
                , _getAllBrandsHandler, _getAllTypesHandler  , _getSpecsByTypeIdHandler , _addDeviceHandler , 
                _getAllBrandsHandler , _getAllTypesHandler  , _getSpecsByTypeIdHandler , _getAllSources , _getCampaignLookup)
            {
                Owner = this
            };
            createOrderWin.ShowDialog();
        }

        public void LoadSavedLogo()
        {
            try
            {
                var companySettings = _getCompanySettingsHandler.Handle();

                if (companySettings != null)
                {
                    TxtCompanyName.Text = companySettings.CompanyName;

                    if (companySettings.CompanyLogo != null && companySettings.CompanyLogo.Length > 0)
                    {
                        using var stream = new System.IO.MemoryStream(companySettings.CompanyLogo);
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();

                        CompanyLogo.Source = bitmap;
                    }
                    else
                    {
                        CompanyLogo.Source = null; 
                    }
                }
            }
            catch (Exception)
            {
                CompanyLogo.Source = null;
                TxtCompanyName.Text = "Pro Fix Company";
            }
        }



        public void OpenOrderDetailsAfterCreate(int orderId)
        {
            TabOrders.IsChecked = true;

            CustomersControl.Visibility = Visibility.Collapsed;
            SettingsControl.Visibility = Visibility.Collapsed;
            CampaignsControl.Visibility = Visibility.Collapsed;
            DashboardControl.Visibility = Visibility.Collapsed;
            EmployeeControl.Visibility = Visibility.Collapsed;

            OrdersControl.Visibility = Visibility.Visible;

            SidePanelColumn.Width = new GridLength(0);

            OrdersControl.InitializeServices(
                _getPagedOrderSummariesHandler,
                _getOrderFullDetailsHandler,
                _updateOrderHandler,
                _SearchOrderPageHandler
                , _getEmployeesLookupHandler
                , _createAppointmentHandler , _getAppointments , _updateAppointmentHandler , 
                _getAppointmentByIdHandler , _cancelAppointmentHandler , _createVisitHandler , _getAllEmployees , _getVisitDetails
            );

            OrdersControl.NavigateToOrderDetails(orderId, isComingFromDevices: false);
        }

        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            this.Cursor = Cursors.Wait;

            try
            {
                await RunAutomaticBackupAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Shutdown Backup Error: {ex.Message}");
            }
            finally
            {
                this.Cursor = Cursors.Arrow;

                this.Closing -= MainWindow_Closing;

                System.Windows.Application.Current.Shutdown();
            }
        }
        private System.Threading.Tasks.Task RunAutomaticBackupAsync()
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    string backupFolder = @"D:\Database_Backup";
                    if (!Directory.Exists(backupFolder))
                    {
                        Directory.CreateDirectory(backupFolder);
                    }

                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();

                    string connectionString = configuration.GetConnectionString("DefaultConnection")
                             ?? throw new Exception("ملف اعدادات قاعده البيانات غير موجود ");

                    string databaseName = configuration["DatabaseSettings:DatabaseName"]
                       ?? throw new Exception("اسم قاعده البيانات غير موجود فى  ملف الاعدادات");


                    using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                    {
                        using (var command = new Microsoft.Data.SqlClient.SqlCommand("SP_Backup", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@databaseName", databaseName);

                            connection.Open();
                            command.CommandTimeout = 60;

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Auto Backup Failed: {ex.Message}");
                }
            });
        }


    }

}