using Application.DTOs.OrderDTOs;
using Application.Features.AppointmentManagement.Commands;
using Application.Features.AppointmentManagement.Queries;
using Application.Features.EmployeeManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Application.Features.PhoneManagement.Commands;
using Domain.Entities;
using Presentation.View.Customer_View;
using System;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Presentation.View.OrderView
{
    public partial class OrderDetailsUC : UserControl
    {
        public event EventHandler BackRequested;
        private string _selectedState;
        

        private readonly GetOrderFullDetailsHandler _getOrderFullDetailsHandler;
        private readonly UpdateOrderHandler _updateOrderHandler;
        private  OrderDetailsDto _currentOrder;
        private GetEmployeesLookupHandler _getEmployeesLookup;
        private int _orderId;
        private readonly CreateAppointmentHandler _createAppointmentHandler;
        private readonly GetAppointmentsByOrderIdHandler _getAppointments;
        private readonly UpdateAppointmentHandler _updateAppointmentHandler;
        private readonly GetAppointmentByIdHandler _getAppointmentByIdHandler;
        private readonly CancelAppointmentHandler _cancelAppointmentHandler;

        public OrderDetailsUC(GetOrderFullDetailsHandler getOrderFull, UpdateOrderHandler updateOrder 
            ,GetEmployeesLookupHandler getEmployees, int orderId , CreateAppointmentHandler createAppointment , 
            GetAppointmentsByOrderIdHandler getAppointments , UpdateAppointmentHandler updateAppointment , GetAppointmentByIdHandler getAppointment , CancelAppointmentHandler cancelAppointment)
        {
            InitializeComponent();
            _getOrderFullDetailsHandler = getOrderFull;
            _updateOrderHandler = updateOrder;
            _getEmployeesLookup = getEmployees;
            _orderId = orderId;
            _createAppointmentHandler = createAppointment;
            _getAppointments = getAppointments;
            _updateAppointmentHandler = updateAppointment;
            _getAppointmentByIdHandler = getAppointment;
            _cancelAppointmentHandler = cancelAppointment;

            LoadOrder();
            
        }

        public void LoadOrder()
        {
            var result = _getOrderFullDetailsHandler.Handle(_orderId);

            if (result.IsSuccess)
            {
                 _currentOrder = result.Value;
                if (_currentOrder == null)
                {
                    MessageBox.Show("لم يتم العثور على الطلب");
                    return;
                }
                DataContext = _currentOrder; 
            }
            else
            {
                MessageBox.Show(result.Error);
            }
        }
        private void LoadAppointments()
        {
            try
            {
                var result = _getAppointments.Handle(_orderId);

                if (result.IsSuccess && result.Value.Any())
                {
                    AppointmentsGrid.ItemsSource = result.Value;
                    AppointmentsGrid.Visibility = Visibility.Visible;
                    AppointmentsEmptyState.Visibility = Visibility.Collapsed;
                }
                else
                {
                    AppointmentsGrid.Visibility = Visibility.Collapsed;
                    AppointmentsEmptyState.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                MessageBox.Show("حدثت مشكلة أثناء تحميل المواعيد");
            }
        }
        private void SwitchTab(string active)
        {
            ScrollOrderInfo.Visibility = Visibility.Collapsed;  
            PanelAppointments.Visibility = Visibility.Collapsed;
            PanelVisits.Visibility = Visibility.Collapsed;
            PanelInvoice.Visibility = Visibility.Collapsed;

            foreach (var btn in new[] { TabOrderInfo, TabAppointments, TabVisits, TabInvoice })
            {
                btn.BorderBrush = Brushes.Transparent;
                btn.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#64748B"));
                btn.FontWeight = FontWeights.Normal;
            }

            var blue = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#2563EB"));

            switch (active)
            {
                case "info":
                    ScrollOrderInfo.Visibility = Visibility.Visible;  
                    TabOrderInfo.BorderBrush = blue;
                    TabOrderInfo.Foreground = blue;
                    TabOrderInfo.FontWeight = FontWeights.SemiBold;
                    break;
                case "appointments":
                    PanelAppointments.Visibility = Visibility.Visible;
                    TabAppointments.BorderBrush = blue;
                    TabAppointments.Foreground = blue;
                    TabAppointments.FontWeight = FontWeights.SemiBold;
                    break;
                case "visits":
                    PanelVisits.Visibility = Visibility.Visible;
                    TabVisits.BorderBrush = blue;
                    TabVisits.Foreground = blue;
                    TabVisits.FontWeight = FontWeights.SemiBold;
                    break;
                case "invoice":
                    PanelInvoice.Visibility = Visibility.Visible;
                    TabInvoice.BorderBrush = blue;
                    TabInvoice.Foreground = blue;
                    TabInvoice.FontWeight = FontWeights.SemiBold;
                    break;
            }
        }


        private void TabOrderInfo_Click(object sender, RoutedEventArgs e) => SwitchTab("info");
        private void TabAppointments_Click(object sender, RoutedEventArgs e)
        {
            SwitchTab("appointments");
            LoadAppointments();
        }

        private void TabVisits_Click(object sender, RoutedEventArgs e) => SwitchTab("visits");
        private void TabInvoice_Click(object sender, RoutedEventArgs e) => SwitchTab("invoice");

        public void BtnBack_Click(object sender, RoutedEventArgs e) => BackRequested?.Invoke(this, EventArgs.Empty);

        private void BtnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            var addAppointmentWindow = new AddAppointmentWindow(_orderId, _getEmployeesLookup , _createAppointmentHandler)
            {
                Owner = Window.GetWindow(this)
            };

            if (addAppointmentWindow.ShowDialog() == true)
            {
                LoadAppointments();
                LoadOrder();
            }
        }


        private void BtnEditAppointment_Click(object sender, RoutedEventArgs e)
        {
            var appointmentId = (int)((Button)sender).Tag;
            var updateAppointmentWindow = new UpdateAppointmentWindow(appointmentId , _getEmployeesLookup ,_getAppointmentByIdHandler , _updateAppointmentHandler )
            {
                Owner = Window.GetWindow(this)
            };

            if (updateAppointmentWindow.ShowDialog() == true)
            {
                LoadAppointments();
                LoadOrder();
            }
        }

        private void BtnCancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int appointmentId)
            {
                var reasonWindow = new CancelAppoinmentReasonWindow(appointmentId, _cancelAppointmentHandler);
                reasonWindow.Owner = Window.GetWindow(this); 

                if (reasonWindow.ShowDialog() == true)
                {
                    LoadAppointments(); 
                    LoadOrder();        
                }
            }
        }

        private void BtnRecordVisit_Click(object sender, RoutedEventArgs e)
        {
            var recordVisitWindow = new RecordVisitWindow()
            {
                Owner = Window.GetWindow(this)
            };

            if (recordVisitWindow.ShowDialog() == true)
            {
                
            }
        }
    }
}