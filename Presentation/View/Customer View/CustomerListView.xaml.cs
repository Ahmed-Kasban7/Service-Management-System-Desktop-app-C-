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
            //if (DgCustomers.SelectedItem is CustomerSummary selected)
            //{
            //    ProfileArea.DataContext = selected;
            //}
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
