using Application.DTOs.AppointmentDTOs;
using Application.DTOs.PersonDTOs;
using Application.Features.AppointmentManagement.Commands;
using Application.Features.CustomerManagement.Queries;
using Application.Features.EmployeeManagement.Queries;
using Domain.Entities;
using Domain.Enums;
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
    /// Interaction logic for AddAppointmentWindow.xaml
    /// </summary>
    public partial class AddAppointmentWindow : Window
    {
        private readonly GetEmployeesLookupHandler _employeesLookupHandler;
        private readonly CreateAppointmentHandler _createAppointmentHandler;
        private readonly int _orderId;
        public AddAppointmentWindow ( int orderId,GetEmployeesLookupHandler getEmployees , CreateAppointmentHandler createAppointment)
        {
            InitializeComponent();
            _employeesLookupHandler = getEmployees;
            _createAppointmentHandler = createAppointment;
            _orderId = orderId;


            LoadTechnicians();
            LoadAssistants();
            LoadDrivers();
        }

        private void LoadTechnicians()
        {
            try
            {
                var technicians = _employeesLookupHandler.Handle("فنى")?.ToList() ?? new List<PersonLookupDto>();

                technicians.Insert(0, new PersonLookupDto (  -1, "-- اختر الفني --" ));

                CbTechnician.ItemsSource = technicians;
                CbTechnician.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل الفنيين", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                CbTechnician.ItemsSource = null;
            }
        }

        private void LoadAssistants()
        {
            try
            {
                var assistants = _employeesLookupHandler.Handle("مساعد فنى")?.ToList() ?? new List<PersonLookupDto>();

                assistants.Insert(0, new PersonLookupDto (-1, "-- بدون مساعد --" ));

                CbAssistant.ItemsSource = assistants;
                CbAssistant.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل المساعدين", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                CbAssistant.ItemsSource = null;
            }
        }
        private void LoadDrivers()
        {
            try
            {
                var drivers = _employeesLookupHandler.Handle("سائق")?.ToList() ?? new List<PersonLookupDto>();

                drivers.Insert(0, new PersonLookupDto (  -1,  "-- بدون سائق --" ));

                CbDriver.ItemsSource = drivers;
                CbDriver.SelectedValue = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل السائقين", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                CbDriver.ItemsSource = null;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            TxtTechnicianError.Visibility = Visibility.Collapsed;
            TxtVisitTypeError.Visibility = Visibility.Collapsed;
            TxtDateError.Visibility = Visibility.Collapsed;

            bool isValid = true;

            if (CbTechnician.SelectedValue == null || (int)CbTechnician.SelectedValue == -1)
            {
                TxtTechnicianError.Text = "برجاء اختيار الفني";
                TxtTechnicianError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (CbVisitType.SelectedItem is ComboBoxItem selectedVisit && selectedVisit.Tag?.ToString() == "-1")
            {
                TxtVisitTypeError.Text = "برجاء اختيار نوع الزيارة .";
                TxtVisitTypeError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else if (CbVisitType.SelectedItem == null)
            {
                TxtVisitTypeError.Text = "برجاء اختيار نوع الزيارة.";
                TxtVisitTypeError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (DpScheduledDate.SelectedDate == null || DpScheduledDate.SelectedDate < DateTime.Today)
            {
                TxtDateError.Text = "برجاء اختيار تاريخ صحيح";
                TxtDateError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (!isValid) return;

            try
            {
                var selectedVisitType = (ComboBoxItem)CbVisitType.SelectedItem;

                string tagValue = selectedVisitType.Tag?.ToString() ?? "0";
                int visitTypeTag = int.Parse(tagValue);

                int? assistantId = (int?)CbAssistant.SelectedValue == -1 ? null : (int?)CbAssistant.SelectedValue;
                int? driverId = (int?)CbDriver.SelectedValue == -1 ? null : (int?)CbDriver.SelectedValue;

                var dto = new AddAppointmentDto
                (
                    _orderId,
                    (int)CbTechnician.SelectedValue,
                    assistantId,
                    driverId,
                    DpScheduledDate.SelectedDate!.Value, 
                    TxtNotes.Text.Trim(),
                    (EVisitType)visitTypeTag
                );

                var result = _createAppointmentHandler.Handle(dto);

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
            catch (Exception ex)
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
