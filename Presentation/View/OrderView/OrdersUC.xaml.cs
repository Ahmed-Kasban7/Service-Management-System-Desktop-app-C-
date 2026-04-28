using Application.Common;
using Application.DTOs.OrderDTOs;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Infrastructure.Repositories;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation.View.OrderView
{
    /// <summary>
    /// Interaction logic for OrderListWindow.xaml
    /// </summary>
    public partial class OrdersUC : UserControl
    {
        private int CurrentPage = 1;
        private const int ROWPERPAGE = 8;

        private  GetPagedOrderSummariesHandler  _getPagedOrderSummariesHandler;
        private  GetOrderFullDetailsHandler _getOrderFullDetailsHandler;
        private UpdateOrderHandler _updateOrderHandler;
        public OrdersUC()
        {
            InitializeComponent();
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage("ar-EG");
        }
        public void InitializeServices(GetPagedOrderSummariesHandler getPagedOrderSummaries , GetOrderFullDetailsHandler getOrderFull  , UpdateOrderHandler updateOrder)
        {
            _getPagedOrderSummariesHandler = getPagedOrderSummaries;
            _getOrderFullDetailsHandler = getOrderFull;
            _updateOrderHandler = updateOrder;
            
            LoadAndBindOrders();
        }

        private PagedResult<OrderSummaryDto> LoadOrders()
        {

            try
            {
                var res = _getPagedOrderSummariesHandler.Handle(CurrentPage, ROWPERPAGE);

                if (res.IsSuccess)
                {
                    return res.Value;
                }
                else
                {
                    MessageBox.Show(res.Error);
                    return null;

                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("خطاء فى تحميل الطلبات" );
                 return null;
            }
        }
        public void LoadAndBindOrders()
        {
            var result = LoadOrders();
            if (result != null)
            {
                DgOrders.ItemsSource = result.Items;
                TxtPageInfo.Text = CurrentPage.ToString();
                TxtOrderCountNumber.Text = result.TotalCount.ToString();


                BtnNextPage.IsEnabled = result.HasNextPage;
                BtnPrevPage.IsEnabled = result.HasPreviousPage; 
            }
        }
        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            LoadAndBindOrders();
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadAndBindOrders();
            }
        }
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void DgOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgOrders.SelectedItem is OrderSummaryDto selectedOrder)
            {
                OrdersListPanel.Visibility = Visibility.Collapsed;

                var detailsUC = new OrderDetailsUC(_getOrderFullDetailsHandler,_updateOrderHandler, selectedOrder.OrderId);

                detailsUC.BackRequested += (s, args) =>
                {
                    OrderDetailsHolder.Visibility = Visibility.Collapsed;
                    OrderDetailsHolder.Content = null; 

                    OrdersListPanel.Visibility = Visibility.Visible;

                    LoadAndBindOrders();
                };
                OrderDetailsHolder.Content = detailsUC;
                OrderDetailsHolder.Visibility = Visibility.Visible;
            }
        }
        public void RefreshIfVisible()
        {
            if (this.Visibility == Visibility.Visible)
            {
                LoadAndBindOrders();
            }
        }
    }

}
