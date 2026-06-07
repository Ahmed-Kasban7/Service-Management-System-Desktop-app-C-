using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.OrderDTOs;
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
using Presentation.View.MainView; // تأكد من Namespace الخاص بـ MainWindow
using Presentation.View.OrderView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation.View.CustomerView
{
    /// <summary>
    /// Interaction logic for CustomersUC.xaml
    /// </summary>
    public partial class CustomersUC : UserControl
    {
        private int CurrentPage = 1;
        private const int ROWPERPAGE = 8;
        private int TotalPages;
        private int TotalCustomers;
        private bool IsSearching = false;
        private string CurrentSearchText = "";

        private CreateCustomerHandler _createCustomerHandler;
        private GetAllBrandsHandler _getAllBrandsHandler;
        private GetAllTypesHandler _getAllTypesHandler;
        private GetSpecsByTypeIdHandler _getSpecsByTypeIdHandler;
        private GetPagedCustomerSummariesHandler _getPagedCustomerSummariesHandler;
        private SearchCustomerPageHandler _searchCustomerPageHandler;
        private GetCustomerBasicInfoHandler _getCustomerBasicInfoHandler;
        private UpdateCustomerHandler _updateCustomerHandler;
        private GetCustomerPhonesHandler _getCustomerPhonesHandler;
        private AddPhoneToCustomer _addPhoneToCustomer;
        private DeletePhoneHandler _deletePhoneHandler;
        private UpdatePhoneHandler _updatePhoneHandler;
        private GetCustomerDevicesHandler _getCustomerDevicesHandler;
        private AddDeviceToCustomerHandler _addDeviceToCustomer;
        private UpdateDeviceHandler _updateDeviceHandler;
        private DeleteDeviceHandler _deleteDeviceHandler;
        private GetDeviceOrders _getDeviceOrders;
        private  GetCustomerOrdersHandler _getCustomerOrdersHandler;
        private DeleteCustomerHandler _deleteCustomerHandler;

        public event EventHandler<int> OrderDetailsRequested;

        public CustomersUC()
        {
            InitializeComponent();
        }

        public void InitializeServices(CreateCustomerHandler createCustomer, GetAllBrandsHandler getAllBrands,
            GetAllTypesHandler getAllTypes, GetSpecsByTypeIdHandler getSpecsByTypeId,
            GetPagedCustomerSummariesHandler getPagedCustomer, SearchCustomerPageHandler searchCustomerPage,
            GetCustomerBasicInfoHandler getCustomerBasicInfoHandler, UpdateCustomerHandler updateCustomer
            , GetCustomerPhonesHandler getCustomerPhones, AddPhoneToCustomer phoneToCustomer,
            DeletePhoneHandler deletePhone, UpdatePhoneHandler updatePhone
            , GetCustomerDevicesHandler getCustomerDevices,
            AddDeviceToCustomerHandler addDeviceToCustomer
            , UpdateDeviceHandler updateDevice, DeleteDeviceHandler deleteDevice, 
            GetDeviceOrders getDeviceOrders, GetCustomerOrdersHandler getCustomerOrdersHandler , DeleteCustomerHandler deleteCustomer)
        {
            _createCustomerHandler = createCustomer;
            _getAllBrandsHandler = getAllBrands;
            _getAllTypesHandler = getAllTypes;
            _getSpecsByTypeIdHandler = getSpecsByTypeId;
            _getPagedCustomerSummariesHandler = getPagedCustomer;
            _searchCustomerPageHandler = searchCustomerPage;
            _getCustomerBasicInfoHandler = getCustomerBasicInfoHandler;
            _updateCustomerHandler = updateCustomer;
            _getCustomerPhonesHandler = getCustomerPhones;
            _addPhoneToCustomer = phoneToCustomer;
            _deletePhoneHandler = deletePhone;
            _updatePhoneHandler = updatePhone;
            _getCustomerDevicesHandler = getCustomerDevices;
            _addDeviceToCustomer = addDeviceToCustomer;
            _updateDeviceHandler = updateDevice;
            _deleteDeviceHandler = deleteDevice;
            _getDeviceOrders = getDeviceOrders; 
            _getCustomerOrdersHandler = getCustomerOrdersHandler;
            _deleteCustomerHandler = deleteCustomer;


            createCustomer.CustomerCreated += LoadAndBindOrders;

            LoadAndBindOrders();
        }

        private void BtnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var createOrderWin = new CreateCustomerWindow(
                _createCustomerHandler,
                _getAllBrandsHandler,
                _getAllTypesHandler,
                _getSpecsByTypeIdHandler)
            {
                Owner = Window.GetWindow(this)
            };

            createOrderWin.ShowDialog();
        }

        private PagedResult<CustomerSummaryDto> LoadCustomers()
        {
            try
            {
                return _getPagedCustomerSummariesHandler.Handle(CurrentPage, ROWPERPAGE);
            }
            catch
            {
                MessageBox.Show("خطأ في تحميل العملاء");
                return new PagedResult<CustomerSummaryDto>(
                    new List<CustomerSummaryDto>(), 0, 1, ROWPERPAGE);
            }
        }

        public void LoadAndBindOrders()
        {
            PagedResult<CustomerSummaryDto> result;

            if (IsSearching && !string.IsNullOrWhiteSpace(CurrentSearchText))
            {
                result = _searchCustomerPageHandler.Handle(
                    CurrentSearchText,
                    CurrentPage,
                    ROWPERPAGE);
            }
            else
            {
                result = LoadCustomers();
            }

            Bind(result);
        }

        private void Bind(PagedResult<CustomerSummaryDto> result)
        {
            if (result == null) return;

            DgCustomers.ItemsSource = result.Items;
            TxtPageInfo.Text = CurrentPage.ToString();
            TxtCustomerCountNumber.Text = result.TotalCount.ToString();

            BtnNextPage.IsEnabled = result.HasNextPage;
            BtnPrevPage.IsEnabled = result.HasPreviousPage;
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (!BtnNextPage.IsEnabled) return;

            CurrentPage++;
            LoadAndBindOrders();
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage <= 1) return;

            CurrentPage--;
            LoadAndBindOrders();
        }

        private void DgCustomers_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (DgCustomers.SelectedItem is CustomerSummaryDto selectedCustomer)
            {
                var customerProfileUC = new CustomerProfileUC(selectedCustomer.customerId, _getCustomerBasicInfoHandler,
                    _updateCustomerHandler, _getCustomerPhonesHandler, _addPhoneToCustomer
                    , _deletePhoneHandler, _updatePhoneHandler, _getCustomerDevicesHandler, _addDeviceToCustomer
                    , _getAllBrandsHandler, _getAllTypesHandler, _getSpecsByTypeIdHandler,
                    _updateDeviceHandler, _deleteDeviceHandler, _getDeviceOrders , _getCustomerOrdersHandler , _deleteCustomerHandler);

                customerProfileUC.OrderDetailsRequested += (s, orderId) =>
                {
                    if (Window.GetWindow(this) is MainWindow mainWindow)
                    {
                        mainWindow.OpenOrderDetailsFromCustomers(orderId);
                    }
                };

                customerProfileUC.BackRequested += (s, args) =>
                {
                    CustomerProfileHolder.Visibility = Visibility.Collapsed;
                    CustomerProfileHolder.Content = null;
                    CustomersContainer.Visibility = Visibility.Visible;
                    LoadAndBindOrders();

                };

                CustomerProfileHolder.Content = customerProfileUC;
                CustomersContainer.Visibility = Visibility.Collapsed;
                CustomerProfileHolder.Visibility = Visibility.Visible;
            }
        }

        private void ResetSearch()
        {
            IsSearching = false;
            CurrentSearchText = "";
            CurrentPage = 1;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrentPage = 1;
                IsSearching = true;
                CurrentSearchText = SearchBox.Text;

                LoadAndBindOrders();
            }
        }

        private void ReloadCustomerProfile(int customerId) { }
        private void LoadCustomerProfile(CustomerBasicInfoDto customer) { }
    }
}