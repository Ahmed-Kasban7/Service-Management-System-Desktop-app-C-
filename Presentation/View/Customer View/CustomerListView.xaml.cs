using Application.Common.Interfaces;
using Application.DTOs;
using Application.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation.View.Customer_View
{
    public partial class CustomerListView : Window
    {
        private readonly CustomerService _customerService;
        private readonly PhoneService _phoneService;
        private CustomerProfileDTO? _currentCustomer;

        public CustomerListView(CustomerService customerRepository, PhoneService phoneService)
        {
            InitializeComponent();

            _customerService = customerRepository;
            _phoneService = phoneService;

            LoadAllCustomers();
        }


        private void LoadAllCustomers()
        {
            try
            {
                DgCustomers.ItemsSource = _customerService.GetAllCustomers();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"خطأ في تحميل العملاء: {ex.Message}");
            }
        }

        private void DgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgCustomers.SelectedItem is CustomerSummaryDTO selectedSummary)
            {
                try
                {
                    int customerId = int.Parse(selectedSummary.ID.Replace("C-", ""));

                    var fullProfile = _customerService.GetCustomerFullProfile(customerId);

                    if (fullProfile != null)
                    {
                        ProfileSection.Opacity = 1.0;
                        BtnEditCustomer.IsEnabled = true;
                        BtnDeleteCustomer.IsEnabled = true;
                        btnAddNum.IsEnabled = true;
                        btnAddDevice.IsEnabled = true;
                        LoadCustomerProfile(fullProfile);
                    }
                    else
                    {
                        btnAddDevice.IsEnabled = false;
                        btnAddNum.IsEnabled = false;
                        ProfileSection.Opacity = 0.3;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"خطأ في جلب بيانات العميل: {ex.Message}");
                }
            }
            else
            {
                ProfileSection.Visibility = Visibility.Collapsed;
            }
        }


        private void LoadCustomerProfile(CustomerProfileDTO customer)
        {
            _currentCustomer = customer;

            TxtProfileID.Text = customer.ID;
            TxtProfileName.Text = customer.Name;
            TxtProfileAge.Text = customer.Age.ToString();
            TxtProfileSex.Text = customer.Sex;
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
                    ClearProfile();

                    var results = _customerService.SearchCustomerBy(SearchBox.Text);
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {

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
                    System.Windows.MessageBox.Show("تمت إضافة الرقم بنجاح", "نجاح",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    RefreshCustomerProfile(customerId);
                }
                else
                {
                    System.Windows.MessageBox.Show("فشل في إضافة الرقم", "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"حدث خطأ: {ex.Message}");
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

                    bool updated = _phoneService.UpdatePhone(newPhone , customerId);

                    if (updated)
                    {
                        System.Windows.MessageBox.Show("تم تعديل الرقم بنجاح", "نجاح",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        if (_currentCustomer != null)
                        {
                            RefreshCustomerProfile(customerId);
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("فشل في تعديل الرقم", "خطأ",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                    }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"حدث خطأ: {ex.Message}");
                }
            }
        }

        private void BtnAddDevice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteDevice_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnDeviceHistory_Click(object sender, RoutedEventArgs e)
        {

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

                        bool deleted = _phoneService.DeletePhone(phoneNumber , customerId);

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

        private void BtnEditCustomer_Click(object sender, RoutedEventArgs e) { }
        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (DgCustomers.SelectedItem is CustomerSummaryDTO selectedSummary)
            {
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

                        bool deleted = _customerService.DeleteCustomer(customerId);

                        if (deleted)
                        {
                            System.Windows.MessageBox.Show("تم حذف العميل بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadAllCustomers();

                            DgCustomers.SelectedItem = null;

                            ProfileSection.Opacity = 0.3;
                            ProfileSection.Visibility = Visibility.Visible;
                            BtnEditCustomer.IsEnabled = false;
                            BtnDeleteCustomer.IsEnabled = false;
                            btnAddNum.IsEnabled = false;
                            btnAddDevice.IsEnabled = false;

                            _currentCustomer = null;
                            TxtProfileID.Text = "---";
                            TxtProfileName.Text = "---";
                            TxtProfileAge.Text = "---";
                            TxtProfileSex.Text = "---";
                            TxtProfileAddress.Text = "---";
                            TxtProfileDiscount.Text = "---";
                            PhonesList.ItemsSource = null;
                            DevicesList.ItemsSource = null;
                            TxtNoPhonesMessage.Visibility = Visibility.Visible;
                            TxtNoDevicesMessage.Visibility = Visibility.Visible;
                        }

                        else
                        {
                            System.Windows.MessageBox.Show("فشل في حذف العميل", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("الرجاء اختيار عميل للحذف", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnNewCustomer_Click(object sender, RoutedEventArgs e) { }
        private void BtnEditDevice_Click(object sender, RoutedEventArgs e) { }

        private void BtnCreateCustomer_Click(object sender, RoutedEventArgs e)
        { }

        private void ClearProfile()
        {
            _currentCustomer = null;
            TxtProfileID.Text = "---";
            TxtProfileName.Text = "---";
            TxtProfileAge.Text = "---";
            TxtProfileSex.Text = "---";
            TxtProfileAddress.Text = "---";
            TxtProfileDiscount.Text = "---";
            PhonesList.ItemsSource = null;
            DevicesList.ItemsSource = null;
            TxtNoPhonesMessage.Visibility = Visibility.Visible;
            TxtNoDevicesMessage.Visibility = Visibility.Visible;

            BtnEditCustomer.IsEnabled = false;
            BtnDeleteCustomer.IsEnabled = false;
            btnAddNum.IsEnabled = false;
            btnAddDevice.IsEnabled = false;
        }

    

    private void RefreshCustomerProfile(int customerId)
        {
            var refreshedProfile = _customerService.GetCustomerFullProfile(customerId);
            if (refreshedProfile != null)
            {
                LoadCustomerProfile(refreshedProfile);
            }
        }

    }
}
