using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement.Queries;
using Application.Features.DeviceManagement.Commands;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement.Queries;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.Customer_View
{
    public partial class UpdateDeviceWindow : Window
    {
        private readonly DeviceInfoDTO _editingDevice;
        private readonly UpdateDeviceHandler _updateDeviceHandler;
        private readonly GetAllBrandsHandler _getBrandsHandler;
        private readonly GetAllTypesHandler _getTypesHandler;
        private readonly GetSpecsByTypeIdHandler _getSpecsHandler;

        public UpdateDeviceWindow(DeviceInfoDTO deviceToEdit,
                                  UpdateDeviceHandler updateDeviceHandler,
                                  GetAllBrandsHandler getBrandsHandler,
                                  GetAllTypesHandler getTypesHandler,
                                  GetSpecsByTypeIdHandler getSpecsHandler)
        {
            InitializeComponent();
            _editingDevice = deviceToEdit;
            _updateDeviceHandler = updateDeviceHandler;
            _getBrandsHandler = getBrandsHandler;
            _getTypesHandler = getTypesHandler;
            _getSpecsHandler = getSpecsHandler;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                CbBrand.ItemsSource = _getBrandsHandler.Handle();
                CbType.ItemsSource = _getTypesHandler.Handle();

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
            CbSpec.ItemsSource = _getSpecsHandler.Handle(typeId);
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbType.SelectedValue is int selectedTypeId)
                LoadSpecs(selectedTypeId);
            else
                CbSpec.ItemsSource = null;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (CbType.SelectedValue == null)
            {
                TxtTypeError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else TxtTypeError.Visibility = Visibility.Collapsed;

            if (CbBrand.SelectedValue == null)
            {
                TxtBrandError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else TxtBrandError.Visibility = Visibility.Collapsed;

            if (CbSpec.SelectedValue == null)
            {
                TxtSpecError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else TxtSpecError.Visibility = Visibility.Collapsed;

            if (!isValid) return;

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

                bool updated = _updateDeviceHandler.Handle(_editingDevice);
                if (updated)
                {
                    MessageBox.Show("تم تحديث بيانات الجهاز بنجاح", "نجاح",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("فشل تحديث البيانات", "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء التحديث: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}