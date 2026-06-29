using Application.DTOs.EmployeeDTOs;
using Application.Features.AttachmentManagement.Commands;
using Application.Features.AttachmentManagement.Queries;
using Application.Features.DepartmentManagement;
using Application.Features.EmployeeManagement.Commands;
using Application.Features.EmployeeManagement.Queries;
using Application.Features.PhoneManagement.Commands;
using Application.Features.PhoneManagement.Queries;
using Domain.Enums;
using Presentation.View.CustomerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Presentation.View.EmployeeView
{
    /// <summary>
    /// Interaction logic for EmployeeProfileUC.xaml
    /// </summary>
    public partial class EmployeeProfileUC : UserControl
    {
        public event EventHandler BackRequested;
        public event EventHandler<int> OrderDetailsRequested;

        private readonly GetEmployeeProfileHandler _getEmployeeProfileHandler;
        private readonly UpdateEmployeeHandler _updateEmployeeHandler;
        //private readonly DeleteEmployeeHandler _deleteEmployeeHandler;
        //private readonly GetEmployeeOrdersHandler _getEmployeeOrdersHandler;

        private readonly GetEmployeePhonesHandler _getEmployeePhonesHandler; 
        private readonly AddPhoneToEmployee _addPhoneToEmployee;             
        private readonly DeleteEmployeePhoneHandler _deletePhoneHandler;
        private readonly UpdatePhoneHandler _updatePhoneHandler;
        private readonly AddAttachmentHandler _addAttachmentHandler;
        private readonly DeleteAttachmentHandler _deleteAttachmentHandler;
        private readonly GetDepartmentsLookupHandler _getDepartmentsLookupHandler;
        private readonly GetRolesByDepartmentHandler _getRolesByDepartmentHandler;

        private readonly GetEmployeeAttachmentsHandler _employeeAttachmentsHandler;

        private readonly int _employeeId;

        public EmployeeProfileUC(
            int employeeId,GetEmployeeProfileHandler getEmployeeProfileHandler , GetEmployeePhonesHandler getEmployeePhones , 
            AddPhoneToEmployee phoneToEmployee , UpdatePhoneHandler updatePhoneHandler , 
            DeleteEmployeePhoneHandler deleteEmployeePhone , GetEmployeeAttachmentsHandler getEmployeeAttachments , 
            AddAttachmentHandler addAttachmentHandler , DeleteAttachmentHandler deleteAttachmentHandler ,
            GetDepartmentsLookupHandler getDepartmentsLookup , GetRolesByDepartmentHandler getRolesByDepartment , UpdateEmployeeHandler updateEmployee)
        {
            InitializeComponent();
            _employeeId = employeeId;

            _getEmployeeProfileHandler = getEmployeeProfileHandler;
            _getEmployeePhonesHandler  = getEmployeePhones;


            _addPhoneToEmployee = phoneToEmployee;
             _deletePhoneHandler = deleteEmployeePhone;
            _updatePhoneHandler = updatePhoneHandler;
            
            _employeeAttachmentsHandler = getEmployeeAttachments;
            _addAttachmentHandler = addAttachmentHandler;
            _deleteAttachmentHandler = deleteAttachmentHandler;
            _getDepartmentsLookupHandler = getDepartmentsLookup;
            _getRolesByDepartmentHandler = getRolesByDepartment;
            _updateEmployeeHandler = updateEmployee;

            LoadEmployeeProfileInfo();
        }

        private void LoadEmployeeProfileInfo()
        {
            try
            {
                var emp = _getEmployeeProfileHandler.Handle(_employeeId);
                if (emp == null) return;

                TxtProfileID.Text = emp.EmployeeNumber;
                TxtProfileName.Text = emp.Name;
                TxtProfileDepartment.Text = emp.DepartmentName;
                TxtProfileRole.Text = emp.RoleName;
                TxtProfileSex.Text = emp.Sex == 1 ? "أنثى" : "ذكر"; 
                TxtProfileAddress.Text = string.IsNullOrWhiteSpace(emp.Address)?"---" : emp.Address;
                TxtProfileAge.Text =
                    emp.Age?.ToString() ?? "---";
                TxtProfileHireDate.Text = $"{ emp.HireDate:yyyy/MM/dd}";

                RunEmployeeCode.Text = $"#{emp.EmployeeNumber}";
                TxtHireDate.Text = $"تاريخ التعيين: {emp.HireDate:yyyy/MM/dd}";

                TxtProfileCompensationType.Text = emp.CompensationTypeText;
                TxtProfileCompensationType.Text = emp.CompensationTypeText;

                switch (emp.CompensationType)
                {
                    case 0: // مرتب
                        TxtProfileBaseSalary.Text = $"{emp.BaseSalary:F0} ج.م";
                        TxtCommissionTitle.Text = "العمولة";
                        TxtProfileCommissionPercent.Text = "---";
                        break;

                    case 1: // عمولة
                        TxtProfileBaseSalary.Text = "---";

                        if (emp.CommissionType == true)
                        {
                            TxtCommissionTitle.Text = "العمولة الثابتة";
                            TxtProfileCommissionPercent.Text =
                                $"{emp.Commission:F0} ج.م";
                        }
                        else
                        {
                            TxtCommissionTitle.Text = "نسبة العمولة";
                            TxtProfileCommissionPercent.Text = $"%{emp.Commission:F0}";
                        }
                        break;

                    case 2: // مرتب + عمولة
                        TxtProfileBaseSalary.Text =
                            $"{emp.BaseSalary:F0} ج.م";

                        if (emp.CommissionType == true)
                        {
                            TxtCommissionTitle.Text = "العمولة الثابتة";
                            TxtProfileCommissionPercent.Text =
                                $"{emp.Commission:F0} ج.م";
                        }
                        else
                        {
                            TxtCommissionTitle.Text = "نسبة العمولة";
                            TxtProfileCommissionPercent.Text = $"%{emp.Commission:F0}";
                        }
                        break;

                    case 3: // بالمشوار
                        TxtProfileBaseSalary.Text = "---";
                        TxtCommissionTitle.Text = "الاستحقاق";
                        TxtProfileCommissionPercent.Text =
                            "قيمة المشوار كاملة";
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل بيانات الموظف", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadEmployeePhones()
        {
            try
            {
                var phones = _getEmployeePhonesHandler.Handle(_employeeId).ToList();

                if (phones != null && phones.Any())
                {
                    PhonesList.ItemsSource = phones;
                    TxtNoPhonesMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PhonesList.ItemsSource = null;
                    TxtNoPhonesMessage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل هواتف الموظف", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadEmployeeAttachments()
        {
            try
            {
                var attachments = _employeeAttachmentsHandler.Handle(_employeeId).ToList();

                if (attachments != null && attachments.Any())
                {
                    AttachmentsList.ItemsSource = attachments;
                    TxtNoAttachmentsMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    AttachmentsList.ItemsSource = null;
                    TxtNoAttachmentsMessage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل هواتف الموظف", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LoadEmployeeOrders()
        {
            //try
            //{
            //    var orders = _getEmployeeOrdersHandler.Handle(_employeeId).ToList();

            //    if (orders != null && orders.Any())
            //    {
            //        OrdersList.ItemsSource = orders;
            //        TxtNoOrdersMessage.Visibility = Visibility.Collapsed;
            //    }
            //    else
            //    {
            //        OrdersList.ItemsSource = null;
            //        TxtNoOrdersMessage.Visibility = Visibility.Visible;
            //    }
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("حدث خطأ أثناء تحميل العمليات المسجلة للموظف", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        // 4️⃣ فتح تفاصيل الطلب بالضغط مرتين على الجدول
        private void OrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (OrdersList.SelectedItem is dynamic selectedOrder) // أو اكتب اسم الـ DTO المختصر لعمليات الموظف هنا
            //{
            //    OrderDetailsRequested?.Invoke(this, selectedOrder.OrderId);
            //}
        }

        private void BtnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var emp = _getEmployeeProfileHandler.Handle(_employeeId);

                 var editWindow = new EditEmployee(_employeeId,emp , _getRolesByDepartmentHandler , _getDepartmentsLookupHandler  , _updateEmployeeHandler) 
                 { 
                     Owner = Window.GetWindow(this) 
                 };

                if (editWindow.ShowDialog() == true) 
                    LoadEmployeeProfileInfo();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 6️⃣ إنهاء الخدمة (الـ Soft Delete اللي اتكلمنا عليه)
        private void BtnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            //var confirm = MessageBox.Show(
            //    "هل أنت متأكد من إنهاء خدمة هذا الموظف؟\n(سيتم إيقاف حسابه ومنعه من الظهور في الشاشات المستقبلية مع الحفاظ على تاريخه الحسابي القديم)",
            //    "تأكيد إنهاء الخدمة",
            //    MessageBoxButton.YesNo,
            //    MessageBoxImage.Warning);

            //if (confirm == MessageBoxResult.Yes)
            //{
            //    var result = _deleteEmployeeHandler.Handle(_employeeId);
            //    if (result.IsSuccess)
            //    {
            //        MessageBox.Show("تم إنهاء خدمة الموظف بنجاح وتأمين بياناته.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
            //        LoadEmployeeProfileInfo(); // ريفريش للشاشة عشان تقلب أحمر (موقوف) وتأمن الزرار
            //    }
            //    else
            //    {
            //        MessageBox.Show(result.Error, "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }

        private void BtnAddPhone_Click(object sender, RoutedEventArgs e)
        {
            var addPhoneWindow = new AddPhoneToEmployeeWindow(_addPhoneToEmployee, _employeeId)
            {
                Owner = Window.GetWindow(this)
            };

            if (addPhoneWindow.ShowDialog() == true)
                LoadEmployeePhones();
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
                    LoadEmployeePhones();
            }
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

                        var deleted = _deletePhoneHandler.Handle(phoneNumber, _employeeId);

                        if (deleted.IsSuccess)
                        {
                            MessageBox.Show("تم حذف الرقم بنجاح", "نجاح",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            LoadEmployeePhones();

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
        private void TabAttachments_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(PanelAttachments, TabAttachments);
            LoadEmployeeAttachments();
        }

        private void SetActiveTab(Border panel, Button tab)
        {
            PanelBasicInfo.Visibility = Visibility.Collapsed;
            PanelFinancialInfo.Visibility = Visibility.Collapsed;
            PanelPhones.Visibility = Visibility.Collapsed;
            PanelOrders.Visibility = Visibility.Collapsed;
            PanelAttachments.Visibility = Visibility.Collapsed;

            foreach (var t in new[] { TabBasicInfo, TabFinancialInfo, TabPhones, TabOperations,TabAttachments })
            {
                if (t == null) continue;
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

        private void TabFinancialInfo_Click(object sender, RoutedEventArgs e) => SetActiveTab(PanelFinancialInfo, TabFinancialInfo);

        private void TabPhones_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(PanelPhones, TabPhones);
            LoadEmployeePhones();
        }

        private void TabOperations_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(PanelOrders, TabOperations);
            LoadEmployeeOrders();
        }

        public void BtnBack_Click(object sender, RoutedEventArgs e) => BackRequested?.Invoke(this, EventArgs.Empty);

        private void BtnViewAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is EmployeeAttachmentDto attachment)
                {
                    if (attachment.AttachmentData != null && attachment.AttachmentData.Length > 0)
                    {
                        string tempFileName = $"Attachment_{attachment.Id}_{Guid.NewGuid().ToString().Substring(0, 5)}.jpg";
                        string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), tempFileName);

                        System.IO.File.WriteAllBytes(tempFilePath, attachment.AttachmentData);

                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = tempFilePath,
                            UseShellExecute = true
                        });
                    }
                    else
                    {
                        MessageBox.Show("عذراً، ملف الصورة فارغ أو تالف.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"عذراً، فشل فتح الملف: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnDeleteAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is EmployeeAttachmentDto attachment)
            {
                var result = MessageBox.Show("هل أنت متأكد من حذف هذه الوثيقة نهائياً من قاعدة البيانات؟", "تأكيد الحذف",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _deleteAttachmentHandler.Handle(attachment.Id);

                        LoadEmployeeAttachments();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ غير متوقع أثناء الحذف: {ex.Message}", "خطأ في السيستم", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnUploadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp) | *.jpg; *.jpeg; *.png; *.bmp",
                    Title = "اختر وثيقة الموظف الرسمية"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFilePath = openFileDialog.FileName;

                    byte[] imageBytes = System.IO.File.ReadAllBytes(selectedFilePath);

                    _addAttachmentHandler.Handle(_employeeId, imageBytes);

                    LoadEmployeeAttachments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ غير متوقع أثناء الرفع: {ex.Message}", "خطأ في السيستم", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}