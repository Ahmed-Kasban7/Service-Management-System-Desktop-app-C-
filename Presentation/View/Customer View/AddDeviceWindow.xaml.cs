using Application.DTOs;
using Application.DTOs.DeviceDTOs;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Controls;


namespace Presentation.View.Customer_View
{
    /// <summary>
    /// Interaction logic for AddDeviceWindow.xaml
    /// </summary>
    public partial class AddDeviceWindow : Window
    {
        private readonly int _customerId;
        private readonly DeviceAddDTO deviceAddDTO;
        private readonly CustomerService _customerService;
        private readonly DeviceBrandService _deviceBrandService;
        private readonly DeviceTypeService _deviceTypeService;
        private readonly DeviceSpecService _deviceSpecService;
        private readonly DeviceService _deviceService;

        public AddDeviceWindow(int customerId,DeviceService deviceService,CustomerService customerService , DeviceBrandService deviceBrandService, DeviceTypeService deviceTypeService , DeviceSpecService deviceSpecService)
        {
            InitializeComponent();
            _customerId = customerId; 
            _deviceService = deviceService;
            _customerService = customerService;

            _deviceBrandService = deviceBrandService;
            _deviceTypeService = deviceTypeService;
            _deviceSpecService = deviceSpecService;

            CbBrand.ItemsSource = _deviceBrandService.GetAllBrands();
            CbType.ItemsSource = _deviceTypeService.GetAllTypes();

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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (CbType.SelectedItem is not TypeDto selectedType ||
                CbBrand.SelectedItem is not BrandDto selectedBrand ||
                CbSpec.SelectedItem is not SpecDto selectedSpec)
            {
               System.Windows.MessageBox.Show("برجاء ادخال بيانات الجهاز");
                return;
            }

            try
            {
                var deviceDto = new DeviceAddDTO
                {
                    TypeID = selectedType.TypeID,
                    BrandID = selectedBrand.BrandID,
                    SpecID = selectedSpec.SpecID,
                    Model = TxtDeviceModel.Text.Trim(),
                    SerialNumber = TxtSerial.Text?.Trim(),

                };

                _deviceService.AddDeviceToCustomer(_customerId, deviceDto);

                System.Windows.MessageBox.Show("تم إضافة الجهاز بنجاح");

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"حدث خطأ أثناء الحفظ: {ex.Message}");
            }
        }
    }
}
