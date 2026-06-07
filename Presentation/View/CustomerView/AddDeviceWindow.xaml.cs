using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement;
using Application.Features.BrandManagement.Queries;
using Application.Features.CustomerManagment;
using Application.Features.DeviceManagement;
using Application.Features.DeviceManagement.Commands;
using Application.Features.SpecManagement;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement;
using Application.Features.TypeManagement.Queries;
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
        private readonly AddDeviceToCustomerHandler _addDeviceHandler;
        private readonly GetAllBrandsHandler _getBrandsHandler;
        private readonly GetAllTypesHandler _getTypesHandler;
        private readonly GetSpecsByTypeIdHandler _getSpecsHandler;

        public AddDeviceWindow(int customerId, AddDeviceToCustomerHandler addDeviceHandler,
            GetAllBrandsHandler getBrandsHandler, GetAllTypesHandler getTypesHandler,
            GetSpecsByTypeIdHandler getSpecsHandler)
        {
            InitializeComponent();
            _customerId = customerId;
            _addDeviceHandler = addDeviceHandler;
            _getBrandsHandler = getBrandsHandler;
            _getTypesHandler = getTypesHandler;
            _getSpecsHandler = getSpecsHandler;

            CbBrand.ItemsSource = _getBrandsHandler.Handle();
            CbType.ItemsSource = _getTypesHandler.Handle();
        }

        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbType.SelectedValue is int selectedTypeId)
            {
                CbSpec.ItemsSource = _getSpecsHandler.Handle(selectedTypeId);
                CbSpec.SelectedIndex = -1;
                TxtSpecError.Visibility = Visibility.Collapsed;
            }
            else
            {
                CbSpec.ItemsSource = null;
            }
        }

        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (CbType.SelectedItem is not TypeDto)
            {
                TxtTypeError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else TxtTypeError.Visibility = Visibility.Collapsed;

            if (CbBrand.SelectedItem is not BrandDto)
            {
                TxtBrandError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else TxtBrandError.Visibility = Visibility.Collapsed;

            if (CbSpec.SelectedItem is not SpecDto)
            {
                TxtSpecError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else TxtSpecError.Visibility = Visibility.Collapsed;

            if (!isValid) return;

            try
            {
                var dto = new DeviceAddDTO
                {
                    TypeID = ((TypeDto)CbType.SelectedItem).TypeID,
                    BrandID = ((BrandDto)CbBrand.SelectedItem).BrandID,
                    SpecID = ((SpecDto)CbSpec.SelectedItem).SpecID,
                    Model = TxtDeviceModel.Text.Trim(),
                    SerialNumber = TxtSerial.Text?.Trim()
                };

                bool added = _addDeviceHandler.AddDeviceToCustomer(_customerId, dto);
                if (added)
                {
                    MessageBox.Show("تم إضافة الجهاز بنجاح", "نجاح",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("فشل في إضافة الجهاز", "خطأ",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "خطأ في البيانات",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء الحفظ: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
