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
        private string? _searchWord = null;

        private  CreateCustomerHandler _createCustomerHandler;

        private  GetAllBrandsHandler _getAllBrandsHandler;
        private  GetAllTypesHandler _getAllTypesHandler;
        private  GetSpecsByTypeIdHandler _getSpecsByTypeIdHandler;
        private  GetPagedCustomerSummariesHandler _getPagedCustomerSummariesHandler;
        public CustomersUC()
        {
            InitializeComponent();
        }
        public void InitializeServices(CreateCustomerHandler createCustomer, GetAllBrandsHandler getAllBrands, 
            GetAllTypesHandler getAllTypes, GetSpecsByTypeIdHandler getSpecsByTypeId,
            GetPagedCustomerSummariesHandler getPagedCustomer)
        {

            _createCustomerHandler = createCustomer;
            _getAllBrandsHandler = getAllBrands;
            _getAllTypesHandler = getAllTypes;
            _getSpecsByTypeIdHandler = getSpecsByTypeId;
            _getPagedCustomerSummariesHandler = getPagedCustomer;

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

        private PagedResult<CustomerSummaryDto> LoadOrders()
        {

            try
            {
                var res = _getPagedCustomerSummariesHandler.Handle(CurrentPage, ROWPERPAGE);

                if (res.IsSuccess)
                {
                    return res.Value;
                }
                else
                {
                    MessageBox.Show(res.Error);
                    return null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطاء فى تحميل العملاء");
                return null;
            }
        }
        public void LoadAndBindOrders()
        {
            var result = LoadOrders();
            if (result != null)
            {
                DgCustomers.ItemsSource = result.Items;
                TxtPageInfo.Text = CurrentPage.ToString();
                TxtCustomerCountNumber.Text = result.TotalCount.ToString();


                BtnNextPage.IsEnabled = result.HasNextPage;
                BtnPrevPage.IsEnabled = result.HasPreviousPage;
            }
        }
        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            LoadAndBindOrders();
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadAndBindOrders();
            }
        }

        //    private void LoadCustomers()
        //    {
        //        try
        //        {
        //            var customers = string.IsNullOrWhiteSpace(_searchWord)
        //                ? _customerService.GetPagedCustomerSummaries(CurrentPage, ROWPERPAGE)
        //                : _customerService.SearchCustomerPagedBy(_searchWord, CurrentPage, ROWPERPAGE);

        //            DgCustomers.ItemsSource = customers;

        //            if (customers == null || !customers.Any())
        //            {
        //                DgCustomers.Visibility = Visibility.Collapsed;
        //                EmptyStateOverlay.Visibility = Visibility.Visible;

        //                TxtEmptyMessage.Text = string.IsNullOrWhiteSpace(_searchWord)
        //                    ? "لا يوجد عملاء مسجلين حالياً"
        //                    : $"لا توجد نتائج للبحث عن: \"{_searchWord}\"";
        //            }
        //            else
        //            {
        //                DgCustomers.Visibility = Visibility.Visible;
        //                EmptyStateOverlay.Visibility = Visibility.Collapsed;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"خطأ في تحميل قائمة العملاء");
        //        }
        //    }

        //private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        //{
        //    //CurrentPage--;
        //    //ChangePage();
        //}

        //private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        //{
        //    //CurrentPage++;
        //    //ChangePage();
        //}

        //private void ChangePage()
        //{
        //    //LoadCustomers();
        //    //TxtPageInfo.Text = CurrentPage.ToString();

        //    //UpdateButtonPage();
        //}


        //private void UpdateButtonPage()
        //{

        //    //BtnPrevPage.IsEnabled = CurrentPage > 1;

        //    //BtnNextPage.IsEnabled = (CurrentPage < TotalPages);
        //}

        //private void UpdatePageInfo()
        //{
        //    //if (_searchWord == null)
        //    //    TotalCustomers = _customerService.GetCustomerCount();
        //    //else
        //    //    TotalCustomers = _customerService.GetSearchCustomerCount(_searchWord);

        //    //TxtCustomerCountNumber.Text = TotalCustomers.ToString();

        //    //TotalPages = (int)Math.Ceiling((double)TotalCustomers / ROWPERPAGE);

        //    //if (CurrentPage > TotalPages)
        //    //    CurrentPage = TotalPages;

        //    //if (TotalPages == 0)
        //    //    CurrentPage = 1;

        //    //TxtPageInfo.Text = CurrentPage.ToString();

        //    //LoadCustomers();

        //    //UpdateButtonPage();
        //}

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            //if (e.Key == Key.Enter)
            //{
            //    try
            //    {
            //        if (string.IsNullOrWhiteSpace(SearchBox.Text))
            //        {
            //            _searchWord = null;
            //            CurrentPage = 1;
            //            UpdatePageInfo();
            //            return;
            //        }

            //        var search = SearchBox.Text;

            //        if (SearchBox.Text.ToLower().StartsWith("c-"))
            //        {
            //            search = SearchBox.Text.ToLower().Replace("c-", "");
            //        }

            //        _searchWord = search;
            //        CurrentPage = 1;

            //        UpdatePageInfo();
            //        DgCustomers.SelectedItem = null;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"خطأ في تحميل العملاء: {ex.Message}");
            //    }
            //}

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
