using Application.Common;
using Application.Repositories;
using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.Services;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Button = System.Windows.Controls.Button;
using System.Net.NetworkInformation;


namespace Presentation.View.Customer_View
{
    public partial class CustomerListView : Window
    {
        private int CurrentPage = 1;
        private const int ROWPERPAGE = 8;
        private int TotalPages;
        private int TotalCustomers;

      
        

        private readonly CustomerService _customerService;
        private readonly DeviceBrandService _deviceBrandService;
        private readonly DeviceTypeService _deviceTypeService;
        private readonly DeviceSpecService _deviceSpecService;
        private readonly DeviceService _deviceService;
        private readonly PhoneService _phoneService;
        private CustomerProfileDTO? _currentCustomer;

        public CustomerListView(CustomerService customerService, PhoneService phoneService
            , DeviceBrandService deviceBrandService, DeviceTypeService deviceTypeService, DeviceSpecService specService, DeviceService deviceService)
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

            UpdatePageInfo();
            LoadPagedCustomers(CurrentPage, ROWPERPAGE);
            UpdatePageInfo();

        }


        private void LoadPagedCustomers(int pageNumber , int rowPerPage)
        {
            try
            {
                DgCustomers.ItemsSource = _customerService.GetPagedCustomerSummaries(pageNumber , rowPerPage);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"خطأ في تحميل  قائمه العملاء: {ex.Message}");
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
            LoadPagedCustomers(CurrentPage, ROWPERPAGE);
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
            TotalCustomers = _customerService.GetCustomerCount();
            TxtCustomerCountNumber.Text = TotalCustomers.ToString();

            TotalPages = (int)Math.Ceiling((double)TotalCustomers / ROWPERPAGE);

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            if (TotalPages == 0)
                CurrentPage = 1;

            UpdateButtonPage();
        }

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;
            var selectedSummary = clickedButton?.DataContext as CustomerSummary;

            if (selectedSummary == null) return;

