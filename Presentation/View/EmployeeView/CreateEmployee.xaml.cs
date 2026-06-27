using Application.DTOs.EmployeeDTOs;
using Application.Features.DepartmentManagement;
using Application.Features.EmployeeManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.EmployeeView
{
    public class AttachmentItem
    {
        public string FilePath { get; set; }
        public string FileExtension => System.IO.Path.GetExtension(FilePath)?.ToLower();

        public bool IsImage => new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".ico" }.Contains(FileExtension);
    }
    public partial class CreateEmployee : Window
    {
        private readonly CreateEmployeeHandler _createEmployeeHandler;
        private readonly GetDepartmentsLookupHandler _getDepartmentsHandler;
        private readonly GetRolesByDepartmentHandler _getRolesHandler;

        public ObservableCollection<string> PhonesList { get; set; } = new ObservableCollection<string>();
        private ObservableCollection<AttachmentItem> AttachmentsList { get; set; } = new ObservableCollection<AttachmentItem>();

        public int _employeeId;

        public CreateEmployee(
            CreateEmployeeHandler createEmployee,
            GetDepartmentsLookupHandler getDepartments,
            GetRolesByDepartmentHandler getRoles)
        {
            InitializeComponent();

            _createEmployeeHandler = createEmployee;
            _getDepartmentsHandler = getDepartments;
            _getRolesHandler = getRoles;

            LstPhones.ItemsSource = PhonesList;
            ListAttachments.ItemsSource = AttachmentsList;


            CbDepartment.ItemsSource = _getDepartmentsHandler.Handle();

            UpdateNoAttachmentsMessage();

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate(out decimal? baseSalary, out decimal? commissionPercent))
                return;

            try
            {
                var selectedSex = (ComboBoxItem)CbSex.SelectedItem;
                byte sex = selectedSex.Content.ToString() == "ذكر" ? (byte)0 : (byte)1;

                int? age = string.IsNullOrWhiteSpace(TxtAge.Text)
                    ? null
                    : Convert.ToInt32(TxtAge.Text);

                var dto = new CreateEmployeeDto
                {
                    Name = TxtName.Text.Trim(),
                    Age = age,
                    Sex = sex,
                    HireDate = DpHireDate.SelectedDate.Value,
                    RoleId = (int)CbRole.SelectedValue,
                    DepartmentId = (int)CbDepartment.SelectedValue,
                    BaseSalary = baseSalary,
                    CommissionPercent = commissionPercent,
                    Address = string.IsNullOrWhiteSpace(TxtAddress.Text) ?null : TxtAddress.Text.Trim(),

                    CompensationType = ((ComboBoxItem)CbCompensationType.SelectedItem).Tag?.ToString() switch
                    {
                        "Salary" => 0,
                        "Commission" => 1,
                        "Both" => 3,
                        "FullTrip" => 4,
                        _ => 0
                    },

                    Phones = PhonesList.ToList(),
                    Attachments = AttachmentsList.Select(a => a.FilePath).ToList()

                };

                var res = _createEmployeeHandler.Handle(dto);

                MessageBox.Show(res.IsSuccess
                    ? "تم حفظ بيانات الموظف بنجاح"
                    : res.Error);

                if (res.IsSuccess)
                {
                    _employeeId = res.Value;
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء الحفظ برجاء إعادة المحاولة");
            }
        }

        private bool Validate(out decimal? baseSalary, out decimal? commissionPercent)
        {
            baseSalary = null;
            commissionPercent = null;
            bool hasError = false;

            TxtNameError.Text = "";
            TxtDepartmentError.Text = "";
            TxtRoleError.Text = "";
            TxtHireDateError.Text = "";
            TxtSexError.Text = "";
            TxtCompensationTypeError.Text = "";
            TxtSalartError.Text = "";
            TxtCompensationError.Text = "";
            TxtAgeError.Text = "";
            txtPhoneError.Text = "";

            TxtNameError.Visibility = Visibility.Collapsed;
            TxtDepartmentError.Visibility = Visibility.Collapsed;
            TxtRoleError.Visibility = Visibility.Collapsed;
            TxtHireDateError.Visibility = Visibility.Collapsed;
            TxtSexError.Visibility = Visibility.Collapsed;
            TxtCompensationTypeError.Visibility = Visibility.Collapsed;
            TxtSalartError.Visibility = Visibility.Collapsed;
            TxtCompensationError.Visibility = Visibility.Collapsed;
            TxtAgeError.Visibility = Visibility.Collapsed;
            txtPhoneError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                TxtNameError.Text = "برجاء إدخال اسم الموظف";
                TxtNameError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (CbDepartment.SelectedValue == null)
            {
                TxtDepartmentError.Text = "برجاء اختيار القسم";
                TxtDepartmentError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (CbRole.SelectedValue == null)
            {
                TxtRoleError.Text = "برجاء اختيار الدور الوظيفي";
                TxtRoleError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (DpHireDate.SelectedDate == null)
            {
                TxtHireDateError.Text = "برجاء اختيار تاريخ التعيين";
                TxtHireDateError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (CbSex.SelectedItem == null)
            {
                TxtSexError.Text = "برجاء اختيار الجنس";
                TxtSexError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!string.IsNullOrWhiteSpace(TxtAge.Text))
            {
                if (!int.TryParse(TxtAge.Text, out int age) || age <= 0)
                {
                    TxtAgeError.Text = "العمر غير صالح";
                    TxtAgeError.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            if (PhonesList.Count == 0)
            {
                txtPhoneError.Text = "يجب إدخال رقم هاتف واحد على الأقل";
                txtPhoneError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (CbCompensationType.SelectedItem == null)
            {
                TxtCompensationTypeError.Text = "برجاء اختيار نوع الحساب";
                TxtCompensationTypeError.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                var compensationItem = (ComboBoxItem)CbCompensationType.SelectedItem;
                string tag = compensationItem.Tag?.ToString();

                switch (tag)
                {
                    case "Salary":
                        if (!decimal.TryParse(TxtBaseSalary.Text, out decimal salary) || salary < 0)
                        {
                            TxtSalartError.Text = "برجاء إدخال مرتب صحيح";
                            TxtSalartError.Visibility = Visibility.Visible;
                            hasError = true;
                        }
                        else baseSalary = salary;
                        break;

                    case "Commission":
                        if (!decimal.TryParse(TxtCommissionPercent.Text, out decimal comm) || comm < 0 || comm > 100)
                        {
                            TxtCompensationError.Text = "نسبة العمولة يجب أن تكون بين 0 و 100";
                            TxtCompensationError.Visibility = Visibility.Visible;
                            hasError = true;
                        }
                        else commissionPercent = comm;
                        break;

                    case "Both":
                        if (!decimal.TryParse(TxtBaseSalary.Text, out decimal bothSalary) || bothSalary < 0)
                        {
                            TxtSalartError.Text = "برجاء إدخال مرتب صحيح";
                            TxtSalartError.Visibility = Visibility.Visible;
                            hasError = true;
                        }
                        else baseSalary = bothSalary;

                        if (!decimal.TryParse(TxtCommissionPercent.Text, out decimal bothComm) || bothComm < 0 || bothComm > 100)
                        {
                            TxtCompensationError.Text = "نسبة العمولة يجب أن تكون بين 0 و 100";
                            TxtCompensationError.Visibility = Visibility.Visible;
                            hasError = true;
                        }
                        else commissionPercent = bothComm;
                        break;

                    case "FullTrip":
                        commissionPercent = 100;
                        break;
                }
            }

            return !hasError;
        }

        private void CbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbDepartment.SelectedValue == null)
            {
                CbRole.ItemsSource = null;
                return;
            }

            int departmentId = (int)CbDepartment.SelectedValue;
            var roles = _getRolesHandler.Handle(departmentId);
            CbRole.ItemsSource = roles;
            CbRole.SelectedIndex = -1;
        }

        private void CbCompensationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PanelBaseSalary == null || PanelCommission == null)
                return;

            var selected = (ComboBoxItem)CbCompensationType.SelectedItem;
            string tag = selected?.Tag?.ToString();

            switch (tag)
            {
                case "Salary":
                    PanelBaseSalary.Visibility = Visibility.Visible;
                    PanelCommission.Visibility = Visibility.Collapsed;
                    break;
                case "Commission":
                    PanelBaseSalary.Visibility = Visibility.Collapsed;
                    PanelCommission.Visibility = Visibility.Visible;
                    break;
                case "Both":
                    PanelBaseSalary.Visibility = Visibility.Visible;
                    PanelCommission.Visibility = Visibility.Visible;
                    break;
                case "FullTrip":
                    PanelBaseSalary.Visibility = Visibility.Collapsed;
                    PanelCommission.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void BtnAddPhoneToList_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtNewPhone.Text))
            {
                PhonesList.Add(TxtNewPhone.Text.Trim());
                TxtNewPhone.Clear();
            }
        }

        private void BtnRemovePhone_Click(object sender, RoutedEventArgs e)
        {
            var phone = (sender as Button)?.DataContext as string;
            if (phone != null) PhonesList.Remove(phone);
        }

        private void BtnAddAttachment_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png|All Files|*.*",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                string tempFolder = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Attachments", "Employees", "Temp");
                Directory.CreateDirectory(tempFolder);

                foreach (var file in dialog.FileNames)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file)}";
                    string destPath = Path.Combine(tempFolder, fileName);
                    File.Copy(file, destPath, true);

                    AttachmentsList.Add(new AttachmentItem { FilePath = destPath });

                }
                UpdateNoAttachmentsMessage();

            }
        }
        private void UpdateNoAttachmentsMessage()
        {
            TxtNoAttachments.Visibility = AttachmentsList.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnRemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var attachment = btn?.DataContext as AttachmentItem;

            if (attachment != null)
            {
                try
                {
                    if (File.Exists(attachment.FilePath))
                        File.Delete(attachment.FilePath);
                }
                catch { }

                AttachmentsList.Remove(attachment);

                UpdateNoAttachmentsMessage();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}