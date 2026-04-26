using Application.DTOs.OrderDTOs;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using System;
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
                var order = result.Value;
                if (order == null)
                {
                    MessageBox.Show("لم يتم العثور على الطلب");
                    return;
                }
                DataContext = order; 
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
        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is OrderDetailsDto currentOrder)
            {
                if (currentOrder.Problem?.Trim() == EditProblem.Text?.Trim() &&
                    currentOrder.Notes?.Trim() == EditNotes.Text?.Trim())
                {
                    return;
                }

                var updateDto = new UpdateOrderDto(
                    currentOrder.OrderId,    
                    EditProblem.Text,        
                    EditNotes.Text           
                );

                var result = _updateOrderHandler.Handle(updateDto);

                if (result.IsSuccess)
                {
                    var updatedOrder = currentOrder with
                    {
                        Problem = EditProblem.Text,
                        Notes = EditNotes.Text
                    };

                    this.DataContext = updatedOrder;

                    MessageBox.Show("تم تحديث البيانات بنجاح", "تأكيد", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"خطأ: {result.Error}", "خطأ في التحديث", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void StatusBadge_Click(object sender, MouseButtonEventArgs e)
        {
            // بيفتح الـ ContextMenu على الـ Badge مباشرة
            StatusBadge.ContextMenu.PlacementTarget = StatusBadge;
            StatusBadge.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            StatusBadge.ContextMenu.IsOpen = true;
        }

        private void StateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem item) return;
            string newState = item.Tag.ToString();
            if (newState == _selectedState) return;

            _selectedState = newState;
            UpdateStateBadge(newState);

            // حفظ فوري
            SaveState();
        }

        private void UpdateStateBadge(string state)
        {
            TxtStatus.Text = state;

            (string bg, string fg) = state switch
            {
                "قيد الانتظار" => ("#FEF9C3", "#CA8A04"),
                "مجدول" => ("#EFF6FF", "#2563EB"),
                "جارى التنفيذ" => ("#FFF7ED", "#EA580C"),
                "مكتمل" => ("#F0FDF4", "#16A34A"),
                "ملغى" => ("#FEF2F2", "#DC2626"),
                _ => ("#F1F5F9", "#64748B")
            };

            StatusBadge.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(bg));
            TxtStatus.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(fg));
            TxtStatusArrow.Foreground = TxtStatus.Foreground;
        }

        private void SaveState()
        {
            //if (DataContext is not OrderDetailsDto order) return;

            //var dto = new UpdateOrderDto(order.OrderId, order.Problem, order.Notes, _selectedState);
            //var result = _updateOrderHandler.Handle(dto);

            //if (!result.IsSuccess)
            //    MessageBox.Show(result.Error);
        }
        private void TabOrderInfo_Click(object sender, RoutedEventArgs e) => SwitchTab("info");
        private void TabAppointments_Click(object sender, RoutedEventArgs e) => SwitchTab("appointments");
        private void TabVisits_Click(object sender, RoutedEventArgs e) => SwitchTab("visits");
        private void TabInvoice_Click(object sender, RoutedEventArgs e) => SwitchTab("invoice");

        public void BtnBack_Click(object sender, RoutedEventArgs e) => BackRequested?.Invoke(this, EventArgs.Empty);
        private void BtnEditOrder_Click(object sender, RoutedEventArgs e) { }
    }
}