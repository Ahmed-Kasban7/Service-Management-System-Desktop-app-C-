using Application.Common;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment;
using Application.Features.DeviceManagement;
using Application.Features.DeviceManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.PhoneManagement;
using Application.Features.SpecManagement;
using Application.Features.TypeManagement;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
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


namespace Presentation.View.Customer_View
{
    public partial class CustomerListView : Window
    {
        private int CurrentPage = 1;
        private const int ROWPERPAGE = 8;
        private int TotalPages ;
        private int TotalCustomers ;
        private string? _searchWord = null;



        private readonly CustomerService _customerService;
        private readonly DeviceBrandService _deviceBrandService;
        private readonly DeviceTypeService _deviceTypeService;
        private readonly DeviceSpecService _deviceSpecService;
        private readonly DeviceService _deviceService;
        private readonly PhoneService _phoneService;
        private CustomerProfileDto? _currentCustomer;
        private readonly GetCustomersLookupHandler _getCustomerLookupHandler;
        private readonly GetCustomerDevicesHandler _getCustomerDevicesHandler;
        private readonly CreateOrderHandler _createOrderHandler;

        public CustomerListView(CustomerService customerService, PhoneService phoneService
            , DeviceBrandService deviceBrandService, DeviceTypeService deviceTypeService, DeviceSpecService specService, DeviceService deviceService 
            , GetCustomersLookupHandler CustomerLookup
            , GetCustomerDevicesHandler CustomerDevices,
            CreateOrderHandler createOrder)
        {
            InitializeComponent();
            LoadSavedLogo();
            TxtTodayDate.Text = DateTime.Now.ToString("dddd، dd MMMM yyyy", new CultureInfo("ar-EG"));

            _customerService = customerService;
            _phoneService = phoneService;
            _deviceBrandService = deviceBrandService;
            _deviceSpecService = specService;
            _deviceService = deviceService;
            _deviceTypeService = deviceTypeService;
            _getCustomerLookupHandler = CustomerLookup;
            _getCustomerDevicesHandler = CustomerDevices;
            _createOrderHandler = createOrder;

          //  _customerService.CustomerAdded += UpdatePageInfo;

           UpdatePageInfo();

        }

        private void LoadCustomers()
        {
            try
            {
                var customers = string.IsNullOrWhiteSpace(_searchWord)
                    ? _customerService.GetPagedCustomerSummaries(CurrentPage, ROWPERPAGE)
                    : _customerService.SearchCustomerPagedBy(_searchWord, CurrentPage, ROWPERPAGE);

                DgCustomers.ItemsSource = customers;

                if (customers == null || !customers.Any())
                {
                    DgCustomers.Visibility = Visibility.Collapsed;
                    EmptyStateOverlay.Visibility = Visibility.Visible;

                    TxtEmptyMessage.Text = string.IsNullOrWhiteSpace(_searchWord)
                        ? "لا يوجد عملاء مسجلين حالياً"
                        : $"لا توجد نتائج للبحث عن: \"{_searchWord}\"";
                }
                else
                {
                    DgCustomers.Visibility = Visibility.Visible;
                    EmptyStateOverlay.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل قائمة العملاء: {ex.Message}");
            }
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
            ChangePage();
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            ChangePage();
        }

        private void ChangePage()
        {
            LoadCustomers();
            TxtPageInfo.Text = CurrentPage.ToString();

            UpdateButtonPage();
        }


        private void UpdateButtonPage()
        {

            BtnPrevPage.IsEnabled = CurrentPage > 1;

            BtnNextPage.IsEnabled = (CurrentPage < TotalPages);
        }

        private void UpdatePageInfo()
        {
            if (_searchWord == null)
                TotalCustomers = _customerService.GetCustomerCount();
            else
                TotalCustomers = _customerService.GetSearchCustomerCount(_searchWord);

            TxtCustomerCountNumber.Text = TotalCustomers.ToString();

            TotalPages = (int)Math.Ceiling((double)TotalCustomers / ROWPERPAGE);

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            if (TotalPages == 0)
                CurrentPage = 1;

            TxtPageInfo.Text = CurrentPage.ToString();

            LoadCustomers();

            UpdateButtonPage();
        }

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            var selectedSummary = clickedButton?.DataContext as CustomerSummaryDto;

            if (selectedSummary == null) return;

            var result = MessageBox.Show(
                $"هل أنت متأكد من حذف العميل {selectedSummary.Name}؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int customerId = int.Parse(selectedSummary.ID.Replace("C-", ""));

                    var deleted = _customerService.DeleteCustomer(customerId);

                    if (deleted.IsSuccess)
                    {
                        MessageBox.Show("تم حذف العميل بنجاح", "نجاح");
                        UpdatePageInfo();

                    }
                    else
                    {
                        MessageBox.Show($"فشل في حذف العميل: {deleted.Error}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"حدث خطأ: {ex.Message}");
                }
            }
        }
        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Enter )
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(SearchBox.Text))
                    {
                        _searchWord = null;
                        CurrentPage = 1;
                        UpdatePageInfo();
                        return;
                    }

