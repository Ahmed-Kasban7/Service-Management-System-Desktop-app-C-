using Application.DTOs.OrderDTOs;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
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
        public OrderDetailsUC(GetOrderFullDetailsHandler getOrderFull, UpdateOrderHandler updateOrder, int orderId)
        {
            InitializeComponent();
            _getOrderFullDetailsHandler = getOrderFull;
            _updateOrderHandler = updateOrder;
            LoadOrder(orderId);
        }

        public void LoadOrder(int orderId)
        {
            var result = _getOrderFullDetailsHandler.Handle(orderId);

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

        private void SwitchTab(string active)
        {
            PanelOrderInfo.Visibility = Visibility.Collapsed;
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
                    PanelOrderInfo.Visibility = Visibility.Visible;
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
        private void TabAppointments_Click(object sender, RoutedEventArgs e) => SwitchTab("appointments");
        private void TabVisits_Click(object sender, RoutedEventArgs e) => SwitchTab("visits");
        private void TabInvoice_Click(object sender, RoutedEventArgs e) => SwitchTab("invoice");

        public void BtnBack_Click(object sender, RoutedEventArgs e) => BackRequested?.Invoke(this, EventArgs.Empty);

        private void BtnEditOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}