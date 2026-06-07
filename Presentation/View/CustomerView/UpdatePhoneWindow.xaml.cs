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

namespace Presentation.View.CustomerView
{
    public partial class UpdatePhoneWindow : Window
    {
        private readonly UpdatePhoneHandler _updatePhoneHandler;
        private readonly string _oldPhone;

        public UpdatePhoneWindow(UpdatePhoneHandler updatePhoneHandler, string oldPhone)
        {
            InitializeComponent();
            _updatePhoneHandler = updatePhoneHandler;
            _oldPhone = oldPhone;
            TxtPhone.Text = oldPhone;
            TxtPhone.Focus();
            TxtPhone.SelectAll();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPhone.Text))
            {
                TxtPhoneError.Text = "رقم الهاتف مطلوب";
                TxtPhoneError.Visibility = Visibility.Visible;
                return;
            }

            if (TxtPhone.Text.Trim() == _oldPhone)
            {
                DialogResult = false;
                return;
            }

            TxtPhoneError.Visibility = Visibility.Collapsed;

            try
            {
                var updated = _updatePhoneHandler.Handle(TxtPhone.Text.Trim(), _oldPhone);

                if (updated.IsSuccess)
                {
                    MessageBox.Show("تم تعديل الرقم بنجاح", "نجاح",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    TxtPhoneError.Text = updated.Error;
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

