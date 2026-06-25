using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement.Commands;
using Application.Features.BrandManagement.Queries;
using Application.Features.SpecManagement.Commands;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement.Commands;
using Application.Features.TypeManagement.Queries;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation.View.Settings_View
{
    public partial class SettingsUC : UserControl
    {
        private GetAllTypesHandler _getAllTypesHandler;
        private GetAllBrandsHandler _getAllBrandsHandler;
        private GetSpecsByTypeIdHandler _getSpecsByType;

        private CreateBrandHandler _createBrandHandler;
        private DeleteBrandHandler _deleteBrandHandler;
        private UpdateBrandHandler _updateBrandHandler;

        private AddTypeHandler _addTypeHandler;
        private DeleteTypeHandler _deleteType;
        private UpdateTypeHandler _updateTypeHandler;

        private GetAllSpecsHandler _getAllSpecsHandler;

        private AddSpecHandler _addSpecHandler;
        private DeleteSpecHandler _deleteSpecHandler;

        private UpdateSpecHandler _updateSpecHandler;
        public SettingsUC()
        {
            InitializeComponent();
        }

        public void InitializeServices(
            GetAllTypesHandler getAllTypesHandler,
            GetAllBrandsHandler getAllBrands,
            GetSpecsByTypeIdHandler specsByTypeIdHandler,
            CreateBrandHandler createBrandHandler,
            DeleteBrandHandler deleteBrand,
            UpdateBrandHandler updateBrandHandler, AddTypeHandler addTypeHandler, 
            DeleteTypeHandler deleteType, UpdateTypeHandler updateType , GetAllSpecsHandler getAllSpecs 
            , AddSpecHandler addSpecHandler  , DeleteSpecHandler deleteSpec , UpdateSpecHandler updateSpecHandler)
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

            LoadAllData();
        }

        private void LoadAllData()
        {
            try
            {
                BrandsList.ItemsSource = _getAllBrandsHandler.Handle();
                TypesList.ItemsSource = _getAllTypesHandler.Handle();
                SpecsList.ItemsSource = _getAllSpecsHandler.Handle();
                ComboDeviceType.ItemsSource = _getAllTypesHandler.Handle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل تحميل البيانات.\n{ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EditField_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var grid = button?.Parent as Grid;
            if (grid == null) return;

            var textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
            var textBox = grid.Children.OfType<TextBox>().FirstOrDefault();

            if (textBlock == null || textBox == null) return;

            textBox.Text = textBlock.Text;

            textBlock.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;

            textBox.Focus();
            textBox.SelectAll();
        }

        private void TxtBoxEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CommitEdit(sender as TextBox);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                CancelBrandEdit(sender as TextBox);
                e.Handled = true;
            }
        }

        private void TxtBoxEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            CommitEdit(sender as TextBox);
        }

      
        private void CommitEdit(TextBox textBox)
        {
            if (textBox == null || textBox.Visibility == Visibility.Collapsed) return;

            var grid = textBox.Parent as Grid;
            var textBlock = grid?.Children.OfType<TextBlock>().FirstOrDefault();
            var boundItem = textBox.DataContext;

            string currentText = textBlock?.Text;
            string newText = textBox.Text.Trim();

            if (boundItem is BrandDto brand)
            {
                if (newText == currentText)
                {
                    ExitEditMode(textBlock, textBox);
                    return;
                }

                if (string.IsNullOrWhiteSpace(newText))
                {
                    MessageBox.Show("لا يمكن ترك الاسم فارغاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CancelBrandEdit(textBox);
                    return;
                }

                try
                {
                    bool isUpdated = _updateBrandHandler.Handle(brand.BrandID, newText);

                    if (!isUpdated)
                    {
                        MessageBox.Show("فشل تعديل البيانات في السيرفر.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "خطأ في التحقق", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadAllData();
                }
                return;
            }

            if (boundItem is TypeDto type)
            {
                if (newText == currentText)
                {
                    ExitEditMode(textBlock, textBox);
                    return;
                }

                if (string.IsNullOrWhiteSpace(newText))
                {
                    MessageBox.Show("لا يمكن ترك الاسم فارغاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CancelBrandEdit(textBox);
                    return;
                }

                try
                {
                    bool isUpdated = _updateTypeHandler.Handle(type.TypeID, newText);

                    if (!isUpdated)
                    {
                        MessageBox.Show("فشل تعديل البيانات في السيرفر.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "خطأ في التحقق", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    LoadAllData();
                }
                return;
            }

            ExitEditMode(textBlock, textBox);
        }

        private void CancelBrandEdit(TextBox textBox)
        {
            if (textBox == null) return;
            var grid = textBox.Parent as Grid;
            var textBlock = grid?.Children.OfType<TextBlock>().FirstOrDefault();
            ExitEditMode(textBlock, textBox);
        }

        private void ExitEditMode(TextBlock block, TextBox box)
        {
            if (block != null && box != null)
            {
                box.Visibility = Visibility.Collapsed;
                block.Visibility = Visibility.Visible;
            }
        }

        private void EditSpecField_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var grid = button?.Parent as Grid;
            if (grid == null) return;

            var textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();
            var textBox = grid.Children.OfType<TextBox>().FirstOrDefault();

            if (textBlock == null || textBox == null) return;

            textBox.Text = textBlock.Text;

            textBlock.Visibility = Visibility.Collapsed;
            textBox.Visibility = Visibility.Visible;

            textBox.Focus();
            textBox.SelectAll();
        }

        private void TxtSpecBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CommitSpecEdit(sender as TextBox);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                CancelSpecEdit(sender as TextBox);
                e.Handled = true;
            }
        }

        private void TxtSpecBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CommitSpecEdit(sender as TextBox);
        }

        private void CommitSpecEdit(TextBox textBox)
        {
            if (textBox == null || textBox.Visibility == Visibility.Collapsed) return;

            var grid = textBox.Parent as Grid;
            var textBlock = grid?.Children.OfType<TextBlock>().FirstOrDefault();
            var boundItem = textBox.DataContext;

            string currentText = textBlock?.Text;
            string newText = textBox.Text.Trim();

            if (newText == currentText)
            {
                ExitEditMode(textBlock, textBox);
                return;
            }

            if (string.IsNullOrWhiteSpace(newText))
            {
                MessageBox.Show("لا يمكن ترك اسم المواصفة فارغاً.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                CancelSpecEdit(textBox);
                return;
            }

            if (boundItem is SpecDto spec)
            {
                try
                {
                    if (!_updateSpecHandler.Handle(spec.SpecID, newText))
                    {
                        MessageBox.Show("فشل تعديل المواصفة في السيرفر.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                finally
                {
                    LoadAllData();
                }
            }
        }
        private void CancelSpecEdit(TextBox textBox)
        {
            if (textBox == null) return;
            var grid = textBox.Parent as Grid;
            var textBlock = grid?.Children.OfType<TextBlock>().FirstOrDefault();
            ExitEditMode(textBlock, textBox);
        }

        private void BtnAddBrand_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewBrand.Text)) return;
            try
            {
                if (_createBrandHandler.Handle(TxtNewBrand.Text.Trim()))
                {
                    TxtNewBrand.Clear();
                    LoadAllData(); 
                    MessageBox.Show("تم إضافة الماركة بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("عفواً، هذه الماركة مسجلة بالفعل في النظام!", "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ"); }
        }

        private void BtnDeleteListItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext;
            if (item is BrandDto b)
            {
                if (MessageBox.Show("هل أنت متأكد من حذف هذه الماركة؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (_deleteBrandHandler.Handle(b.BrandID)) BrandsList.ItemsSource = _getAllBrandsHandler.Handle();
                    else MessageBox.Show("لا يمكن الحذف لارتباطه بأجهزة مرتبطة في النظام.");
                }
            }
            if (item is TypeDto t)
            {
                if (MessageBox.Show("هل أنت متأكد من حذف هذه النوع؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (_deleteType.Handle(t.TypeID))
                        TypesList.ItemsSource = _getAllTypesHandler.Handle();
                    else MessageBox.Show("لا يمكن الحذف لارتباطه بأجهزة مرتبطة في النظام.");
                }
            }

            if (item is SpecDto s)
            {
                if (MessageBox.Show("هل أنت متأكد من حذف هذه المواصفة؟", "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (_deleteSpecHandler.Handle(s.SpecID))
                    {
                        SpecsList.ItemsSource = _getAllSpecsHandler.Handle();
                    }
                    else
                    {
                        MessageBox.Show("لا يمكن الحذف لارتباطه بأجهزة مرتبطة في النظام.", "تنبيه حماية البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }


        }

        private void BtnAddType_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNewType.Text)) return;
            try
            {
                if (_addTypeHandler.Handle(TxtNewType.Text.Trim()))
                {
                    TxtNewType.Clear();
                    LoadAllData(); 
                    MessageBox.Show("تم إضافة نوع الجهاز بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("عفواً، نوع الجهاز هذا مسجل بالفعل في النظام!", "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ"); }
        }
        private void BtnAddSpec_Click(object sender, RoutedEventArgs e)
        {
            if (ComboDeviceType.SelectedValue == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtNewSpec.Text)) 
            {
                return;
            }

            try
            {
                int selectedTypeId = Convert.ToInt32(ComboDeviceType.SelectedValue);

                if (_addSpecHandler.Handle(TxtNewSpec.Text.Trim(), selectedTypeId))
                {
                    TxtNewSpec.Clear();
                    SpecsList.ItemsSource = _getAllSpecsHandler.Handle(); 
                    MessageBox.Show("تم إضافة المواصفة بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("عفواً، هذه المواصفة مسجلة بالفعل لهذا النوع من الأجهزة!", "تنبيه تكرار البيانات", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ داتابيز", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnGeneralSettings_Checked(object sender, RoutedEventArgs e) => ShowPanel(GeneralSettingsPanel);
        private void BtnHardwareSettings_Checked(object sender, RoutedEventArgs e) => ShowPanel(HardwareSettingsPanel);
        private void BtnBackupSettings_Checked(object sender, RoutedEventArgs e) => ShowPanel(BackupSettingsPanel);

        private void ShowPanel(Border targetPanel)
        {
            if (GeneralSettingsPanel == null || HardwareSettingsPanel == null || BackupSettingsPanel == null) return;
            GeneralSettingsPanel.Visibility = Visibility.Collapsed;
            HardwareSettingsPanel.Visibility = Visibility.Collapsed;
            BackupSettingsPanel.Visibility = Visibility.Collapsed;
            if (targetPanel != null) targetPanel.Visibility = Visibility.Visible;
        }

        private void BtnUploadLogo_Click(object sender, RoutedEventArgs e) { }
        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e) { }
        private void BtnResetSettings_Click(object sender, RoutedEventArgs e) { }
        private void BtnRemoveLogo_Click(object sender, RoutedEventArgs e) { }
    }
}