using Application.DTOs.EmployeeDTOs;
using Application.Features.DepartmentManagement;
using Application.Features.EmployeeManagement.Commands;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.EmployeeView
{
    public partial class EditEmployee : Window
    {
        private readonly GetRolesByDepartmentHandler _getRolesHandler;
        private readonly GetDepartmentsLookupHandler _getDepartments;

        private readonly UpdateEmployeeHandler _updateEmployeeHandler;
        private readonly int _employeeId;
        private readonly EmployeeProfileDto _currentEmployee;

        public EditEmployee(
     int employeeId,
     EmployeeProfileDto employee,
     GetRolesByDepartmentHandler getRolesHandler,
     GetDepartmentsLookupHandler getDepartments,
     UpdateEmployeeHandler updateEmployeeHandler)
        {
            InitializeComponent();

            _employeeId = employeeId;
            _getRolesHandler = getRolesHandler;
            _getDepartments = getDepartments;
            _currentEmployee = employee;
            _updateEmployeeHandler = updateEmployeeHandler;

            LoadInitialData();
        }

        private void LoadInitialData()
        {
            
            CbDepartment.ItemsSource = _getDepartments.Handle();

            TxtName.Text = _currentEmployee.Name;
            TxtAddress.Text = _currentEmployee.Address;
            TxtAge.Text = _currentEmployee.Age?.ToString(); 

            CbSex.SelectedIndex = _currentEmployee.Sex;

            CbDepartment.SelectedValue = _currentEmployee.DepartmentId;

            if (_currentEmployee.DepartmentId > 0)
            {
                CbRole.ItemsSource = _getRolesHandler.Handle(_currentEmployee.DepartmentId);
                CbRole.SelectedValue = _currentEmployee.RoleId;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            try
            {
                byte sex = (byte)CbSex.SelectedIndex;

                var dto = new UpdateEmployeeDto
                {
                    EmployeeId = _employeeId,
                    Name = TxtName.Text.Trim(),
                    Age = string.IsNullOrWhiteSpace(TxtAge.Text) ? null : Convert.ToInt32(TxtAge.Text),
                    Sex = sex,
                    Address = TxtAddress.Text?.Trim(),
                    RoleId = Convert.ToInt32(CbRole.SelectedValue),
                    DepartmentId = Convert.ToInt32(CbDepartment.SelectedValue)
                };

                var result = _updateEmployeeHandler.Handle(dto);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تعديل بيانات الموظف بنجاح!", "نجاح العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show(result.Error, "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"عفواً، حدث خطأ أثناء حفظ البيانات.\nالسبب: {ex.Message}", "خطأ في النظام", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            bool hasError = false;

            TxtNameError.Text = "";
            TxtDepartmentError.Text = "";
            TxtRoleError.Text = "";
            TxtSexError.Text = "";
            TxtAgeError.Text = "";

            TxtNameError.Visibility = Visibility.Collapsed;
            TxtDepartmentError.Visibility = Visibility.Collapsed;
            TxtRoleError.Visibility = Visibility.Collapsed;
            TxtSexError.Visibility = Visibility.Collapsed;
            TxtAgeError.Visibility = Visibility.Collapsed;

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

            return !hasError;
        }

        private void CbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbDepartment.SelectedValue == null)
            {
                CbRole.ItemsSource = null;
                return;
            }

            if (int.TryParse(CbDepartment.SelectedValue.ToString(), out int departmentId))
            {
                var roles = _getRolesHandler.Handle(departmentId);
                CbRole.ItemsSource = roles;
                CbRole.SelectedIndex = -1;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}