            var result = System.Windows.MessageBox.Show(
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
                        System.Windows.MessageBox.Show("تم حذف العميل بنجاح", "نجاح");
                        UpdatePageInfo();
                       LoadPagedCustomers(CurrentPage , ROWPERPAGE);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"فشل في حذف العميل: {deleted.Error}");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"حدث خطأ: {ex.Message}");
                }
            }
        }



        //----------------------------------------------------------------------
        private void DgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgCustomers.SelectedItem is CustomerSummary selectedSummary)
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
                    System.Windows.MessageBox.Show($"خطأ: {ex.Message}");
                }
            }
            else
            {
                ProfileColumn.Width = new GridLength(0);
                ProfileSection.Visibility = Visibility.Collapsed;
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
                    System.Windows.MessageBox.Show("فشل حفظ مسار الصورة في الإعدادات: " + ex.Message);
                }
            }
        }
        private void LoadCustomerProfile(CustomerProfileDTO customer)
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

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SearchBox.Text != "")
            {
                try
                {
                    var search = SearchBox.Text;

                    if (SearchBox.Text.ToLower().StartsWith("c-"))
                    {
                        search = SearchBox.Text.ToLower().Replace("c-", "");
                    }

                    var results = _customerService.SearchCustomerBy(search);
                    DgCustomers.ItemsSource = results;

                    DgCustomers.SelectedItem = null;

                    ProfileSection.Visibility = Visibility.Visible;
                    ProfileSection.Opacity = 0.3;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"خطأ في تحميل العملاء: {ex.Message}");
                }
            }

        }


        private void BtnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            //if (_currentCustomer == null)
            //    return;

            //var input = Microsoft.VisualBasic.Interaction.InputBox(
            //    "أدخل رقم الهاتف الجديد:",
            //    "إضافة رقم هاتف",
            //    "");

            //if (string.IsNullOrWhiteSpace(input))
            //    return;

            //try
            //{
            //    int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

            //    bool added = _phoneService.AddPhone(input, customerId);

            //    if (added)
            //    {
            //        System.Windows.MessageBox.Show("تمت إضافة الرقم بنجاح", "نجاح",
            //            MessageBoxButton.OK, MessageBoxImage.Information);

            //        RefreshCustomerProfile(customerId);
            //    }
            //    else
            //    {
            //        System.Windows.MessageBox.Show("فشل في إضافة الرقم", "خطأ",
            //            MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show($"حدث خطأ: {ex.Message}");
            //}
        }

        private void BtnEditPhone_Click(object sender, RoutedEventArgs e)
        {
        //    if (sender is System.Windows.Controls.Button btn && btn.Tag is string oldPhone)
        //    {
        //        var newPhone = Microsoft.VisualBasic.Interaction.InputBox(
        //            "عدل رقم الهاتف:",
        //            "تعديل رقم",
        //            oldPhone);

        //        if (string.IsNullOrWhiteSpace(newPhone) || newPhone == oldPhone)
        //            return;

        //        try
        //        {
        //            int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

        //            bool updated = _phoneService.UpdatePhone(newPhone, oldPhone);

        //            if (updated)
        //            {
        //                System.Windows.MessageBox.Show("تم تعديل الرقم بنجاح", "نجاح",
        //                    MessageBoxButton.OK, MessageBoxImage.Information);

        //                RefreshCustomerProfile(customerId);

        //                if (DgCustomers.SelectedItem is CustomerSummary selected)
        //                {

        //                    selected.Phone = newPhone;

        //                    DgCustomers.Items.Refresh();
        //                }

        //            }
        //            else
        //            {
        //                System.Windows.MessageBox.Show("فشل في تعديل الرقم", "خطأ",
        //                    MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Windows.MessageBox.Show($"حدث خطأ: {ex.Message}");
        //        }
        //    }
        }
        private void BtnDeletePhone_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Tag is string phoneNumber)
            {
                var result = System.Windows.MessageBox.Show(
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
                            System.Windows.MessageBox.Show("تم حذف الرقم بنجاح", "نجاح",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            if (_currentCustomer != null)
                            {
                                var refreshedProfile = _customerService.GetCustomerFullProfile(customerId);
                                LoadCustomerProfile(refreshedProfile!);
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("فشل في حذف الرقم", "خطأ",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}");
                    }
                }
            }
        }


        private void BtnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            //if (_currentCustomer == null)
            //{
            //    System.Windows.MessageBox.Show("الرجاء اختيار عميل قبل التعديل.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            //if (!int.TryParse(_currentCustomer.ID.Replace("C-", ""), out int customerId))
            //{
            //    System.Windows.MessageBox.Show("رقم العميل غير صالح.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //var customerUpdateDTO = new CustomerUpdateDTO
            //{
            //    Name = _currentCustomer.Name,
            //    Age = _currentCustomer.Age,
            //    Sex = _currentCustomer.Sex,
            //    Address = _currentCustomer.Address,
            //    Discount = _currentCustomer.Discount
            //};

            //try
            //{
            //    var editWindow = new EditCustomerView(_customerService, customerUpdateDTO, customerId)
            //    {
            //        Owner = this
            //    };

            //    bool? dialogResult = editWindow.ShowDialog();

            //    if (dialogResult == true)
            //    {
            //        RefreshCustomerProfile(customerId);

            //        if (DgCustomers.SelectedItem is CustomerSummary selected)
            //        {

            //            selected.Address = _currentCustomer.Address;
            //            selected.Name = _currentCustomer.Name;

            //            DgCustomers.Items.Refresh();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
        private void BtnCreateCustomer_Click(object sender, RoutedEventArgs e)
        {
            //var createWin = new CreateCustomerWindow(_customerService, _deviceBrandService, _deviceTypeService, _deviceSpecService);

            //createWin.Owner = Window.GetWindow(this);

            //if (createWin.ShowDialog() == true)
            //{

            //    LoadAllCustomers();
            //}
        }

        private void BtnEditDevice_Click(object sender, RoutedEventArgs e)
        {
            //if (_currentCustomer == null)
            //{
            //    System.Windows.MessageBox.Show("الرجاء اختيار عميل أولاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            //if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
            //{
            //    int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

            //    var updateWindow = new UpdateDeviceWindow(
            //        selectedDevice,
            //        _deviceService,
            //        _deviceBrandService,
            //        _deviceTypeService,
            //        _deviceSpecService
            //    )
            //    {
            //        Owner = this
            //    };

            //    bool? result = updateWindow.ShowDialog();

            //    if (result == true)
            //    {
            //        RefreshCustomerProfile(customerId);
            //    }
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("برجاء اختيار الجهاز الذي تريد تعديله");
            //}
        }

        private void BtnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            //int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

            //var AddWin = new AddDeviceWindow(customerId, _deviceService, _customerService, _deviceBrandService, _deviceTypeService, _deviceSpecService);

            //AddWin.Owner = Window.GetWindow(this);

            //if (AddWin.ShowDialog() == true)
            //{

            //    RefreshCustomerProfile(customerId);
            //}
        }

        private void BtnDeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            //if (_currentCustomer == null)
            //    return;

            //if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
            //{
            //    var result = System.Windows.MessageBox.Show(
            //        $"هل أنت متأكد من حذف الجهاز ؟",
            //        "تأكيد الحذف",
            //        MessageBoxButton.YesNo,
            //        MessageBoxImage.Warning);

            //    if (result == MessageBoxResult.Yes)
            //    {
            //        try
            //        {
            //            int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));
            //            bool deleted = _deviceService.DeleteCustomerDevice(selectedDevice.DeviceId);

            //            if (deleted)
            //            {
            //                System.Windows.MessageBox.Show("تم حذف الجهاز بنجاح", "نجاح",
            //                    MessageBoxButton.OK, MessageBoxImage.Information);

            //                RefreshCustomerProfile(customerId);
            //            }
            //            else
            //            {
            //                System.Windows.MessageBox.Show("فشل في حذف الجهاز", "خطأ",
            //                    MessageBoxButton.OK, MessageBoxImage.Error);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            System.Windows.MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}", "خطأ",
            //                MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("الرجاء اختيار جهاز للحذف", "تنبيه",
            //        MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }
        private void BtnDeviceHistory_Click(object sender, RoutedEventArgs e)
        {

        }
     
        private void SetButtonsEnabled(bool isEnabled)
        {
            BtnEditCustomer.IsEnabled = isEnabled;
            btnAddNum.IsEnabled = isEnabled;
            btnAddDevice.IsEnabled = isEnabled;
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

        //private void RefreshCustomerProfile(int customerId)
        //{
        //    DgCustomers.ItemsSource = null;
        //    DgCustomers.ItemsSource = _customerService.GetPagedCustomerSummaries();

        //    DgCustomers.SelectedItem = null;


        //    var customerInGrid = DgCustomers.Items.Cast<CustomerSummary>()
        //                   .FirstOrDefault(c => c.ID == $"C-{customerId}");
        //    if (customerInGrid != null)
        //        DgCustomers.SelectedItem = customerInGrid;
        //}

    }

}
