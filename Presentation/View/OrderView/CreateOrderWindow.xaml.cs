using Application.DTOs.OrderDTOs;
using Application.Features.CustomerManagement.Queries;
using Application.Features.DeviceManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.SpecManagement;
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
    /// Interaction logic for CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        private readonly GetCustomersLookupHandler _getCustomerLookupHandler;
        private readonly GetCustomerDevicesHandler _getCustomerDevicesHandler;
        private readonly CreateOrderHandler _createOrderHandler;
        public CreateOrderWindow(GetCustomersLookupHandler customersLookup, GetCustomerDevicesHandler getCustomerDevices , CreateOrderHandler createOrderHandler)
        {
            InitializeComponent();
            _getCustomerLookupHandler = customersLookup;
            _getCustomerDevicesHandler = getCustomerDevices;
            _createOrderHandler = createOrderHandler;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = _getCustomerLookupHandler.Handle();
            if (customers == null) return;

            customerID.ItemsSource = customers;
            var view = CollectionViewSource.GetDefaultView(customerID.ItemsSource);

            customerID.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent,
                new TextChangedEventHandler((s, e) =>
                {
                    string searchText = customerID.Text;

                    if (customerID.SelectedItem != null && ((dynamic)customerID.SelectedItem).Name == searchText)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(searchText))
                    {
                        view.Filter = null;
                    }
                    else
                    {
                        view.Filter = obj =>
                        {
                            var customer = obj as dynamic;
                            return customer.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        };

                        view.Refresh();

                        System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (view.Cast<object>().Any())
                            {
                                customerID.IsDropDownOpen = true;

                                var textBox = e.OriginalSource as TextBox;
                                if (textBox != null)
                                {
                                    textBox.SelectionStart = searchText.Length;
                                    textBox.SelectionLength = 0;
                                }
                            }
                        }), System.Windows.Threading.DispatcherPriority.Input); 
                    }
                }));
        }

        private void BtnCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            // Reset errors
            CustomerError.Text = "";
            DeviceError.Text = "";
            IssueError.Text = "";

            if (customerID.SelectedValue == null)
            {
                CustomerError.Text = "برجاء ادخال بيانات العميل";
                isValid = false;

            }

            if (DeviceID.SelectedValue == null)
            {
                DeviceError.Text = "برجاء ادخال بيانات الجهاز";
                isValid = false;

            }

            if (string.IsNullOrWhiteSpace(TxtIssue.Text))
            {
                IssueError.Text = "برجاء تسجل العطل";
                isValid = false;

            }

            if (!isValid)
                return;

            try
            {
                int customerId = Convert.ToInt32(customerID.SelectedValue);
                int deviceId = Convert.ToInt32(DeviceID.SelectedValue);

                var order = new CreateOrderDto(
                            TxtIssue.Text,
                            TxtNotes.Text,
                            customerId,
                            deviceId
                        );
                var res =_createOrderHandler.Handle(order);

                if (res.IsSuccess)
                {
                    MessageBox.Show("تم إنشاء الطلب بنجاح");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(res.Error);
                }
            }
            catch (Exception ex) {

                MessageBox.Show("حدث خطأ غير متوقع، حاول مرة أخرى");
            }
        }

        private void customerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (customerID.SelectedValue == null)
                {
                    DeviceID.ItemsSource = null;
                    return;
                }

                int customerId = Convert.ToInt32(customerID.SelectedValue);

                var result = _getCustomerDevicesHandler.Handle(customerId);

                if (result?.IsSuccess == true)
                {
                    DeviceID.ItemsSource = result.Value;
                    DeviceID.SelectedIndex = -1;
                }
                else
                {
                    DeviceID.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدثت مشكله اثناء تحميل بيانات الاجهزه");
                DeviceID.ItemsSource = null;
            }
        }

        
    }
}