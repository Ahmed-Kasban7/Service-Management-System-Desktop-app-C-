using Application.DTOs.CustomerDTOs;
using Application.Services;
using Domain.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.Customer_View
{
    public partial class EditCustomerView : Window
    {
        private readonly CustomerService _customerService;
        private readonly CustomerUpdate _customer;
        private readonly int _customerId;

        public EditCustomerView(CustomerService customerService,
                                CustomerUpdate customer ,int customerId)
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
            TxtSex.SelectedIndex = (_customer.Sex == ESex.MALE) ? 0 : 1;
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

            int? age = null;

            if (!string.IsNullOrWhiteSpace(TxtAge.Text))
            {
                if (!int.TryParse(TxtAge.Text, out int parsedAge) || parsedAge <= 0)
                {
                    System.Windows.MessageBox.Show("الرجاء إدخال عمر صالح أكبر من صفر", "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                age = parsedAge;
            }

            if (TxtSex.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("الرجاء اختيار الجنس", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ESex sex = TxtSex.SelectedIndex ==0 ?ESex.MALE : ESex.FEMALE;

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
            var updatedCustomer = new CustomerUpdate(_customerId, name, age, sex, address, discount);

            try
            {
                bool updated = _customerService.UpdateCustomerInfo(updatedCustomer);
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