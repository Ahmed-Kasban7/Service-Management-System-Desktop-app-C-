using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement;
using Application.Features.BrandManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment;
using Application.Features.CustomerManagment.Commands;
using Application.Features.DeviceManagement;
using Application.Features.DeviceManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Application.Features.PhoneManagement;
using Application.Features.SpecManagement;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement;
using Application.Features.TypeManagement.Queries;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Presentation.View.Customer_View;
using Presentation.View.OrderView;
using Presentation.View.Settings_View;
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
        public MainWindow(GetOrderFullDetailsHandler getOrderFullDetails ,
            GetPagedOrderSummariesHandler getPagedOrderSummaries, GetCustomersLookupHandler getCustomersLookup,
            GetCustomerDevicesHandler getCustomerDevicesHandler, CreateOrderHandler createOrderHandler
            , UpdateOrderHandler updateOrderHandler , CreateCustomerHandler createCustomer,
            GetAllBrandsHandler getAllBrands, GetAllTypesHandler getAllTypes, GetSpecsByTypeIdHandler getSpecsByTypeId,
            GetPagedCustomerSummariesHandler getPagedCustomer)
        {
            InitializeComponent();
            LoadSavedLogo();
            TxtTodayDate.Text = DateTime.Now.ToString("dddd، dd MMMM yyyy", new CultureInfo("ar-EG"));
            _getOrderFullDetailsHandler = getOrderFullDetails;
            _getPagedOrderSummariesHandler = getPagedOrderSummaries;
            _getCustomerDevicesHandler = getCustomerDevicesHandler;
            _createOrderHandler = createOrderHandler;
            _getCustomerDevicesHandler = getCustomerDevicesHandler;
            _getCustomersLookupHandler = getCustomersLookup;
            _createOrderHandler = createOrderHandler;
            _updateOrderHandler = updateOrderHandler;
            _createCustomerHandler = createCustomer;
            _getAllBrandsHandler = getAllBrands;
            _getAllTypesHandler = getAllTypes;
            _getSpecsByTypeIdHandler = getSpecsByTypeId;
            _updateOrderHandler = updateOrderHandler;
            GetPagedCustomerSummariesHandler = getPagedCustomer;

            _createOrderHandler.AddOrderToList += OrdersControl.RefreshIfVisible;
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
                OrdersControl.InitializeServices(_getPagedOrderSummariesHandler, _getOrderFullDetailsHandler , _updateOrderHandler);

             if(content == CustomersControl)
                CustomersControl.InitializeServices(_createCustomerHandler , _getAllBrandsHandler , _getAllTypesHandler , _getSpecsByTypeIdHandler , GetPagedCustomerSummariesHandler);
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




















        private void LoadCustomers()
        {
            //try
            //{
            //    var customers = string.IsNullOrWhiteSpace(_searchWord)
            //        ? _customerService.GetPagedCustomerSummaries(CurrentPage, ROWPERPAGE)
            //        : _customerService.SearchCustomerPagedBy(_searchWord, CurrentPage, ROWPERPAGE);

            //    DgCustomers.ItemsSource = customers;

            //    if (customers == null || !customers.Any())
            //    {
            //        DgCustomers.Visibility = Visibility.Collapsed;
            //        EmptyStateOverlay.Visibility = Visibility.Visible;

            //        TxtEmptyMessage.Text = string.IsNullOrWhiteSpace(_searchWord)
            //            ? "لا يوجد عملاء مسجلين حالياً"
            //            : $"لا توجد نتائج للبحث عن: \"{_searchWord}\"";
            //    }
            //    else
            //    {
            //        DgCustomers.Visibility = Visibility.Visible;
            //        EmptyStateOverlay.Visibility = Visibility.Collapsed;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"خطأ في تحميل قائمة العملاء");
            //}
        }

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            //var clickedButton = sender as Button;
            //var selectedSummary = clickedButton?.DataContext as CustomerSummaryDto;

            //if (selectedSummary == null) return;

            //var result = MessageBox.Show(
            //    $"هل أنت متأكد من حذف العميل {selectedSummary.Name}؟",
            //    "تأكيد الحذف",
            //    MessageBoxButton.YesNo,
            //    MessageBoxImage.Warning);

            //if (result == MessageBoxResult.Yes)
            //{
            //    try
            //    {
            //        int customerId = int.Parse(selectedSummary.ID.Replace("C-", ""));

            //        var deleted = _customerService.DeleteCustomer(customerId);

            //        if (deleted.IsSuccess)
            //        {
            //            MessageBox.Show("تم حذف العميل بنجاح", "نجاح");
            //            UpdatePageInfo();

            //        }
            //        else
            //        {
            //            MessageBox.Show($"فشل في حذف العميل: {deleted.Error}");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"حدث خطأ: {ex.Message}");
            //    }
            //}
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {

            //if (e.Key == Key.Enter )
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
     

        //private int GetCurrentCustomerId()
        //{
        //    //if (!int.TryParse(_currentCustomer.ID.Replace("C-", ""), out int customerId))
        //    //{
        //    //    MessageBox.Show("رقم العميل غير صالح.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //}
        //    //return customerId;
        //}
        private void BtnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            //var createWin = new CreateCustomerWindow(_customerService, _deviceBrandService, _deviceTypeService, _deviceSpecService);

            //createWin.Owner = GetWindow(this);
            //createWin.ShowDialog();
        }
   
        //private void ReloadCustomerProfile(int customerId)
        //{

        //    _currentCustomer = _customerService.GetCustomerFullProfile(customerId);
        //    LoadCustomerProfile(_currentCustomer);

        //    LoadCustomers();

        //    var customers = DgCustomers.ItemsSource as IEnumerable<CustomerSummaryDto>;
        //    var updatedCustomer = customers.FirstOrDefault(c => c.ID == $"C-{customerId}");

        //    if (updatedCustomer != null)
        //    {
        //        DgCustomers.SelectedItem = updatedCustomer;
        //        DgCustomers.ScrollIntoView(updatedCustomer);
        //    }
        //}
        //private void LoadCustomerProfile(CustomerProfileDto customer)
        //{
        //    _currentCustomer = customer;

        //    TxtProfileID.Text = customer.ID;
        //    TxtProfileName.Text = customer.Name;
        //    TxtProfileAge.Text = customer.Age?.ToString() ?? "---";
        //    TxtProfileSex.Text = customer.Sex == ESex.MALE ? "ذكر" : "أنثى";
        //    TxtProfileAddress.Text = customer.Address;
        //    TxtProfileDiscount.Text = $"{customer.Discount}%";

        //    PhonesList.ItemsSource = customer.Phones;
        //    TxtNoPhonesMessage.Visibility = customer.Phones == null || customer.Phones.Count == 0
        //        ? Visibility.Visible : Visibility.Collapsed;

        //    DevicesList.ItemsSource = customer.Devices;
        //    TxtNoDevicesMessage.Visibility = customer.Devices == null || customer.Devices.Count == 0
        //        ? Visibility.Visible : Visibility.Collapsed;
        //}
        //private void SetButtonsEnabled(bool isEnabled)
        //{
        //    BtnEditCustomer.IsEnabled = isEnabled;
        //    btnAddNum.IsEnabled = isEnabled;
        //    btnAddDevice.IsEnabled = isEnabled;
        //}
    
        private void DgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (DgCustomers.SelectedItem is CustomerSummaryDto selectedSummary)
            //{
            //    ProfileSection.Visibility = Visibility.Visible;
            //    ProfileColumn.Width = new GridLength(450);
            //    ProfileSection.Opacity = 1.0;
            //    try
            //    {
            //        int customerId = int.Parse(selectedSummary.ID.Replace("C-", ""));
            //        var fullProfile = _customerService.GetCustomerFullProfile(customerId);

            //        if (fullProfile != null)
            //        {
            //            SetButtonsEnabled(true);
            //            LoadCustomerProfile(fullProfile);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"خطأ: {ex.Message}");
            //    }
            //}
            //else
            //{
            //    ProfileColumn.Width = new GridLength(0);
            //    ProfileSection.Visibility = Visibility.Collapsed;
            //}
        }

   

    }

}
