using Application.Features.PhoneManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.View.EmployeeView
{
    /// <summary>
    /// Interaction logic for AddPhoneWindow.xaml
    /// </summary>
    public partial class AddPhoneToEmployeeWindow : Window
    {
        public string PhoneNumber { get; private set; }
        private readonly AddPhoneToEmployee _addPhoneToEmployee;
        private readonly int _employeeId;

        public AddPhoneToEmployeeWindow(AddPhoneToEmployee addPhoneToEmployee, int employeeId)
        {
            InitializeComponent();
            _addPhoneToEmployee = addPhoneToEmployee;
            _employeeId = employeeId;
            TxtPhone.Focus();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPhone.Text))
            {
                TxtPhoneError.Text = "رقم الهاتف مطلوب";
                TxtPhoneError.Visibility = Visibility.Visible;
                return;
            }
            TxtPhoneError.Visibility = Visibility.Collapsed;
            try
            {
                var added = _addPhoneToEmployee.Handle(TxtPhone.Text.Trim(), _employeeId);
                if (added.IsSuccess)
                {
                    MessageBox.Show("تمت إضافة الرقم بنجاح", "نجاح",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    TxtPhoneError.Text = added.Error;
                    TxtPhoneError.Visibility = Visibility.Visible;
                }
            }
            catch (ArgumentException ex)
            {
                TxtPhoneError.Text = ex.Message;
                TxtPhoneError.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
