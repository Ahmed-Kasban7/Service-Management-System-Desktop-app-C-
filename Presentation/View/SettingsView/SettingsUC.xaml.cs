using Application.DTOs.CompanySettingsDTOs;
using Application.DTOs.DepartmentDTOs;
using Application.DTOs.DepartmentRolesDTOs;
using Application.DTOs.DeviceDTOs;
using Application.DTOs.SourceDTOs;
using Application.Features.BrandManagement.Commands;
using Application.Features.BrandManagement.Queries;
using Application.Features.CompanySettingsManagement;
using Application.Features.DepartmentManagement;
using Application.Features.SourceManagement;
using Application.Features.SpecManagement.Commands;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement.Commands;
using Application.Features.TypeManagement.Queries;
using Presentation.View.MainView;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Presentation.View.Settings_View
{
    public partial class SettingsUC : UserControl
    {
        private byte[] _selectedLogoBytes = null;
        private  UpdateCompanySettingsHandler _updateSettingsHandler;
        private GetCompanySettingsHandler _getCompanySettingsHandler;
        private GetAllTypesHandler _getAllTypesHandler;
        private GetAllBrandsHandler _getAllBrandsHandler;
        private GetSpecsByTypeIdHandler _getSpecsByType;
        private GetAllSpecsHandler _getAllSpecsHandler;
        private GetDepartmentsLookupHandler _getAllDepartmentsHandler;
        private GetAllDepartmentRolesHandler _getAllDepartmentRolesHandler;
        private GetAllSourcesHandler _getAllSource;

        private CreateBrandHandler _createBrandHandler;
        private DeleteBrandHandler _deleteBrandHandler;
        private UpdateBrandHandler _updateBrandHandler;

        private AddTypeHandler _addTypeHandler;
        private DeleteTypeHandler _deleteType;
        private UpdateTypeHandler _updateTypeHandler;

        private AddSpecHandler _addSpecHandler;
        private DeleteSpecHandler _deleteSpecHandler;
        private UpdateSpecHandler _updateSpecHandler;

        private AddDepartmentHandler _addDepartmentHandler;
        private DeleteDepartmentHandler _deleteDepartmentHandler;
        private UpdateDepartmentHandler _updateDepartmentHandler;

        private AddRoleHandler _addRoleHandler;
        private DeleteRoleHandler _deleteRoleHandler;
        private UpdateRoleHandler _updateRoleHandler;

        private AddSourceHandler _addSourceHandler;
        private DeleteSourceHandler _deleteSourceHandler;
        private UpdateSourceHandler _updateSourceHandler;

        public SettingsUC()
        {
            InitializeComponent();
        }

        public void InitializeServices(
            GetAllTypesHandler getAllTypesHandler, GetAllBrandsHandler getAllBrands, GetSpecsByTypeIdHandler specsByTypeIdHandler,
            CreateBrandHandler createBrandHandler, DeleteBrandHandler deleteBrand, UpdateBrandHandler updateBrandHandler,
            AddTypeHandler addTypeHandler, DeleteTypeHandler deleteType, UpdateTypeHandler updateType,
            GetAllSpecsHandler getAllSpecs, AddSpecHandler addSpecHandler, DeleteSpecHandler deleteSpec, UpdateSpecHandler updateSpecHandler,
            DeleteDepartmentHandler deleteDepartment, UpdateDepartmentHandler updateDepartment, AddDepartmentHandler addDepartment,
            GetDepartmentsLookupHandler getDepartments, AddRoleHandler addRoleHandler, DeleteRoleHandler deleteRole,
            UpdateRoleHandler updateRole,
            GetAllDepartmentRolesHandler getAllDepartmentRoles, GetAllSourcesHandler getAllSources,
           
            AddSourceHandler addSourceHandler, UpdateSourceHandler updateSource,
            DeleteSourceHandler deleteSource , UpdateCompanySettingsHandler updateCompanySettings , GetCompanySettingsHandler getCompanySettingsHandler)
        {
            _getAllTypesHandler = getAllTypesHandler;
            _getAllBrandsHandler = getAllBrands;
            _getSpecsByType = specsByTypeIdHandler;
            _createBrandHandler = createBrandHandler;
            _deleteBrandHandler = deleteBrand;
            _updateBrandHandler = updateBrandHandler;
            _addTypeHandler = addTypeHandler;
            _deleteType = deleteType;
            _updateTypeHandler = updateType;
            _getAllSpecsHandler = getAllSpecs;
            _addSpecHandler = addSpecHandler;
            _deleteSpecHandler = deleteSpec;
            _updateSpecHandler = updateSpecHandler;
            _deleteDepartmentHandler = deleteDepartment;
            _updateDepartmentHandler = updateDepartment;
            _addDepartmentHandler = addDepartment;
            _getAllDepartmentsHandler = getDepartments;
            _addRoleHandler = addRoleHandler;
            _deleteRoleHandler = deleteRole;
            _updateRoleHandler = updateRole;
            _getAllDepartmentRolesHandler = getAllDepartmentRoles;
            _getAllSource = getAllSources;
            _addSourceHandler = addSourceHandler;
            _updateSourceHandler = updateSource;
            _deleteSourceHandler = deleteSource;
            _updateSettingsHandler = updateCompanySettings;
            _getCompanySettingsHandler = getCompanySettingsHandler;
            LoadSavedLogo();

        }

        #region Data Loading
        private void LoadAllData()
        {
            ExecuteSafe(() =>
            {
                BrandsList.ItemsSource = _getAllBrandsHandler.Handle();
                TypesList.ItemsSource = _getAllTypesHandler.Handle();
                SpecsList.ItemsSource = _getAllSpecsHandler.Handle();
                ComboDeviceType.ItemsSource = _getAllTypesHandler.Handle();
            }, "فشل تحميل بيانات الأجهزة والأنواع.");
        }

        private void LoadHRData()
        {
            ExecuteSafe(() =>
            {
                DepartmentsList.ItemsSource = _getAllDepartmentsHandler.Handle();
                ComboHRDepartment.ItemsSource = _getAllDepartmentsHandler.Handle();
                RolesList.ItemsSource = _getAllDepartmentRolesHandler.Handle();
            }, "فشل تحميل بيانات الموارد البشرية والأقسام.");
        }

        private void LoadSourceData()
        {
            ExecuteSafe(() =>
            {
                SourcesList.ItemsSource = _getAllSource.Handle();
            }, "فشل تحميل بيانات المصادر.");
        }
        #endregion

        #region Generic UI Edit Mode Toggle
        private void EditField_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Parent is Grid grid)
            {
                var textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
                var textBox = grid.Children.OfType<TextBox>().FirstOrDefault();

                if (textBlock != null && textBox != null)
                {
                    textBox.Text = textBlock.Text;
                    textBlock.Visibility = Visibility.Collapsed;
                    textBox.Visibility = Visibility.Visible;
                    textBox.Focus();
                    textBox.SelectAll();
                }
            }
        }

        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (e.Key == Key.Enter)
                {
                    CommitEditDynamic(textBox);
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    CancelEdit(textBox);
                    e.Handled = true;
                }
            }
        }

        private void TxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CommitEditDynamic(sender as TextBox);
        }

        private void CancelEdit(TextBox textBox)
        {
            if (textBox == null) return;
            var grid = textBox.Parent as Grid;
            var textBlock = grid?.Children.OfType<TextBlock>().FirstOrDefault();
            ToggleVisibility(textBlock, textBox, isEditing: false);
        }

        private void ToggleVisibility(TextBlock block, TextBox box, bool isEditing)
        {
            if (box != null) box.Visibility = isEditing ? Visibility.Visible : Visibility.Collapsed;
            if (block != null) block.Visibility = isEditing ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion

        #region Dynamic Save Logic (تحديث المنطق ليشمل تعديل المصادر تلقائياً)
        private void CommitEditDynamic(TextBox textBox)
        {
            if (textBox == null || textBox.Visibility == Visibility.Collapsed) return;

            var grid = textBox.Parent as Grid;
            var textBlock = grid?.Children.OfType<TextBlock>().FirstOrDefault();
            string currentText = textBlock?.Text ?? string.Empty;
            string newText = textBox.Text.Trim();

            if (newText == currentText || string.IsNullOrWhiteSpace(newText))
            {
                ToggleVisibility(textBlock, textBox, isEditing: false);
                if (string.IsNullOrWhiteSpace(newText) && textBlock != null) textBox.Text = currentText;
                return;
            }

            ExecuteSafe(() =>
            {
                bool isUpdated = false;
                string warningMessage = "عفواً، هذه البيانات مسجلة بالفعل في النظام!";

                switch (textBox.DataContext)
                {
                    case BrandDto brand:
                        isUpdated = _updateBrandHandler.Handle(brand.BrandID, newText);
                        warningMessage = "عفواً، هذه الماركة مسجلة بالفعل في النظام!";
                        break;

                    case TypeDto type:
                        isUpdated = _updateTypeHandler.Handle(type.TypeID, newText);
                        warningMessage = "عفواً، هذا النوع مسجل بالفعل في النظام!";
                        break;

                    case SpecDto spec:
                        isUpdated = _updateSpecHandler.Handle(spec.SpecID, newText);
                        warningMessage = "عفواً، هذه المواصفة مسجلة بالفعل لهذا النوع!";
                        break;

                    case DepartmentLookupDto dept:
                        isUpdated = _updateDepartmentHandler.Handle(newText, dept.DepartmentId);
                        warningMessage = "عفواً، هذا القسم مسجل بالفعل في النظام!";
                        break;

                    case DepartmentWithRolesDto role:
                        isUpdated = _updateRoleHandler.Handle(newText, role.RoleId);
                        warningMessage = "عفواً، هذا الدور الوظيفي مسجل بالفعل لهذا القسم!";
                        break;

                    case SourceDto source:
                        isUpdated = _updateSourceHandler.Handle(source.SourceId, newText);
                        warningMessage = "عفواً، هذا المصدر مسجل بالفعل في النظام!";
                        break;
                }

                if (!isUpdated)
                {
                    MessageBox.Show(warningMessage, "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (textBox.DataContext is DepartmentLookupDto || textBox.DataContext is DepartmentWithRolesDto) LoadHRData();
                else if (textBox.DataContext is SourceDto) LoadSourceData();
                else LoadAllData();

            }, "حدث خطأ أثناء حفظ التعديلات.");

            ToggleVisibility(textBlock, textBox, isEditing: false);
        }
        #endregion

        #region Add Operations
        private void BtnAddBrand_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewBrand.Text)) return;
            ExecuteHandler(() => _createBrandHandler.Handle(TxtNewBrand.Text.Trim()), TxtNewBrand, LoadAllData, "عفواً، هذه الماركة مسجلة بالفعل في النظام!");
        }

        private void BtnAddType_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewType.Text)) return;
            ExecuteHandler(() => _addTypeHandler.Handle(TxtNewType.Text.Trim()), TxtNewType, LoadAllData, "عفواً، نوع الجهاز هذا مسجل بالفعل في النظام!");
        }

        private void BtnAddSpec_Click(object sender, RoutedEventArgs e)
        {
            if (ComboDeviceType.SelectedValue == null || string.IsNullOrWhiteSpace(TxtNewSpec.Text)) return;

            ExecuteSafe(() =>
            {
                int selectedTypeId = Convert.ToInt32(ComboDeviceType.SelectedValue);
                if (_addSpecHandler.Handle(TxtNewSpec.Text.Trim(), selectedTypeId))
                {
                    TxtNewSpec.Clear();
                    SpecsList.ItemsSource = _getAllSpecsHandler.Handle();
                }
                else
                {
                    MessageBox.Show("عفواً، هذه المواصفة مسجلة بالفعل لهذا النوع من الأجهزة!", "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }, "خطأ في قاعدة البيانات أثناء إضافة المواصفة.");
        }

        private void BtnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewDepartment.Text))
            {
                return;
            }
            ExecuteHandler(() => _addDepartmentHandler.Handle(TxtNewDepartment.Text.Trim()), TxtNewDepartment, LoadHRData, "عفواً، القسم هذا مسجل بالفعل في النظام!");
        }

        private void BtnAddRole_Click(object sender, RoutedEventArgs e)
        {
            if (ComboHRDepartment.SelectedValue == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtNewRole.Text)) return;

            ExecuteSafe(() =>
            {
                int selectedDeptId = Convert.ToInt32(ComboHRDepartment.SelectedValue);
                if (_addRoleHandler.Handle(TxtNewRole.Text.Trim(), selectedDeptId))
                {
                    TxtNewRole.Clear();
                    LoadHRData();
                }
                else
                {
                    MessageBox.Show("عفواً، هذا الدور الوظيفي مسجل بالفعل لهذا القسم!", "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }, "خطأ أثناء إضافة الدور الوظيفي.");
        }

        private void BtnAddSource_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewSource.Text)) return;
            ExecuteHandler(() => _addSourceHandler.Handle(TxtNewSource.Text.Trim()), TxtNewSource, LoadSourceData, "عفواً، هذا المصدر مسجل بالفعل في النظام!");
        }
        #endregion

        #region Delete Operations
        private void BtnDeleteListItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext;
            if (item == null) return;

            ExecuteSafe(() =>
            {
                if (item is BrandDto b && ConfirmDelete("هذه الماركة"))
                {
                    if (_deleteBrandHandler.Handle(b.BrandID)) BrandsList.ItemsSource = _getAllBrandsHandler.Handle();
                    else ShowDeleteWarning();
                }
                else if (item is TypeDto t && ConfirmDelete("هذا النوع"))
                {
                    if (_deleteType.Handle(t.TypeID)) TypesList.ItemsSource = _getAllTypesHandler.Handle();
                    else ShowDeleteWarning();
                }
                else if (item is SpecDto s && ConfirmDelete("هذه المواصفة"))
                {
                    if (_deleteSpecHandler.Handle(s.SpecID)) SpecsList.ItemsSource = _getAllSpecsHandler.Handle();
                    else ShowDeleteWarning();
                }
            }, "خطأ أثناء الحذف.");
        }

        private void BtnDeleteDeptItem_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is DepartmentLookupDto dept && ConfirmDelete("هذا القسم"))
            {
                ExecuteSafe(() =>
                {
                    if (_deleteDepartmentHandler.Handle(dept.DepartmentId)) LoadHRData();
                    else MessageBox.Show("عفواً، لا يمكن حذف قسم مربوط بموظفين!", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                }, "خطأ أثناء حذف القسم.");
            }
        }

        private void BtnDeleteRoleItem_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is DepartmentWithRolesDto role && ConfirmDelete("هذا الدور الوظيفي"))
            {
                ExecuteSafe(() =>
                {
                    if (_deleteRoleHandler.Handle(role.RoleId)) LoadHRData();
                    else MessageBox.Show("عفواً، لا يمكن حذف دور وظيفي مرتبط بموظفين !", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                }, "خطأ أثناء حذف الدور الوظيفي.");
            }
        }

        private void BtnDeleteSourceItem_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is SourceDto source && ConfirmDelete("هذا المصدر"))
            {
                ExecuteSafe(() =>
                {
                    if (_deleteSourceHandler.Handle(source.SourceId)) LoadSourceData();
                    else MessageBox.Show("عفواً، لا يمكن حذف مصدر مرتبط بحملة أو بعميل !", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                }, "خطأ أثناء حذف المصدر.");
            }
        }
        #endregion

        #region Navigation & Panels
        private void BtnGeneralSettings_Checked(object sender, RoutedEventArgs e) {
            ShowPanel(GeneralSettingsPanel);
            LoadSavedLogo();
        }
        private void BtnHardwareSettings_Checked(object sender, RoutedEventArgs e)
        {
            ShowPanel(HardwareSettingsPanel);
            LoadAllData();
        }
        private void BtnBackupSettings_Checked(object sender, RoutedEventArgs e) => ShowPanel(BackupSettingsPanel);
        private void BtnHRSettings_Checked(object sender, RoutedEventArgs e)
        {
            ShowPanel(HRSettingsPanel);
            LoadHRData();
        }
        private void BtnSourcesSettings_Checked(object sender, RoutedEventArgs e)
        {
            ShowPanel(SourcesSettingsPanel);
            LoadSourceData();
        }

        private void ShowPanel(Border targetPanel)
        {
            if (GeneralSettingsPanel == null || HardwareSettingsPanel == null || BackupSettingsPanel == null || HRSettingsPanel == null || SourcesSettingsPanel == null) return;

            GeneralSettingsPanel.Visibility = Visibility.Collapsed;
            HardwareSettingsPanel.Visibility = Visibility.Collapsed;
            BackupSettingsPanel.Visibility = Visibility.Collapsed;
            HRSettingsPanel.Visibility = Visibility.Collapsed;
            SourcesSettingsPanel.Visibility = Visibility.Collapsed;

            if (targetPanel != null) targetPanel.Visibility = Visibility.Visible;
        }
        #endregion

        #region Helpers
        private bool ConfirmDelete(string itemName)
        {
            return MessageBox.Show($"هل أنت متأكد من حذف {itemName}؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        private void ShowDeleteWarning()
        {
            MessageBox.Show("لا يمكن الحذف لارتباطه بأجهزة مرتبطة في النظام.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ExecuteSafe(Action action, string errorTitle = "خطأ")
        {
            try { action(); }
            catch (Exception ex) { MessageBox.Show($"{errorTitle}\n{ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ExecuteHandler(Func<bool> handlerFunc, TextBox inputTextBox, Action refreshGridAction, string duplicateMessage)
        {
            try
            {
                if (handlerFunc())
                {
                    inputTextBox.Clear();
                    refreshGridAction();
                }
                else
                {
                    MessageBox.Show(duplicateMessage, "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ"); }
        }
        #endregion

        #region Unused Placeholders & Events

        private void LoadSavedLogo()
        {
            try
            {
                if (_getCompanySettingsHandler == null) return;

                var companySettings = _getCompanySettingsHandler.Handle();

                if (companySettings != null)
                {
                    TxtCenterName.Text = companySettings.CompanyName;

                    if (companySettings.CompanyLogo != null && companySettings.CompanyLogo.Length > 0)
                    {
                        _selectedLogoBytes = companySettings.CompanyLogo;

                        using var stream = new MemoryStream(_selectedLogoBytes);
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();

                        LogoImage.Source = bitmap;

                        LogoImage.Visibility = Visibility.Visible;
                        DefaultLogoState.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ClearLogoState();
                    }

                    LockFields();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error Loading Logo: {ex.Message}");
            }
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCenterName.Text))
            {
                MessageBox.Show("برجاء إدخال اسم الشركة أولاً!", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var dto = new CompanySettingsDto
                {
                    CompanyName = TxtCenterName.Text.Trim(),
                    CompanyLogo = _selectedLogoBytes
                };

                _updateSettingsHandler.Handle(dto);
                
                MessageBox.Show("تم حفظ البيانات وشعار النظام بنجاح!", "تمت العملية", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadSavedLogo();

                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.LoadSavedLogo();
                }
     
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ غير متوقع أثناء الحفظ: {ex.Message}", "خطأ في السيستم", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnUploadLogo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; *.jpeg; *.png; *.bmp",
                    Title = "اختر شعار المركز الرسمي"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    _selectedLogoBytes = File.ReadAllBytes(openFileDialog.FileName);

                    using var stream = new MemoryStream(_selectedLogoBytes);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();

                    LogoImage.Source = bitmap;

                    LogoImage.Visibility = Visibility.Visible;
                    DefaultLogoState.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تحميل الصورة: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LockFields()
        {
            TxtCenterName.IsReadOnly = true;
            BtnUploadLogo.IsEnabled = false;
            BtnSaveSettings.IsEnabled = false;
        }
        private void BtnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            TxtCenterName.IsReadOnly = false;
            BtnUploadLogo.IsEnabled = true;
            BtnSaveSettings.IsEnabled = true;

            TxtCenterName.Focus();
            TxtCenterName.SelectAll();
        }
        private void ClearLogoState()
        {
            _selectedLogoBytes = null;
            LogoImage.Source = null;
            LogoImage.Visibility = Visibility.Collapsed;
            DefaultLogoState.Visibility = Visibility.Visible;
        }
        private void ComboHRDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        #endregion
    }
}