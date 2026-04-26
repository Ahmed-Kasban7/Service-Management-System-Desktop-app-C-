using Application.DTOs;
using Application.DTOs.CustomerDTOs;
using Application.Features.CustomerManagment;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private CustomerService _customerService;
        public CustomersUC()
        {
            InitializeComponent();
        }
        public void Initialize(CustomerService customerService)
        {
            _customerService = customerService;

            CurrentPage = 1;
            _searchWord = null;

            //UpdatePageInfo();
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

        //    private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        //    {
        //        CurrentPage--;
        //        ChangePage();
        //    }

        //    private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        //    {
        //        CurrentPage++;
        //        ChangePage();
        //    }

        //    private void ChangePage()
        //    {
        //        LoadCustomers();
        //        TxtPageInfo.Text = CurrentPage.ToString();

        //        UpdateButtonPage();
        //    }


        //    private void UpdateButtonPage()
        //    {

        //        BtnPrevPage.IsEnabled = CurrentPage > 1;

        //        BtnNextPage.IsEnabled = (CurrentPage < TotalPages);
        //    }

        //    private void UpdatePageInfo()
        //    {
        //        //if (_searchWord == null)
        //        //    TotalCustomers = _customerService.GetCustomerCount();
        //        //else
        //        //    TotalCustomers = _customerService.GetSearchCustomerCount(_searchWord);

        //        //TxtCustomerCountNumber.Text = TotalCustomers.ToString();

        //        //TotalPages = (int)Math.Ceiling((double)TotalCustomers / ROWPERPAGE);

        //        //if (CurrentPage > TotalPages)
        //        //    CurrentPage = TotalPages;

        //        //if (TotalPages == 0)
        //        //    CurrentPage = 1;

        //        //TxtPageInfo.Text = CurrentPage.ToString();

        //        //LoadCustomers();

        //        //UpdateButtonPage();
        //    }

        //    private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //    {

        //        if (e.Key == Key.Enter)
        //        {
        //            try
        //            {
        //                if (string.IsNullOrWhiteSpace(SearchBox.Text))
        //                {
        //                    _searchWord = null;
        //                    CurrentPage = 1;
        //                    UpdatePageInfo();
        //                    return;
        //                }

        //                var search = SearchBox.Text;

        //                if (SearchBox.Text.ToLower().StartsWith("c-"))
        //                {
        //                    search = SearchBox.Text.ToLower().Replace("c-", "");
        //                }

        //                _searchWord = search;
        //                CurrentPage = 1;

        //                UpdatePageInfo();
        //                DgCustomers.SelectedItem = null;
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"خطأ في تحميل العملاء: {ex.Message}");
        //            }
        //        }

        //    }
        //    private void DgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        //if (DgCustomers.SelectedItem is CustomerSummaryDto selectedSummary)
        //        //{
        //        //    ProfileSection.Visibility = Visibility.Visible;
        //        //    ProfileColumn.Width = new GridLength(450);
        //        //    ProfileSection.Opacity = 1.0;
        //        //    try
        //        //    {
        //        //        int customerId = int.Parse(selectedSummary.ID.Replace("C-", ""));
        //        //        var fullProfile = _customerService.GetCustomerFullProfile(customerId);

        //        //        if (fullProfile != null)
        //        //        {
        //        //            SetButtonsEnabled(true);
        //        //            LoadCustomerProfile(fullProfile);
        //        //        }
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        MessageBox.Show($"خطأ: {ex.Message}");
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    ProfileColumn.Width = new GridLength(0);
        //        //    ProfileSection.Visibility = Visibility.Collapsed;
        //        //}
        //    }
        //    private void ReloadCustomerProfile(int customerId)
        //    {

        //        //_currentCustomer = _customerService.GetCustomerFullProfile(customerId);
        //        //LoadCustomerProfile(_currentCustomer);

        //        //LoadCustomers();

        //        //var customers = DgCustomers.ItemsSource as IEnumerable<CustomerSummaryDto>;
        //        //var updatedCustomer = customers.FirstOrDefault(c => c.ID == $"C-{customerId}");

        //        //if (updatedCustomer != null)
        //        //{
        //        //    DgCustomers.SelectedItem = updatedCustomer;
        //        //    DgCustomers.ScrollIntoView(updatedCustomer);
        //        //}
        //    }
        //    private void LoadCustomerProfile(CustomerProfileDto customer)
        //    {
        //        //_currentCustomer = customer;

        //        //TxtProfileID.Text = customer.ID;
        //        //TxtProfileName.Text = customer.Name;
        //        //TxtProfileAge.Text = customer.Age?.ToString() ?? "---";
        //        //TxtProfileSex.Text = customer.Sex == ESex.MALE ? "ذكر" : "أنثى";
        //        //TxtProfileAddress.Text = customer.Address;
        //        //TxtProfileDiscount.Text = $"{customer.Discount}%";

        //        //PhonesList.ItemsSource = customer.Phones;
        //        //TxtNoPhonesMessage.Visibility = (customer.Phones == null || customer.Phones.Count == 0)
        //        //    ? Visibility.Visible : Visibility.Collapsed;

        //        //DevicesList.ItemsSource = customer.Devices;
        //        //TxtNoDevicesMessage.Visibility = (customer.Devices == null || customer.Devices.Count == 0)
        //        //    ? Visibility.Visible : Visibility.Collapsed;
        //    }
        //}
    }
}
