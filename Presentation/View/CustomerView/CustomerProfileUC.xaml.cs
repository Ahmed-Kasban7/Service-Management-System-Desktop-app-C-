using Application.DTOs.CustomerDTOs;
using Application.DTOs.DeviceDTOs;
using Application.DTOs.OrderDTOs;
using Application.Features.BrandManagement;
using Application.Features.BrandManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment;
using Application.Features.CustomerManagment.Commands;
using Application.Features.DeviceManagement;
using Application.Features.DeviceManagement.Commands;
using Application.Features.DeviceManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Application.Features.PhoneManagement;
using Application.Features.PhoneManagement.Commands;
using Application.Features.PhoneManagement.Queries;
using Application.Features.SpecManagement;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement;
using Application.Features.TypeManagement.Queries;
using Domain.Entities;
using Domain.Enums;
using Presentation.View.Customer_View;
using Presentation.View.OrderView;
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
        public event EventHandler BackRequested;
        private GetCustomerBasicInfoHandler _getCustomerBasicInfoHandler;
        private UpdateCustomerHandler _updateCustomerHandler;
        private readonly GetCustomerPhonesHandler _getCustomerPhonesHandler;
        private readonly AddPhoneToCustomer _addPhoneToCustomer;
        private readonly DeletePhoneHandler _deletePhoneHandler;
        private readonly UpdatePhoneHandler _updatePhoneHandler;
        private readonly GetCustomerDevicesHandler _getCustomerDevicesHandler;
        private readonly AddDeviceToCustomerHandler _addDeviceHandler;
        private readonly GetAllBrandsHandler _getBrandsHandler;
        private readonly GetAllTypesHandler _getTypesHandler;
        private readonly GetSpecsByTypeIdHandler _getSpecsHandler;
        private readonly UpdateDeviceHandler _updateDeviceHandler;
        private readonly DeleteDeviceHandler _deleteDeviceHandler;
        private readonly GetDeviceOrders _getDeviceOrders;
        private readonly GetCustomerOrdersHandler _getCustomerOrdersHandler;
        private readonly DeleteCustomerHandler _deleteCustomerHandler;


        public event EventHandler<int> OrderDetailsRequested;

        private int _customerId;

        public CustomerProfileUC(int customerId, GetCustomerBasicInfoHandler getCustomerBasicInfoHandler, UpdateCustomerHandler updateCustomerHandler,
            GetCustomerPhonesHandler getCustomerPhones, AddPhoneToCustomer phoneToCustomer, DeletePhoneHandler deletePhone,
            UpdatePhoneHandler updatePhone, GetCustomerDevicesHandler getCustomerDevices, AddDeviceToCustomerHandler addDeviceToCustomer,
            GetAllBrandsHandler getAllBrands, GetAllTypesHandler getAllTypes,
            GetSpecsByTypeIdHandler getSpecsByType, UpdateDeviceHandler updateDeviceHandler,
            DeleteDeviceHandler deleteDevice , GetDeviceOrders getDeviceOrders, 
            GetCustomerOrdersHandler getCustomerOrdersHandler , DeleteCustomerHandler deleteCustomer)
        {
            InitializeComponent();
            _customerId = customerId;
            _getCustomerBasicInfoHandler = getCustomerBasicInfoHandler;
            _updateCustomerHandler = updateCustomerHandler;
            _getCustomerPhonesHandler = getCustomerPhones;
            _addPhoneToCustomer = phoneToCustomer;
            _deletePhoneHandler = deletePhone;
            _updatePhoneHandler = updatePhone;
            _getCustomerDevicesHandler = getCustomerDevices;
            _addDeviceHandler = addDeviceToCustomer;
            _getBrandsHandler = getAllBrands;
            _getSpecsHandler = getSpecsByType;
            _getTypesHandler = getAllTypes;
            _updateDeviceHandler = updateDeviceHandler;
            _deleteDeviceHandler = deleteDevice;
            _getDeviceOrders = getDeviceOrders;
            _getCustomerOrdersHandler = getCustomerOrdersHandler;
            _deleteCustomerHandler = deleteCustomer;

            LoadCustomerBasicInfo();

            BtnEditCustomer.IsEnabled = true;
            BtnAddPhone.IsEnabled = true;
            BtnAddDevice.IsEnabled = true;
        }

        private void  LoadCustomerBasicInfo()
        {
            try
            {
                var customer = _getCustomerBasicInfoHandler.Handle(_customerId);

                if (customer == null) return;

                TxtProfileID.Text = customer.Code;
                TxtProfileName.Text = customer.Name;
                TxtProfileSex.Text = customer.Sex == ESex.FEMALE ? "أنثى" : "ذكر";
                TxtProfileAge.Text = customer.Age.ToString();
                TxtProfileAddress.Text = customer.Address;
                TxtProfileDiscount.Text = $"{customer.Discount}%";
                RunCustomerCode.Text = $"#{customer.Code}";
                TxtCreatedDate.Text = $"تاريخ التسجيل: {customer.CreatedDate:yyyy/MM/dd}";
            }
            catch (Exception ex) 
            {
                    MessageBox.Show("حدث مشكله اثناء تحميل بيانات العميل", "خطأ",
                            MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        private void LoadCustomerPhones()
        {
            try
            {
                List<string> Phones = _getCustomerPhonesHandler.Handle(_customerId).ToList();
                if (Phones?.Any() == true)
                {
                    PhonesList.ItemsSource = Phones;
                    TxtNoPhonesMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TxtNoPhonesMessage.Visibility = Visibility.Visible;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("حدث مشكله اثناء تحميل هواتف العميل", "خطأ",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void LoadCustomerDevices()
        {
            try
            {
                var Devices = _getCustomerDevicesHandler.Handle(_customerId).Value.ToList();
                if (Devices?.Any() == true)
                {
                    DevicesList.ItemsSource = Devices;
                    TxtNoDevicesMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TxtNoDevicesMessage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث مشكله اثناء تحميل اجهزه العميل", "خطأ",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadCustomerOrders()
        {
            try
            {
                var orders = _getCustomerOrdersHandler.Handle(_customerId).ToList();

                TxtTotalOrders.Text = orders.Count.ToString();
                TxtDoneOrders.Text = orders.Count(o => o.OrderState == 3).ToString();
                TxtActiveOrders.Text = orders.Count(o => o.OrderState == 2).ToString();

                if (orders.Any())
                {
                    OrdersList.ItemsSource = orders;
                    TxtNoOrdersMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    OrdersList.ItemsSource = null;
                    TxtNoOrdersMessage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل طلبات العميل", "خطأ",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersList.SelectedItem is CustomerOrderSummaryDto selectedOrder)
            {
                OrderDetailsRequested?.Invoke(this, selectedOrder.OrderId);
            }
        }
        private void BtnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = _getCustomerBasicInfoHandler.Handle(_customerId);
                var customerUpdateDTO = new CustomerUpdateDto(_customerId, customer.Name, customer.Age, customer.Sex, customer.Address, customer.Discount);

                var editWindow = new EditCustomerView(
                    _updateCustomerHandler,
                    customerUpdateDTO,
                    _customerId)
                {
                    Owner = Window.GetWindow(this)
                };

                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadCustomerBasicInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            var addPhoneWindow = new AddPhoneWindow(_addPhoneToCustomer, _customerId)
            {
                Owner = Window.GetWindow(this)
            };

            if (addPhoneWindow.ShowDialog() == true)
                LoadCustomerPhones();
        }

        private void BtnDeletePhone_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string phoneNumber)
            {
                var result = MessageBox.Show(
                    $"هل أنت متأكد من حذف الرقم {phoneNumber} ؟",
                    "تأكيد الحذف",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {

                        var deleted = _deletePhoneHandler.Handle(phoneNumber, _customerId);

                        if (deleted.IsSuccess)
                        {
                            MessageBox.Show("تم حذف الرقم بنجاح", "نجاح",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadCustomerPhones();

                        }
                        else
                        {
                            MessageBox.Show(deleted.Error, "خطأ",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء الحذف: {ex.Message}");
                    }
                }
            }
        }
        private void BtnEditPhone_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string oldPhone)
            {
                var updatePhoneWindow = new UpdatePhoneWindow(_updatePhoneHandler, oldPhone)
                {
                    Owner = Window.GetWindow(this)
                };

                if (updatePhoneWindow.ShowDialog() == true)
                    LoadCustomerPhones();
            }
        }


        private void BtnEditDevice_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
            {
                var updateWindow = new UpdateDeviceWindow(
                    selectedDevice,
                    _updateDeviceHandler,
                    _getBrandsHandler,
                    _getTypesHandler,
                    _getSpecsHandler)
                {
                    Owner = Window.GetWindow(this)
                };

                if (updateWindow.ShowDialog() == true)
                    LoadCustomerDevices();
            }
        }
        private void BtnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            var addDeviceWindow = new AddDeviceWindow(
                _customerId,
                _addDeviceHandler,
                _getBrandsHandler,
                _getTypesHandler,
                _getSpecsHandler)
            {
                Owner = Window.GetWindow(this)
            };

            if (addDeviceWindow.ShowDialog() == true)
                LoadCustomerDevices();
        }

        private void BtnDeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
            {
                var confirm = MessageBox.Show(
                    "هل أنت متأكد من حذف الجهاز ؟",
                    "تأكيد الحذف",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        var result  =  _deleteDeviceHandler.Handle(selectedDevice.DeviceId);

                        if (result.IsSuccess)
                        {
                            MessageBox.Show("تم حذف الجهاز بنجاح", "نجاح",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadCustomerDevices();
                        }
                        else
                        {
                            MessageBox.Show(result.Error, "خطأ",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "خطأ",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnDeviceHistory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DeviceInfoDTO selectedDevice)
            {
                TxtHistoryDeviceName.Text = $"{selectedDevice.TypeName} - {selectedDevice.BrandName}";
                TxtHistoryDeviceDetails.Text = selectedDevice.SpecName;
                TxtHistoryDeviceId.Text = selectedDevice.DeviceId.ToString();

                if (string.IsNullOrEmpty(selectedDevice.SerialNumber))
                {
                    HistorySerialPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    HistorySerialPanel.Visibility = Visibility.Visible;
                    TxtHistorySerial.Text = selectedDevice.SerialNumber;
                }

                if (string.IsNullOrEmpty(selectedDevice.Model))
                {
                    HistoryModelPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    HistoryModelPanel.Visibility = Visibility.Visible;
                    TxtHistoryModel.Text = selectedDevice.Model;
                }

                var orders = _getDeviceOrders.Handle(selectedDevice.DeviceId).ToList();
                if (orders.Any())
                {
                    DeviceOrdersList.ItemsSource = orders;
                    TxtNoDeviceOrdersMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DeviceOrdersList.ItemsSource = null;
                    TxtNoDeviceOrdersMessage.Visibility = Visibility.Visible;
                }

                SubPanelDevicesList.Visibility = Visibility.Collapsed;
                PanelDeviceHistory.Visibility = Visibility.Visible;
            }
        }
        private void DeviceOrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is DeviceOrderHistoryDto selectedOrder)
            {
                OrderDetailsRequested?.Invoke(this, selectedOrder.OrderId);
            }
        }

        private void BtnBackToDevices_Click(object sender, RoutedEventArgs e)
        {
            PanelDeviceHistory.Visibility = Visibility.Collapsed;
            SubPanelDevicesList.Visibility = Visibility.Visible;
        }
        private void SetActiveTab(Border panel, Button tab)
        {
           
            PanelBasicInfo.Visibility = Visibility.Collapsed;
            PanelPhones.Visibility = Visibility.Collapsed;
            PanelDevices.Visibility = Visibility.Collapsed;
            PanelOrders.Visibility = Visibility.Collapsed;


            foreach (var t in new[] { TabBasicInfo, TabPhones, TabDevices , TabOrders })
            {
                t.BorderBrush = Brushes.Transparent;
                t.Foreground = new SolidColorBrush(Color.FromRgb(0x64, 0x74, 0x8B));
                t.FontWeight = FontWeights.Normal;
            }

            panel.Visibility = Visibility.Visible;
            tab.BorderBrush = new SolidColorBrush(Color.FromRgb(0x25, 0x63, 0xEB));
            tab.Foreground = new SolidColorBrush(Color.FromRgb(0x25, 0x63, 0xEB));
            tab.FontWeight = FontWeights.SemiBold;
        }

        private void TabBasicInfo_Click(object sender, RoutedEventArgs e) => SetActiveTab(PanelBasicInfo, TabBasicInfo);
        private void TabPhones_Click(object sender, RoutedEventArgs e) {

            SetActiveTab(PanelPhones, TabPhones);
            LoadCustomerPhones();
        }
        private void TabDevices_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(PanelDevices, TabDevices);

            PanelDeviceHistory.Visibility = Visibility.Collapsed;
            SubPanelDevicesList.Visibility = Visibility.Visible;

            LoadCustomerDevices();
        }
        private void TabOrders_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(PanelOrders, TabOrders);
            LoadCustomerOrders();
        }
        public void BtnBack_Click(object sender, RoutedEventArgs e) => BackRequested?.Invoke(this, EventArgs.Empty);

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var confirm = MessageBox.Show(
                $"هل أنت متأكد من حذف العميل ؟",
                "تأكيد الحذف",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                var result = _deleteCustomerHandler.Handle(_customerId);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم حذف العميل بنجاح", "نجاح",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    BackRequested?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show(result.Error, "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
