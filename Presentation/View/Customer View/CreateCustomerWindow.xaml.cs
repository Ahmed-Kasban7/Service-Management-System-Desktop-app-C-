using Application.DTOs;
using Application.Services;
using Domain.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Presentation.View.Customer_View
{
    public partial class CreateCustomerWindow : Window
    {
        public ObservableCollection<string> PhonesList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<DeviceCreateDTO> DevicesList { get; set; } = new ObservableCollection<DeviceCreateDTO>();
        private readonly CustomerService _customerService;
        private readonly DeviceBrandService _deviceBrandService;
        private readonly DeviceTypeService _deviceTypeService;
        private readonly DeviceSpecService _deviceSpecService;

        public CreateCustomerWindow(CustomerService customerService, DeviceBrandService deviceBrandService , DeviceTypeService deviceTypeService , DeviceSpecService deviceSpecService )
        {
            InitializeComponent();

            _customerService = customerService;
            _deviceBrandService = deviceBrandService;
            _deviceTypeService = deviceTypeService;
            _deviceSpecService = deviceSpecService;
            
            LstPhones.ItemsSource = PhonesList;
            DgTempDevices.ItemsSource = DevicesList;

            CbBrand.ItemsSource = _deviceBrandService.GetAllBrands(); 
            CbType.ItemsSource = _deviceTypeService.GetAllTypes();
        }
        private void BtnAddPhoneToList_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtNewPhone.Text))
            {
                PhonesList.Add(TxtNewPhone.Text.Trim());
                TxtNewPhone.Clear();
            }
        }
        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbType.SelectedValue != null)
            {
                int selectedTypeId = (int)CbType.SelectedValue;


                var specs = _deviceSpecService.GetSpecsByTypeId(selectedTypeId);

                CbSpec.ItemsSource = specs;

                CbSpec.SelectedIndex = -1;
            }
            else
            {
                CbSpec.ItemsSource = null;
            }
        }

        private void BtnRemovePhone_Click(object sender, RoutedEventArgs e)
        {
            var phone = (sender as System.Windows.Controls.Button)?.DataContext as string;
            if (phone != null) PhonesList.Remove(phone);
        }

        private void BtnAddDeviceToList_Click(object sender, RoutedEventArgs e)
        {
            if (CbType.SelectedItem is not TypeDTO selectedType ||
                CbBrand.SelectedItem is not BrandDTO selectedBrand ||
                CbSpec.SelectedItem is not SpecDTO selectedSpec)
            {
                return;
            }

            DevicesList.Add(new DeviceCreateDTO
            {
                TypeID = selectedType.TypeID,
                BrandID = selectedBrand.BrandID,
                SpecID = selectedSpec.SpecID,

                TypeName = selectedType.TypeName,
                BrandName = selectedBrand.BrandName,
                SpecName = selectedSpec.SpecName,

                Model = TxtDeviceModel.Text,
                SerialNumber = TxtSerial.Text
            });

            TxtDeviceModel.Clear();
            TxtSerial.Clear();

            CbType.SelectedIndex = -1;
            CbBrand.SelectedIndex = -1;
            CbSpec.SelectedIndex = -1;
        }

        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text) &&
                 string.IsNullOrWhiteSpace(TxtAddress.Text) &&
                 string.IsNullOrWhiteSpace(TxtDiscount.Text) &&
                 string.IsNullOrWhiteSpace(TxtAge.Text) &&
                 PhonesList.Count == 0 &&
                 DevicesList.Count == 0)
            {
                System.Windows.MessageBox.Show("برجاء إدخال بيانات العميل ");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                System.Windows.MessageBox.Show("برجاء إدخال اسم العميل");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                System.Windows.MessageBox.Show("برجاء إدخال العنوان");
                return;
            }

            if(PhonesList is null ||PhonesList.Count==0)
            {
                System.Windows.MessageBox.Show("يجب إدخال رقم هاتف واحد على الأقل للعميل");
                return;
            }

            if(DevicesList is null || DevicesList.Count==0)
            {
                System.Windows.MessageBox.Show("يجب إدخال جهاز واحد على الأقل للعميل");
                return;
            }

            try
            {
                var customerDto = new CustomerCreateDTO(
                     TxtName.Text.Trim(),
                     TxtAddress.Text?.Trim() ?? "",
                     int.TryParse(TxtDiscount.Text, out int d) ? d : 0,
                     int.TryParse(TxtAge.Text, out int a) ? a : (int?)null,
                     CbSex.SelectedIndex == 0 ? ESex.MALE : ESex.FEMALE ,
                     DevicesList.ToList(),
                     PhonesList.ToList());

                _customerService.CreateCustomer(customerDto);

                System.Windows.MessageBox.Show("تم حفظ بيانات العميل بنجاح");

                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"حدث خطأ أثناء الحفظ: {ex.Message}");
            }
        }
        private void BtnDeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            var deviceDto = button?.DataContext as DeviceCreateDTO;

            if (deviceDto != null)
            {
                DevicesList.Remove(deviceDto);
            }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}