using Application.DTOs.CustomerDTOs;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment;
using Application.Features.CustomerManagment.Commands;
using Domain.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.Customer_View
{
    public partial class EditCustomerView : Window
    {
        private readonly UpdateCustomerHandler _updateCustomerHandler;
        private readonly CustomerUpdateDto _customer;
        private readonly int _customerId;

        public EditCustomerView(UpdateCustomerHandler updateCustomer,
                                CustomerUpdateDto customer, int customerId)
        {
            InitializeComponent();
            _updateCustomerHandler = updateCustomer;
            _customerId = customerId;
            _customer = customer;
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
            bool isValid = true;

            TxtNameError.Visibility = Visibility.Collapsed;
            TxtAgeError.Visibility = Visibility.Collapsed;
            TxtAddressError.Visibility = Visibility.Collapsed;
            TxtDiscountError.Visibility = Visibility.Collapsed;

            string name = TxtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                TxtNameError.Text = "الرجاء إدخال الاسم";
                TxtNameError.Visibility = Visibility.Visible;
                isValid = false;
            }

            int? age = null;
            if (!string.IsNullOrWhiteSpace(TxtAge.Text))
            {
                if (int.TryParse(TxtAge.Text, out int parsedAge) && parsedAge > 0)
                {
                    age = parsedAge;
                }
                else
                {
                    TxtAgeError.Text = "الرجاء إدخال عمر صحيح أكبر من صفر";
                    TxtAgeError.Visibility = Visibility.Visible;
                    isValid = false;
                }
            }

            if (TxtSex.SelectedIndex < 0)
            {
                MessageBox.Show("الرجاء اختيار الجنس (ذكر / أنثى) أولاً", "خطأ",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ESex sex = TxtSex.SelectedIndex == 0 ? ESex.MALE : ESex.FEMALE;

            string address = TxtAddress.Text.Trim();
            if (string.IsNullOrWhiteSpace(address))
            {
                TxtAddressError.Text = "الرجاء إدخال العنوان";
                TxtAddressError.Visibility = Visibility.Visible;
                isValid = false;
            }

            int discount = 0;
            if (!string.IsNullOrWhiteSpace(TxtDiscount.Text))
            {
                if (!int.TryParse(TxtDiscount.Text, out discount) || discount < 0 || discount > 100)
                {
                    TxtDiscountError.Text = "نسبة خصم يجب ان تكون بين 0 الى 100";
                    TxtDiscountError.Visibility = Visibility.Visible;
                    isValid = false;
                }
            }

            if (!isValid)
                return;

            var updatedCustomer = new CustomerUpdateDto(
                _customerId,
                name,
                age,
                sex,
                address,
                discount
            );

            try
            {
                var result = _updateCustomerHandler.Handle(updatedCustomer);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تعديل بيانات العميل بنجاح",
                        "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show(result.Error,
                        "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}