                    var search = SearchBox.Text;

                    if (SearchBox.Text.ToLower().StartsWith("c-"))
                    {
                        search = SearchBox.Text.ToLower().Replace("c-", "");
                    }

                    _searchWord = search;
                    CurrentPage = 1;

                    UpdatePageInfo();
                    DgCustomers.SelectedItem = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطأ في تحميل العملاء: {ex.Message}");
                }
            }

        }
        private void BtnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCustomer == null)
            {
                MessageBox.Show("الرجاء اختيار عميل قبل التعديل.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int customerId = GetCurrentCustomerId();


            var customerUpdateDTO = new CustomerUpdateDto(customerId, _currentCustomer.Name, _currentCustomer.Age,
                _currentCustomer.Sex, _currentCustomer.Address, _currentCustomer.Discount);

            try
            {
                var editWindow = new EditCustomerView(_customerService, customerUpdateDTO, customerId)
                {
                    Owner = this
                };


                bool? dialogResult = editWindow.ShowDialog();


                if (dialogResult == true)
                {
                    ReloadCustomerProfile(customerUpdateDTO.Id);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetCurrentCustomerId()
        {
            if (!int.TryParse(_currentCustomer.ID.Replace("C-", ""), out int customerId))
            {
                MessageBox.Show("رقم العميل غير صالح.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return customerId;
        }
        private void BtnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var createWin = new CreateCustomerWindow(_customerService, _deviceBrandService, _deviceTypeService, _deviceSpecService);

            createWin.Owner = Window.GetWindow(this);
            createWin.ShowDialog();
        }
        private void BtnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCustomer == null)
                return;

            var input = Microsoft.VisualBasic.Interaction.InputBox(
                "أدخل رقم الهاتف الجديد:",
                "إضافة رقم هاتف",
                "");

            if (string.IsNullOrWhiteSpace(input))
                return;

            try
            {
                int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

                bool added = _phoneService.AddPhone(input, customerId);

                if (added)
                {
                    MessageBox.Show("تمت إضافة الرقم بنجاح", "نجاح",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    ReloadCustomerProfile(customerId);
                }
                else
                {
                    MessageBox.Show("فشل في إضافة الرقم", "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}");
            }
        }

        private void BtnEditPhone_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string oldPhone)
            {
                var newPhone = Microsoft.VisualBasic.Interaction.InputBox(
                    "عدل رقم الهاتف:",
                    "تعديل رقم",
                    oldPhone);

                if (string.IsNullOrWhiteSpace(newPhone) || newPhone == oldPhone)
                    return;

                try
                {
                    int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

                    bool updated = _phoneService.UpdatePhone(newPhone, oldPhone);

                    if (updated)
                    {
                        MessageBox.Show("تم تعديل الرقم بنجاح", "نجاح",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        ReloadCustomerProfile(customerId);

                    }
                    else
                    {
                        MessageBox.Show("فشل في تعديل الرقم", "خطأ",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"حدث خطأ: {ex.Message}");
                }
            }
        }
        private void BtnDeletePhone_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string phoneNumber)
            {
                var result = MessageBox.Show(
                    $"هل أنت متأكد من حذف الرقم {phoneNumber} ؟",
                    "تأكيد الحذف",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

                        bool deleted = _phoneService.DeletePhone(phoneNumber, customerId);

                        if (deleted)
                        {
                            MessageBox.Show("تم حذف الرقم بنجاح", "نجاح",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            ReloadCustomerProfile(customerId);

                        }
                        else
                        {
                            MessageBox.Show("فشل في حذف الرقم", "خطأ",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}");
                    }
                }
            }
        }
        private void ReloadCustomerProfile(int customerId)
        {

            _currentCustomer = _customerService.GetCustomerFullProfile(customerId);
            LoadCustomerProfile(_currentCustomer);

            LoadCustomers();

            var customers = DgCustomers.ItemsSource as IEnumerable<CustomerSummaryDto>;
            var updatedCustomer = customers.FirstOrDefault(c => c.ID == $"C-{customerId}");

            if (updatedCustomer != null)
            {
                DgCustomers.SelectedItem = updatedCustomer;
                DgCustomers.ScrollIntoView(updatedCustomer);
            }
        }
        private void LoadCustomerProfile(CustomerProfileDto customer)
        {
            _currentCustomer = customer;

            TxtProfileID.Text = customer.ID;
            TxtProfileName.Text = customer.Name;
            TxtProfileAge.Text = customer.Age?.ToString() ?? "---";
            TxtProfileSex.Text = customer.Sex == ESex.MALE ? "ذكر" : "أنثى";
            TxtProfileAddress.Text = customer.Address;
            TxtProfileDiscount.Text = $"{customer.Discount}%";

            PhonesList.ItemsSource = customer.Phones;
            TxtNoPhonesMessage.Visibility = (customer.Phones == null || customer.Phones.Count == 0)
                ? Visibility.Visible : Visibility.Collapsed;

            DevicesList.ItemsSource = customer.Devices;
            TxtNoDevicesMessage.Visibility = (customer.Devices == null || customer.Devices.Count == 0)
                ? Visibility.Visible : Visibility.Collapsed;
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
        private void SetButtonsEnabled(bool isEnabled)
        {
            BtnEditCustomer.IsEnabled = isEnabled;
            btnAddNum.IsEnabled = isEnabled;
            btnAddDevice.IsEnabled = isEnabled;
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
        private void BtnDeviceHistory_Click(object sender, RoutedEventArgs e)
        {

        }
    
        private void DgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgCustomers.SelectedItem is CustomerSummaryDto selectedSummary)
            {
                ProfileSection.Visibility = Visibility.Visible;
                ProfileColumn.Width = new GridLength(450);
                ProfileSection.Opacity = 1.0;
                try
                {
                    int customerId = int.Parse(selectedSummary.ID.Replace("C-", ""));
                    var fullProfile = _customerService.GetCustomerFullProfile(customerId);

                    if (fullProfile != null)
                    {
                        SetButtonsEnabled(true);
                        LoadCustomerProfile(fullProfile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"خطأ: {ex.Message}");
                }
            }
            else
            {
                ProfileColumn.Width = new GridLength(0);
                ProfileSection.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnEditDevice_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCustomer == null)
            {
               MessageBox.Show("الرجاء اختيار عميل أولاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
            {
                int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

                var updateWindow = new UpdateDeviceWindow(
                    selectedDevice,
                    _deviceService,
                    _deviceBrandService,
                    _deviceTypeService,
                    _deviceSpecService
                )
                {
                    Owner = this
                };

                bool? result = updateWindow.ShowDialog();

                if (result == true)
                {
                    ReloadCustomerProfile(customerId);
                }
            }
            else
            {
                MessageBox.Show("برجاء اختيار الجهاز الذي تريد تعديله");
            }
        }

        private void BtnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

            var AddWin = new AddDeviceWindow(customerId, _deviceService, _customerService, _deviceBrandService, _deviceTypeService, _deviceSpecService);

            AddWin.Owner = Window.GetWindow(this);

            if (AddWin.ShowDialog() == true)
            {

                ReloadCustomerProfile(customerId);
            }
        }

        private void BtnDeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCustomer == null)
                return;

            if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
            {
                var result = MessageBox.Show(
                    $"هل أنت متأكد من حذف الجهاز ؟",
                    "تأكيد الحذف",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));
                        bool deleted = _deviceService.DeleteCustomerDevice(selectedDevice.DeviceId);

                        if (deleted)
                        {
                            MessageBox.Show("تم حذف الجهاز بنجاح", "نجاح",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            ReloadCustomerProfile(customerId);
                        }
                        else
                        {
                            MessageBox.Show("فشل في حذف الجهاز", "خطأ",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}", "خطأ",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("الرجاء اختيار جهاز للحذف", "تنبيه",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSettingsTab_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersContainer == null || SettingsContent == null) return;

            CustomersContainer.Visibility = Visibility.Collapsed;
            SettingsContent.Visibility = Visibility.Visible;
            ProfileSection.Visibility = Visibility.Collapsed;
            ProfileColumn.Width = new GridLength(0);
            SettingsContent.InitializeServices(_deviceTypeService, _deviceBrandService, _deviceSpecService);

            BtnSettingsTab.Foreground = (Brush)new BrushConverter().ConvertFromString("#4169E1"); 
            BtnSettingsTab.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#4169E1");
            BtnSettingsTab.FontWeight = FontWeights.Bold;

            BtnCustomersTab.Foreground = (Brush)new BrushConverter().ConvertFromString("#6B7280"); 
            BtnCustomersTab.BorderBrush = Brushes.Transparent; 
            BtnCustomersTab.FontWeight = FontWeights.Normal;
        }
        private void BtnCustomersTab_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersContainer == null || SettingsContent == null) return;

            SettingsContent.Visibility = Visibility.Collapsed;
            CustomersContainer.Visibility = Visibility.Visible;

            BtnCustomersTab.Foreground = (Brush)new BrushConverter().ConvertFromString("#4169E1");
            BtnCustomersTab.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#4169E1");
            BtnCustomersTab.FontWeight = FontWeights.Bold;

            BtnSettingsTab.Foreground = (Brush)new BrushConverter().ConvertFromString("#6B7280");
            BtnSettingsTab.BorderBrush = Brushes.Transparent;
            BtnSettingsTab.FontWeight = FontWeights.Normal;
        }

        private void BtnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var createOrderWin = new CreateOrderWindow(_getCustomerLookupHandler , _getCustomerDevicesHandler ,_createOrderHandler);

            createOrderWin.Owner = Window.GetWindow(this);
            createOrderWin.ShowDialog();
        }
    }

}
