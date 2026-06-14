using Application.DTOs.AppointmentDTOs;
using Application.DTOs.PersonDTOs;
using Application.Features.AppointmentManagement.Commands;
using Application.Features.AppointmentManagement.Queries;
using Application.Features.EmployeeManagement.Queries;
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
    /// Interaction logic for UpdateAppointmentWindow.xaml
    /// </summary>
    public partial class UpdateAppointmentWindow : Window
    {
        private readonly int _appointmentId;
        private readonly GetEmployeesLookupHandler _employeesLookupHandler;
        private readonly GetAppointmentByIdHandler _getAppointment;
        private readonly UpdateAppointmentHandler _updateAppointment;

        public UpdateAppointmentWindow(
            int appointmentId,
            GetEmployeesLookupHandler getEmployees,
            GetAppointmentByIdHandler getAppointment,
            UpdateAppointmentHandler updateAppointment)
        {
            InitializeComponent();
            _appointmentId = appointmentId;
            _employeesLookupHandler = getEmployees;
            _getAppointment = getAppointment;
            _updateAppointment = updateAppointment;

            LoadTechnicians();
            LoadAssistants();
            LoadDrivers();

            LoadAppointmentData();
        }

        private void LoadTechnicians()
        {
            try
            {
                var technicians = _employeesLookupHandler.Handle("فنى")?.ToList() ?? new List<PersonLookupDto>();

                technicians.Insert(0, new PersonLookupDto(-1, "-- اختر الفني --"));

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

                assistants.Insert(0, new PersonLookupDto(-1, "-- بدون مساعد --"));

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

                drivers.Insert(0, new PersonLookupDto(-1, "-- بدون سائق --"));

                CbDriver.ItemsSource = drivers;
                CbDriver.SelectedValue = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل السائقين", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                CbDriver.ItemsSource = null;
            }
        }

        private void LoadAppointmentData()
        {
            var result = _getAppointment.Handle(_appointmentId);
            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error, "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var a = result.Value;

            CbTechnician.SelectedValue = a.TechnicianId;

            CbAssistant.SelectedValue = a.TechnicianAssistantId ?? -1;
            CbDriver.SelectedValue = a.DriverId ?? -1;

            foreach (ComboBoxItem item in CbVisitType.Items)
                if (int.Parse(item.Tag.ToString()) == a.VisitType)
                { CbVisitType.SelectedItem = item; break; }

            DpScheduledDate.SelectedDate = a.ScheduledDate;

            TxtNotes.Text = a.Notes;
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

                var dto = new UpdateAppointmentDto
                {
                    AppointmentId = _appointmentId,
                    TechnicianId = (int)CbTechnician.SelectedValue,
                    TechnicianAssistantId = assistantId,
                    DriverId = driverId,
                    ScheduledDate = DpScheduledDate.SelectedDate.Value,
                    VisitType = (byte)int.Parse(selectedVisitType.Tag.ToString()),
                    Notes = TxtNotes.Text.Trim()
                };

                var result = _updateAppointment.Handle(dto);

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
