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

namespace Presentation.View.Customer_View
{
    /// <summary>
    /// Interaction logic for AddPhoneWindow.xaml
    /// </summary>
    public partial class AddPhoneWindow : Window
    {
        public string PhoneNumber { get; private set; }
        private readonly AddPhoneToCustomer _addPhoneToCustomer;
        private readonly int _customerId;

        public AddPhoneWindow(AddPhoneToCustomer addPhoneToCustomer, int customerId)
        {
            InitializeComponent();
            _addPhoneToCustomer = addPhoneToCustomer;
            _customerId = customerId;
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
                var added = _addPhoneToCustomer.Handle(TxtPhone.Text.Trim(), _customerId);
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
