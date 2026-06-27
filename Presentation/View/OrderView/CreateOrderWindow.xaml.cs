using Application.DTOs.OrderDTOs;
using Application.Features.BrandManagement.Queries;
using Application.Features.CampaignManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment.Commands;
using Application.Features.DeviceManagement.Commands;
using Application.Features.DeviceManagement.Queries;
using Application.Features.OrderManagement.Commands;
using Application.Features.OrderManagement.Queries;
using Application.Features.SourceManagement;
using Application.Features.SpecManagement;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement.Queries;
using Presentation.View.Customer_View;
using Presentation.View.MainView;
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

        private readonly CreateCustomerHandler _createCustomerHandler;

        private readonly GetAllBrandsHandler _getAllBrandsHandler;
        private readonly GetAllTypesHandler _getAllTypesHandler;
        private readonly GetSpecsByTypeIdHandler _getSpecsByTypeIdHandler;

        private readonly AddDeviceToCustomerHandler _addDeviceHandler;
        private readonly GetAllBrandsHandler _getBrandsHandler;
        private readonly GetAllTypesHandler _getTypesHandler;
        private readonly GetSpecsByTypeIdHandler _getSpecsHandler;
        private readonly GetAllSourcesHandler _getAllSources;
        private readonly GetCampaignLookupHandler _getCampaignLookup;
        public CreateOrderWindow(
                    GetCustomersLookupHandler customersLookup,
                    GetCustomerDevicesHandler getCustomerDevices,
                    CreateOrderHandler createOrderHandler,
                    CreateCustomerHandler createCustomerHandler,
                    GetAllBrandsHandler getAllBrandsHandler,
                    GetAllTypesHandler getAllTypesHandler,
                    GetSpecsByTypeIdHandler getSpecsByTypeIdHandler , AddDeviceToCustomerHandler addDeviceHandler,
            GetAllBrandsHandler getBrandsHandler, GetAllTypesHandler getTypesHandler,
            GetSpecsByTypeIdHandler getSpecsHandler  , GetAllSourcesHandler getAllSources , GetCampaignLookupHandler getCampaignLookup)
        {
            InitializeComponent();

            _getCustomerLookupHandler = customersLookup;
            _getCustomerDevicesHandler = getCustomerDevices;
            _createOrderHandler = createOrderHandler;
            _createCustomerHandler = createCustomerHandler;
            _getAllBrandsHandler = getAllBrandsHandler;
            _getAllTypesHandler = getAllTypesHandler;
            _getSpecsByTypeIdHandler = getSpecsByTypeIdHandler;
            _addDeviceHandler = addDeviceHandler;
            _getBrandsHandler = getBrandsHandler;
            _getTypesHandler = getTypesHandler;
            _getSpecsHandler = getSpecsHandler;
            _getAllSources = getAllSources;
            _getCampaignLookup = getCampaignLookup;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = _getCustomerLookupHandler.Handle();
                if (customers == null) return;

                customerID.ItemsSource = customers;
            }
            catch (Exception ex) {

                MessageBox.Show("حدثت مشكله اثناء تحميل العملاء");
                customerID.ItemsSource = null;
            }
        }
        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var win = new CreateCustomerWindow(_createCustomerHandler  , _getAllBrandsHandler , _getAllTypesHandler , _getSpecsByTypeIdHandler ,_getAllSources ,_getCampaignLookup  );
            win.Owner = this;

            if (win.ShowDialog() == true)
            {
                LoadCustomers();

                customerID.SelectedValue = win._customerId;
            }
        }
        private void BtnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            var customerId = (int)customerID.SelectedValue;

            var win = new AddDeviceWindow(customerId, _addDeviceHandler, _getBrandsHandler, _getTypesHandler, _getSpecsHandler);
            win.Owner = this;

            if (win.ShowDialog() == true)
            {
                var result = _getCustomerDevicesHandler.Handle(customerId);
                if (result?.IsSuccess == true)
                {
                    DeviceID.ItemsSource = result.Value;
                    DeviceID.SelectedValue = win._newDeviceId;
                }
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            CustomerError.Visibility  = Visibility.Collapsed;
            DeviceError.Visibility = Visibility.Collapsed;
            IssueError.Visibility = Visibility.Collapsed;

            CustomerError.Text = "";
            DeviceError.Text = "";
            IssueError.Text = "";

            if (customerID.SelectedValue == null)
            {
                CustomerError.Text = "برجاء ادخال بيانات العميل";
                CustomerError.Visibility = Visibility.Visible;
                isValid = false;

            }

            if (DeviceID.SelectedValue == null)
            {
                DeviceError.Text = "برجاء ادخال بيانات الجهاز";
                DeviceError.Visibility = Visibility.Visible;
                isValid = false;

            }

            if (string.IsNullOrWhiteSpace(TxtIssue.Text))
            {
                IssueError.Text = "برجاء تسجل العطل";
                IssueError.Visibility = Visibility.Visible;
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
                    var mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
                    mainWindow?.OpenOrderDetailsAfterCreate(res.Value);


                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(res.Error);
                }
            }
            catch (Exception ex) {

                MessageBox.Show($"حدث خطأ غير متوقع، حاول مرة أخرى{ex}");
            }
        }

        private bool _isUpdatingText = false;

        private void customerID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (customerID.SelectedValue == null)
                {
                    DeviceID.ItemsSource = null;
                    BtnAddDevice.IsEnabled = false;
                    return;
                }
                BtnAddDevice.IsEnabled = true;
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
            catch
            {
                MessageBox.Show("حدثت مشكله اثناء تحميل بيانات الاجهزه");
                DeviceID.ItemsSource = null;
            }
        }

        private void customerID_TextChanged(object sender, RoutedEventArgs e)
        {
            if (_isUpdatingText) return;

            string searchText = customerID.Text;

            if (customerID.SelectedItem != null)
            {
                var selected = customerID.SelectedItem as dynamic;
                if (selected?.Name == searchText) return;
            }

            var view = CollectionViewSource.GetDefaultView(customerID.ItemsSource);
            if (view == null) return;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = obj =>
                {
                    var customer = obj as dynamic;
                    return customer?.Name?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }

            view.Refresh();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (view.Cast<object>().Any())
                    customerID.IsDropDownOpen = true;

                var textBox = customerID.Template?.FindName("PART_EditableTextBox", customerID) as TextBox;
                if (textBox != null)
                {
                    textBox.SelectionStart = searchText.Length;
                    textBox.SelectionLength = 0;
                }
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}