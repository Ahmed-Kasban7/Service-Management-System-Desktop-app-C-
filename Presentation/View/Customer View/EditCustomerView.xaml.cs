using Application.DTOs;
using Application.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.Customer_View
{
    public partial class EditCustomerView : Window
    {
        private readonly CustomerService _customerService;
        private readonly CustomerUpdateDTO _customer;
        private readonly int _customerId;

        public EditCustomerView(CustomerService customerService,
                                CustomerUpdateDTO customer ,int customerId)
        {
            InitializeComponent();

            _customerService = customerService;
            _customer = customer;
            _customerId = customerId;

            LoadData();
        }

        private void LoadData()
        {
            TxtName.Text = _customer.Name;
            TxtAge.Text = _customer.Age.ToString();
            TxtSex.SelectedIndex = (_customer.Sex?.ToLower() == "أنثى") ? 1 : 0;
            TxtAddress.Text = _customer.Address;
            TxtDiscount.Text = _customer.Discount.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                System.Windows.MessageBox.Show("الرجاء إدخال الاسم", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtAge.Text, out int age) || age <= 0)
            {
                System.Windows.MessageBox.Show("الرجاء إدخال عمر صالح أكبر من صفر", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TxtSex.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("الرجاء اختيار الجنس", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string sex = ((ComboBoxItem)TxtSex.SelectedItem).Content.ToString();

            string address = TxtAddress.Text.Trim();

            if (string.IsNullOrEmpty(address))
            {
                System.Windows.MessageBox.Show("الرجاء إدخال العنوان", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int discount;

            if (string.IsNullOrWhiteSpace(TxtDiscount.Text))
            {
                discount = 0;
            }
            else if (!int.TryParse(TxtDiscount.Text, out discount) || discount < 0 || discount > 100)
            {
                System.Windows.MessageBox.Show(
                    "الرجاء إدخال نسبة خصم صحيحة بين 0 و 100",
                    "خطأ",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning
                );
                return;
            }
            var updatedCustomer = new CustomerUpdateDTO
            {
                Name = name,
                Age = age,
                Sex = sex,
                Address = address,
                Discount = discount
            };

            try
            {
                bool updated = _customerService.UpdateCustomerInfo(_customerId, updatedCustomer);
                if (updated)
                {
                    System.Windows.MessageBox.Show("تم تعديل بيانات العميل بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    System.Windows.MessageBox.Show("فشل في تعديل البيانات", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}