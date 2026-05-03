using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.Features.BrandManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment.Commands;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement.Queries;
using Presentation.View.Customer_View;
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

        private  CreateCustomerHandler _createCustomerHandler;

        private  GetAllBrandsHandler _getAllBrandsHandler;
        private  GetAllTypesHandler _getAllTypesHandler;
        private  GetSpecsByTypeIdHandler _getSpecsByTypeIdHandler;
        private  GetPagedCustomerSummariesHandler _getPagedCustomerSummariesHandler;
        private SearchCustomerPageHandler _searchCustomerPageHandler;
        public CustomersUC()
        {
            InitializeComponent();
        }
        public void InitializeServices(CreateCustomerHandler createCustomer, GetAllBrandsHandler getAllBrands, 
            GetAllTypesHandler getAllTypes, GetSpecsByTypeIdHandler getSpecsByTypeId,
            GetPagedCustomerSummariesHandler getPagedCustomer , SearchCustomerPageHandler searchCustomerPage)
        {

            _createCustomerHandler = createCustomer;
            _getAllBrandsHandler = getAllBrands;
            _getAllTypesHandler = getAllTypes;
            _getSpecsByTypeIdHandler = getSpecsByTypeId;
            _getPagedCustomerSummariesHandler = getPagedCustomer;
            _searchCustomerPageHandler = searchCustomerPage;

            createCustomer.CustomerCreated += LoadAndBindOrders;

            LoadAndBindOrders();
            //UpdatePageInfo();
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
        private void DgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgcustomers.selecteditem is customersummarydto selectedsummary)
            //{
            //    profilesection.visibility = visibility.visible;
            //    profilecolumn.width = new gridlength(450);
            //    profilesection.opacity = 1.0;
            //    try
            //    {
            //        int customerid = int.parse(selectedsummary.id.replace("c-", ""));
            //        var fullprofile = _customerservice.getcustomerfullprofile(customerid);

            //        if (fullprofile != null)
            //        {
            //            setbuttonsenabled(true);
            //            loadcustomerprofile(fullprofile);
            //        }
            //    }
            //    catch (exception ex)
            //    {
            //        messagebox.show($"خطأ: {ex.message}");
            //    }
            //}
            //else
            //{
            //    profilecolumn.width = new gridlength(0);
            //    profilesection.visibility = visibility.collapsed;
            //}
        }
        private void ReloadCustomerProfile(int customerId)
        {

            //_currentCustomer = _customerService.GetCustomerFullProfile(customerId);
            //LoadCustomerProfile(_currentCustomer);

            //LoadCustomers();

            //var customers = DgCustomers.ItemsSource as IEnumerable<CustomerSummaryDto>;
            //var updatedCustomer = customers.FirstOrDefault(c => c.ID == $"C-{customerId}");

            //if (updatedCustomer != null)
            //{
            //    DgCustomers.SelectedItem = updatedCustomer;
            //    DgCustomers.ScrollIntoView(updatedCustomer);
            //}
        }
        private void LoadCustomerProfile(CustomerProfileDto customer)
        {
            //_currentCustomer = customer;

            //TxtProfileID.Text = customer.ID;
            //TxtProfileName.Text = customer.Name;
            //TxtProfileAge.Text = customer.Age?.ToString() ?? "---";
            //TxtProfileSex.Text = customer.Sex == ESex.MALE ? "ذكر" : "أنثى";
            //TxtProfileAddress.Text = customer.Address;
            //TxtProfileDiscount.Text = $"{customer.Discount}%";

            //PhonesList.ItemsSource = customer.Phones;
            //TxtNoPhonesMessage.Visibility = (customer.Phones == null || customer.Phones.Count == 0)
            //    ? Visibility.Visible : Visibility.Collapsed;

            //DevicesList.ItemsSource = customer.Devices;
            //TxtNoDevicesMessage.Visibility = (customer.Devices == null || customer.Devices.Count == 0)
            //    ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
