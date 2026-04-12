using Application.DTOs;
using Application.DTOs.DeviceDTOs;
using Application.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.Customer_View
{
    public partial class UpdateDeviceWindow : Window
    {
        private readonly DeviceInfoDTO _editingDevice;
        private readonly DeviceService _deviceService;
        private readonly DeviceBrandService _deviceBrandService;
        private readonly DeviceTypeService _deviceTypeService;
        private readonly DeviceSpecService _deviceSpecService;

        public UpdateDeviceWindow(DeviceInfoDTO deviceToEdit,
                                  DeviceService deviceService,
                                  DeviceBrandService deviceBrandService,
                                  DeviceTypeService deviceTypeService,
                                  DeviceSpecService deviceSpecService)
        {
            InitializeComponent();

            _editingDevice = deviceToEdit;
            _deviceService = deviceService;
            _deviceBrandService = deviceBrandService;
            _deviceTypeService = deviceTypeService;
            _deviceSpecService = deviceSpecService;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var brands = _deviceBrandService.GetAllBrands();
                var types = _deviceTypeService.GetAllTypes();

                CbBrand.ItemsSource = brands;
                CbType.ItemsSource = types;

                CbBrand.SelectedValue = _editingDevice.BrandID;

                CbType.SelectedValue = _editingDevice.TypeID;

                if (_editingDevice.TypeID > 0)
                {
                    LoadSpecs(_editingDevice.TypeID);
                    CbSpec.SelectedValue = _editingDevice.SpecID;
                }

                TxtDeviceModel.Text = _editingDevice.Model;
                TxtSerial.Text = _editingDevice.SerialNumber;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ أثناء تحميل البيانات: {ex.Message}");
            }
        }

        private void LoadSpecs(int typeId)
        {
            var specs = _deviceSpecService.GetSpecsByTypeId(typeId);
            CbSpec.ItemsSource = specs;
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbType.SelectedValue is int selectedTypeId)
            {
                LoadSpecs(selectedTypeId);
            }
            else
            {
                CbSpec.ItemsSource = null;
            }
        }

        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (CbType.SelectedValue == null || CbBrand.SelectedValue == null || CbSpec.SelectedValue == null)
            {
                MessageBox.Show("برجاء إكمال البيانات الإلزامية (*)");
                return;
            }

            try
            {
                _editingDevice.TypeID = (int)CbType.SelectedValue;
                _editingDevice.BrandID = (int)CbBrand.SelectedValue;
                _editingDevice.SpecID = (int)CbSpec.SelectedValue;

                _editingDevice.TypeName = (CbType.SelectedItem as TypeDto)?.TypeName;
                _editingDevice.BrandName = (CbBrand.SelectedItem as BrandDto)?.BrandName;
                _editingDevice.SpecName = (CbSpec.SelectedItem as SpecDto)?.SpecName;

                _editingDevice.Model = TxtDeviceModel.Text.Trim();
                _editingDevice.SerialNumber = TxtSerial.Text?.Trim();

                 bool isUpdated = _deviceService.UpdateCustomerDevice(_editingDevice);

                if (isUpdated)
                {
                    MessageBox.Show("تم تحديث بيانات الجهاز بنجاح");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("فشل تحديث البيانات في قاعدة البيانات");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء التحديث: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}