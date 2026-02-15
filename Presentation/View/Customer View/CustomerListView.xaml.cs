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
        private readonly CustomerService _customerRepository;
        private CustomerProfileDTO? _currentCustomer;

        public CustomerListView(CustomerService customerRepository)
        {
            InitializeComponent();

            _customerRepository = customerRepository;

            LoadAllCustomers();
        }

        private void LoadAllCustomers()
        {
            try
            {
                DgCustomers.ItemsSource = _customerRepository.GetAllCustomers();
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

                    var fullProfile = _customerRepository.GetCustomerFullProfile(customerId);

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

                    var results = _customerRepository.SearchCustomerBy(SearchBox.Text);
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
        }
        private void BtnEditPhone_Click(object sender, RoutedEventArgs e)
        {
            
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
        { }

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

                        bool deleted = _customerRepository.DeleteCustomer(customerId);

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

    }
}
