using Application.Common;
using Application.Features.AppointmentManagement.Commands;
using Domain.Entities;
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

namespace Presentation.View.OrderView
{
    /// <summary>
    /// Interaction logic for CancelAppoinmentReasonWindow.xaml
    /// </summary>
    public partial class CancelAppoinmentReasonWindow : Window
    {
        private string _cancellationReason { get; set; }
        private readonly int _appointmentId;
        private readonly CancelAppointmentHandler _cancelAppointmentHandler;

        public CancelAppoinmentReasonWindow(int appointmentId , CancelAppointmentHandler cancelAppointmentHandler)
        {
            _appointmentId = appointmentId;
            _cancelAppointmentHandler = cancelAppointmentHandler;
            InitializeComponent();
        }
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            TxtCancelReasonError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(TxtCancelReason.Text))
            {
                TxtCancelReasonError.Text = "برجاء كتابة سبب الإلغاء.";
                TxtCancelReasonError.Visibility = Visibility.Visible;
                return;
            }

            _cancellationReason = TxtCancelReason.Text.Trim();

            var confirm = MessageBox.Show(
    "هل أنت متأكد من إلغاء هذا الموعد؟",
    "تأكيد الإلغاء",
    MessageBoxButton.YesNo,
    MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {

                var result = _cancelAppointmentHandler.Handle(_appointmentId, _cancellationReason);

                if (result.IsSuccess)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Error);
                }

            }
            catch (Exception ex )
            {
                MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}");

            }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
