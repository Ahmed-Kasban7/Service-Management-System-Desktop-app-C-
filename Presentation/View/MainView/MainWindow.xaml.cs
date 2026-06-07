using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment.Commands;
using Application.Features.DeviceManagement.Commands;
using Application.Features.DeviceManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Application.Features.PhoneManagement.Commands;
using Application.Features.PhoneManagement.Queries;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement.Queries;
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
using Microsoft.Extensions.Configuration;

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
        private DeletePhoneHandler _deletePhoneHandler;
        private UpdatePhoneHandler _updatePhoneHandler;
        private GetCustomerDevicesHandler customerDevicesHandler;
        private readonly AddDeviceToCustomerHandler _addDeviceHandler;
        private readonly UpdateDeviceHandler _updateDeviceHandler;
        private readonly DeleteDeviceHandler _deleteDeviceHandler;
        private readonly GetDeviceOrders _getDeviceOrders;
        private readonly GetCustomerOrdersHandler _getCustomerOrdersHandler;
        private readonly DeleteCustomerHandler _deleteCustomerHandler;

        public MainWindow(GetOrderFullDetailsHandler getOrderFullDetails,
            GetPagedOrderSummariesHandler getPagedOrderSummaries, GetCustomersLookupHandler getCustomersLookup,
            GetCustomerDevicesHandler getCustomerDevicesHandler, CreateOrderHandler createOrderHandler
            , UpdateOrderHandler updateOrderHandler, CreateCustomerHandler createCustomer,
            GetAllBrandsHandler getAllBrands, GetAllTypesHandler getAllTypes, GetSpecsByTypeIdHandler getSpecsByTypeId,
            GetPagedCustomerSummariesHandler getPagedCustomer, SearchOrderPageHandler searchOrder,
            SearchCustomerPageHandler searchCustomerPage, GetCustomerBasicInfoHandler getCustomersBasicInfoHandler,
            UpdateCustomerHandler updateCustomerHandler, GetCustomerPhonesHandler getCustomerPhones,
            AddPhoneToCustomer addPhoneToCustomer,
            DeletePhoneHandler deletePhone, UpdatePhoneHandler updatePhone,
            GetCustomerDevicesHandler getCustomerDevices
            , AddDeviceToCustomerHandler addDeviceToCustomer
            , UpdateDeviceHandler updateDevice, DeleteDeviceHandler deleteDevice,
            GetDeviceOrders getDeviceOrders, GetCustomerOrdersHandler getCustomerOrdersHandler , DeleteCustomerHandler deleteCustomer)
        {
            InitializeComponent();
            LoadSavedLogo();
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
            _deletePhoneHandler = deletePhone;
            _updatePhoneHandler = updatePhone;
            customerDevicesHandler = getCustomerDevices;
            _addDeviceHandler = addDeviceToCustomer;
            _updateDeviceHandler = updateDevice;
            _deleteDeviceHandler = deleteDevice;
            _getDeviceOrders = getDeviceOrders;
            _updateCustomerHandler = updateCustomerHandler;
            _getCustomerOrdersHandler = getCustomerOrdersHandler;
            _deleteCustomerHandler = deleteCustomer;


            _createOrderHandler.AddOrderToList += OrdersControl.RefreshIfVisible;
        }

   
        public void OpenOrderDetailsFromCustomers(int orderId)
        {
            TabOrders.IsChecked = true;

            CustomersControl.Visibility = Visibility.Collapsed;
            SettingsControl.Visibility = Visibility.Collapsed;
            OrdersControl.Visibility = Visibility.Visible;

            SidePanelColumn.Width = new GridLength(0);

            OrdersControl.InitializeServices(_getPagedOrderSummariesHandler, _getOrderFullDetailsHandler, _updateOrderHandler, _SearchOrderPageHandler);

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

            selectedContent.Visibility = Visibility.Visible;
            SidePanelColumn.Width = new GridLength(0);

            InitializeTabServices(selectedContent);
        }

        private void InitializeTabServices(UserControl content)
        {
            if (content == OrdersControl)
            {
                OrdersControl.InitializeServices(_getPagedOrderSummariesHandler, _getOrderFullDetailsHandler, _updateOrderHandler, _SearchOrderPageHandler);

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
                    _getCustomerPhonesHandler, _addPhoneToCustomer, _deletePhoneHandler
                    , _updatePhoneHandler, customerDevicesHandler, _addDeviceHandler, 
                    _updateDeviceHandler, _deleteDeviceHandler, _getDeviceOrders ,
                    _getCustomerOrdersHandler , _deleteCustomerHandler);
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
            }
        }

        private void BtnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var createOrderWin = new CreateOrderWindow(_getCustomersLookupHandler, _getCustomerDevicesHandler, _createOrderHandler)
            {
                Owner = this
            };
            createOrderWin.ShowDialog();
        }

        private void LoadSavedLogo()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string savedPath = config["ImageSettings:LogoPath"];

                if (!string.IsNullOrEmpty(savedPath) && File.Exists(savedPath))
                {
                    CompanyLogo.Source = new BitmapImage(new Uri(savedPath));
                }
                else
                {
                    CompanyLogo.Source = null;
                }
            }
            catch (Exception)
            {
                CompanyLogo.Source = null;
            }
        }

        private void BtnChangeLogo_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                CompanyLogo.Source = new BitmapImage(new Uri(selectedFileName));

                try
                {
                    string json = File.ReadAllText("appsettings.json");
                    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    jsonObj["ImageSettings"]["LogoPath"] = selectedFileName;

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText("appsettings.json", output);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("فشل حفظ مسار الصورة في الإعدادات: " + ex.Message);
                }
            }
        }
    }
}