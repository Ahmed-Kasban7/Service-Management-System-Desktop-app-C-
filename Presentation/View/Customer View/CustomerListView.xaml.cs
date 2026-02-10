using Application.Common.Interfaces;
using Application.DTOs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.Customer_View
{
    public partial class CustomerListView : Window
    {
        private readonly ICustomerRepository _customerRepository;
        private CustomerProfileDTO? _currentCustomer;

        public CustomerListView(ICustomerRepository customerRepository)
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
                        ProfileSection.Opacity = 1;
                        btnAddDevice.IsEnabled = true;
                        btnAddNum.IsEnabled = true;
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
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e) { }
        private void BtnNewCustomer_Click(object sender, RoutedEventArgs e) { }
        private void BtnEditDevice_Click(object sender, RoutedEventArgs e) { }
    }
}
