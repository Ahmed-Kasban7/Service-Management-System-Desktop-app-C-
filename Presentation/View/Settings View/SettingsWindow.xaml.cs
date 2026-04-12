using Application.DTOs.DeviceDTOs;
using Application.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.Settings_View
{
    public partial class SettingsUC : UserControl
    {
        private DeviceTypeService _typeService;
        private DeviceBrandService _brandService;
        private DeviceSpecService _specService;

        public SettingsUC()
        {
            InitializeComponent();
        }

        public void InitializeServices(DeviceTypeService typeService, DeviceBrandService brandService, DeviceSpecService specService)
        {
            _typeService = typeService;
            _brandService = brandService;
            _specService = specService;
            LoadAllData();
        }

        private void LoadAllData()
        {

            try
            {
                TypesList.ItemsSource = _typeService.GetAllTypes();
                BrandsList.ItemsSource = _brandService.GetAllBrands();
                SpecsList.ItemsSource = _specService.GetAllSpecs();
                var allTypes = _typeService.GetAllTypes();
                ComboDeviceType.ItemsSource = _typeService.GetAllTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل تحميل البيانات.\n{ex.Message}",
                                "خطأ",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void BtnAddType_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewType.Text))
            {
                return;
            }

            try
            {
                var result = _typeService.AddType(TxtNewType.Text);

                if (result)
                {
                    TxtNewType.Clear();
                    LoadAllData();
                }
                else
                {
                    MessageBox.Show("فشل إضافة النوع.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddBrand_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewBrand.Text))
            {
                return;
            }

            try
            {
                bool result = _brandService.AddDeviceBrand(TxtNewBrand.Text);

                if (result)
                {
                    TxtNewBrand.Clear();
                    LoadAllData();
                }
                else
                {
                    MessageBox.Show("فشل إضافة البراند.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddSpec_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewSpec.Text))
            {
                return;
            }

            if (ComboDeviceType.SelectedValue == null)
            {
                return;
            }
            string specName = TxtNewSpec.Text.Trim();
            int selectedTypeId = (int)ComboDeviceType.SelectedValue;

            try
            {
                var result = _specService.AddSpec(specName, selectedTypeId);

                if (result)
                {
                    TxtNewSpec.Clear();
                    LoadAllData();
                }
                else
                {
                    MessageBox.Show("فشل إضافة الموصفات.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnDeleteListItem_Click(object sender, RoutedEventArgs e)
        {
            //var btn = sender as Button;
            //var item = btn?.Tag;

            //if (item == null) return;

            //var res = MessageBox.Show("هل أنت متأكد من حذف هذا العنصر؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            //if (res != MessageBoxResult.Yes) return;

            //try
            //{
            //    if (item is TypeDto t) _typeService.DeleteType(t.TypeID);
            //    else if (item is BrandDto b) _brandService.DeleteBrand(b.BrandID);
            //    else if (item is SpecDto s) _specService.DeleteSpec(s.SpecID);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"فشل الحذف: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        // --- ميثود التنقل بين اللوحات (Tabs) ---
        private void BtnGeneralSettings_Checked(object sender, RoutedEventArgs e) => ShowPanel(GeneralSettingsPanel);
        private void BtnHardwareSettings_Checked(object sender, RoutedEventArgs e) => ShowPanel(HardwareSettingsPanel);
        private void BtnBackupSettings_Checked(object sender, RoutedEventArgs e) => ShowPanel(BackupSettingsPanel);

        private void ShowPanel(Border targetPanel)
        {
            if (GeneralSettingsPanel == null || HardwareSettingsPanel == null || BackupSettingsPanel == null)
                return;

            GeneralSettingsPanel.Visibility = Visibility.Collapsed;
            HardwareSettingsPanel.Visibility = Visibility.Collapsed;
            BackupSettingsPanel.Visibility = Visibility.Collapsed;

            if (targetPanel != null) targetPanel.Visibility = Visibility.Visible;
        }

        private void BtnDeleteSpec_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUploadLogo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnResetSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveLogo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}