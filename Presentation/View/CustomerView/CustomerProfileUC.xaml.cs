using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement;
using Application.Features.CustomerManagment;
using Application.Features.DeviceManagement;
using Application.Features.PhoneManagement;
using Application.Features.SpecManagement;
using Application.Features.TypeManagement;
using Presentation.View.Customer_View;
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
    /// Interaction logic for CustomerProfileUC.xaml
    /// </summary>
    public partial class CustomerProfileUC : UserControl
    {
        public CustomerProfileUC()
        {
            InitializeComponent();
        }

        private void BtnEditDevice_Click(object sender, RoutedEventArgs e)
        {
            //if (_currentCustomer == null)
            //{
            //    MessageBox.Show("الرجاء اختيار عميل أولاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            //        ReloadCustomerProfile(customerId);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("برجاء اختيار الجهاز الذي تريد تعديله");
            //}
        }

        private void BtnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            //int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

            //var AddWin = new AddDeviceWindow(customerId, _deviceService, _customerService, _deviceBrandService, _deviceTypeService, _deviceSpecService);

            //AddWin.Owner = GetWindow(this);

            //if (AddWin.ShowDialog() == true)
            //{

            //    ReloadCustomerProfile(customerId);
            //}
        }

        private void BtnDeleteDevice_Click(object sender, RoutedEventArgs e)
        {
        //    if (_currentCustomer == null)
        //        return;

        //    if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
        //    {
        //        var result = MessageBox.Show(
        //            $"هل أنت متأكد من حذف الجهاز ؟",
        //            "تأكيد الحذف",
        //            MessageBoxButton.YesNo,
        //            MessageBoxImage.Warning);

        //        if (result == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));
        //                bool deleted = _deviceService.DeleteCustomerDevice(selectedDevice.DeviceId);

        //                if (deleted)
        //                {
        //                    MessageBox.Show("تم حذف الجهاز بنجاح", "نجاح",
        //                        MessageBoxButton.OK, MessageBoxImage.Information);

        //                    ReloadCustomerProfile(customerId);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("فشل في حذف الجهاز", "خطأ",
        //                        MessageBoxButton.OK, MessageBoxImage.Error);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}", "خطأ",
        //                    MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("الرجاء اختيار جهاز للحذف", "تنبيه",
        //            MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
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
            //        MessageBox.Show("تمت إضافة الرقم بنجاح", "نجاح",
            //            MessageBoxButton.OK, MessageBoxImage.Information);

            //        ReloadCustomerProfile(customerId);
            //    }
            //    else
            //    {
            //        MessageBox.Show("فشل في إضافة الرقم", "خطأ",
            //            MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"حدث خطأ: {ex.Message}");
            //}
        }

        private void BtnEditPhone_Click(object sender, RoutedEventArgs e)
        {
            //if (sender is Button btn && btn.Tag is string oldPhone)
            //{
            //    var newPhone = Microsoft.VisualBasic.Interaction.InputBox(
            //        "عدل رقم الهاتف:",
            //        "تعديل رقم",
            //        oldPhone);

            //    if (string.IsNullOrWhiteSpace(newPhone) || newPhone == oldPhone)
            //        return;

            //    try
            //    {
            //        int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

            //        bool updated = _phoneService.UpdatePhone(newPhone, oldPhone);

            //        if (updated)
            //        {
            //            MessageBox.Show("تم تعديل الرقم بنجاح", "نجاح",
            //                MessageBoxButton.OK, MessageBoxImage.Information);

            //            ReloadCustomerProfile(customerId);

            //        }
            //        else
            //        {
            //            MessageBox.Show("فشل في تعديل الرقم", "خطأ",
            //                MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"حدث خطأ: {ex.Message}");
            //    }
            //}
        }
        private void BtnDeletePhone_Click(object sender, RoutedEventArgs e)
        {
            //if (sender is Button btn && btn.Tag is string phoneNumber)
            //{
            //    var result = MessageBox.Show(
            //        $"هل أنت متأكد من حذف الرقم {phoneNumber} ؟",
            //        "تأكيد الحذف",
            //        MessageBoxButton.YesNo,
            //        MessageBoxImage.Warning);

            //    if (result == MessageBoxResult.Yes)
            //    {
            //        try
            //        {
            //            int customerId = int.Parse(_currentCustomer.ID.Replace("C-", ""));

            //            bool deleted = _phoneService.DeletePhone(phoneNumber, customerId);

            //            if (deleted)
            //            {
            //                MessageBox.Show("تم حذف الرقم بنجاح", "نجاح",
            //                    MessageBoxButton.OK, MessageBoxImage.Information);

            //                ReloadCustomerProfile(customerId);

            //            }
            //            else
            //            {
            //                MessageBox.Show("فشل في حذف الرقم", "خطأ",
            //                    MessageBoxButton.OK, MessageBoxImage.Error);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}");
            //        }
            //    }
            //}
        }
        private void BtnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            //if (_currentCustomer == null)
            //{
            //    MessageBox.Show("الرجاء اختيار عميل قبل التعديل.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            //int customerId = GetCurrentCustomerId();


            //var customerUpdateDTO = new CustomerUpdateDto(customerId, _currentCustomer.Name, _currentCustomer.Age,
            //    _currentCustomer.Sex, _currentCustomer.Address, _currentCustomer.Discount);

            //try
            //{
            //    var editWindow = new EditCustomerView(_customerService, customerUpdateDTO, customerId)
            //    {
            //        Owner = this
            //    };


            //    bool? dialogResult = editWindow.ShowDialog();


            //    if (dialogResult == true)
            //    {
            //        ReloadCustomerProfile(customerUpdateDTO.Id);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void BtnDeviceHistory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
