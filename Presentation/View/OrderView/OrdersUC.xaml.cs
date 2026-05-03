using Application.Common;
using Application.DTOs.OrderDTOs;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation.View.OrderView
{
    public partial class OrdersUC : UserControl
    {
        private int CurrentPage = 1;
        private const int ROWPERPAGE = 8;

        private GetPagedOrderSummariesHandler _getPagedOrderSummariesHandler;
        private GetOrderFullDetailsHandler _getOrderFullDetailsHandler;
        private UpdateOrderHandler _updateOrderHandler;
        private SearchOrderPageHandler _searchOrderPageHandler;

        private bool IsSearching = false;
        private string CurrentSearchText = "";

        public OrdersUC()
        {
            InitializeComponent();
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage("ar-EG");
        }

        public void InitializeServices(
            GetPagedOrderSummariesHandler getPagedOrderSummaries,
            GetOrderFullDetailsHandler getOrderFull,
            UpdateOrderHandler updateOrder,
            SearchOrderPageHandler searchOrder)
        {
            _getPagedOrderSummariesHandler = getPagedOrderSummaries;
            _getOrderFullDetailsHandler = getOrderFull;
            _updateOrderHandler = updateOrder;
            _searchOrderPageHandler = searchOrder;

            LoadAndBindOrders();
        }

  
        private PagedResult<OrderSummaryDto> LoadOrders()
        {
            try
            {
                var res = _getPagedOrderSummariesHandler.Handle(CurrentPage, ROWPERPAGE);

                if (res.IsSuccess)
                    return res.Value;

                MessageBox.Show(res.Error);
                return GetEmptyResult();
            }
            catch
            {
                MessageBox.Show("خطأ في تحميل الطلبات");
                return GetEmptyResult();
            }
        }

      
        public void LoadAndBindOrders()
        {
            PagedResult<OrderSummaryDto> result;

            if (IsSearching && !string.IsNullOrWhiteSpace(CurrentSearchText))
            {
                var res = _searchOrderPageHandler.Handle(
                    CurrentSearchText,
                    CurrentPage,
                    ROWPERPAGE);

                result = res;
            }
            else
            {
                result = LoadOrders();
            }

            Bind(result);
        }

  
        private void Bind(PagedResult<OrderSummaryDto> result)
        {
            if (result == null) return;

            DgOrders.ItemsSource = result.Items;
            TxtPageInfo.Text = CurrentPage.ToString();
            TxtOrderCountNumber.Text = result.TotalCount.ToString();

            BtnNextPage.IsEnabled = result.HasNextPage;
            BtnPrevPage.IsEnabled = result.HasPreviousPage;
        }

     
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrentPage = 1;
                IsSearching = true;
                CurrentSearchText = SearchBox.Text;

                LoadAndBindOrders();
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (!BtnNextPage.IsEnabled) return;

            CurrentPage++;
            LoadAndBindOrders();
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage <= 1) return;

            CurrentPage--;
            LoadAndBindOrders();
        }


        private void ResetSearch()
        {
            IsSearching = false;
            CurrentSearchText = "";
            CurrentPage = 1;
        }

        private PagedResult<OrderSummaryDto> GetEmptyResult()
        {
            return new PagedResult<OrderSummaryDto>(
                new System.Collections.Generic.List<OrderSummaryDto>(),
                0,
                1,
                ROWPERPAGE);
        }

  
        private void DgOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgOrders.SelectedItem is OrderSummaryDto selectedOrder)
            {
                OrdersListPanel.Visibility = Visibility.Collapsed;

                var detailsUC = new OrderDetailsUC(
                    _getOrderFullDetailsHandler,
                    _updateOrderHandler,
                    selectedOrder.OrderId);

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

        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}