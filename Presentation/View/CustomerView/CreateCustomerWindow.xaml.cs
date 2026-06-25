using Application.DTOs;
using Application.DTOs.CampaignDTOs;
using Application.DTOs.DeviceDTOs;
using Application.Features.BrandManagement;
using Application.Features.BrandManagement.Queries;
using Application.Features.CampaignManagement.Queries;
using Application.Features.CustomerManagment;
using Application.Features.CustomerManagment.Commands;
using Application.Features.SourceManagement;
using Application.Features.SpecManagement;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement;
using Application.Features.TypeManagement.Queries;
using Domain.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Presentation.View.Customer_View
{
    public partial class CreateCustomerWindow : Window
    {
        public ObservableCollection<string> PhonesList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<DeviceCreateDto> DevicesList { get; set; } = new ObservableCollection<DeviceCreateDto>();
        private readonly CreateCustomerHandler _createCustomerHandler;

        private readonly GetAllBrandsHandler _getAllBrandsHandler;
        private readonly GetAllTypesHandler _getAllTypesHandler;
        private readonly GetSpecsByTypeIdHandler _getSpecsByTypeIdHandler;
        private readonly GetAllSourcesHandler _getAllSourcesHandler;
        private readonly GetCampaignLookupHandler _getCampaignLookup;
        public  int _customerId;
        

        public CreateCustomerWindow(CreateCustomerHandler createCustomer,
            GetAllBrandsHandler getAllBrands , GetAllTypesHandler getAllTypes , 
            GetSpecsByTypeIdHandler getSpecsByTypeId  , GetAllSourcesHandler getAllSources , GetCampaignLookupHandler getCampaignLookup)
        {
            InitializeComponent();

            _createCustomerHandler = createCustomer;
            _getAllBrandsHandler = getAllBrands;
            _getAllTypesHandler = getAllTypes;
            _getSpecsByTypeIdHandler = getSpecsByTypeId;
            _getAllSourcesHandler = getAllSources;
            _getCampaignLookup = getCampaignLookup;
            
            LstPhones.ItemsSource = PhonesList;
            DgTempDevices.ItemsSource = DevicesList;


            CbBrand.ItemsSource = _getAllBrandsHandler.Handle(); 
            CbType.ItemsSource = _getAllTypesHandler.Handle();
            CbSource.ItemsSource = _getAllSourcesHandler.Handle();
        }

        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            try
            {
                var customerDto = new CustomerCreateDto(
                    TxtName.Text.Trim(),
                    TxtAddress.Text.Trim(),
                    string.IsNullOrWhiteSpace(TxtDiscount.Text) ? 0 :Convert.ToInt32( TxtDiscount.Text),
                    string.IsNullOrWhiteSpace(TxtAge.Text) ? null : Convert.ToInt32(TxtAge.Text),
                    Convert.ToInt32(CbSource.SelectedValue),
                    CbCampaign.SelectedValue != null ? Convert.ToInt32(CbCampaign.SelectedValue) : null ,
                    CbSex.SelectedIndex == 0 ? ESex.MALE : ESex.FEMALE,
                    DevicesList.ToList(),
                    PhonesList.ToList());

                var res = _createCustomerHandler.Handle(customerDto);

                MessageBox.Show(res.IsSuccess
                    ? "تم حفظ بيانات العميل بنجاح"
                    : res.Error);

                if (res.IsSuccess)
                {
                    _customerId = res.Value;
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء الحفظ برجاء إعادة المحاولة" );
            }
        }
        private bool Validate()
        {
            bool hasError = false;

            TxtNameError.Text = "";
            TxtAgeError.Text = "";
            TxtDiscountError.Text = "";
            TxtAddressError.Text = "";
            txtPhoneError.Text = "";
            txtDeviceError.Text = "";
            TxtSourceError.Text = "";
            TxtCampaignError.Text = "";

            TxtNameError.Visibility = Visibility.Collapsed;
            TxtAgeError.Visibility = Visibility.Collapsed;
            TxtDiscountError.Visibility = Visibility.Collapsed;
            TxtAddressError.Visibility = Visibility.Collapsed;
            txtPhoneError.Visibility = Visibility.Collapsed;
            txtDeviceError.Visibility = Visibility.Collapsed;
            TxtSourceError.Visibility = Visibility.Collapsed;
            TxtCampaignError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                TxtNameError.Text = "برجاء إدخال اسم العميل";
                TxtNameError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!string.IsNullOrWhiteSpace(TxtAge.Text))
            {
                if (!int.TryParse(TxtAge.Text, out int age) || age <= 0)
                {
                    TxtAgeError.Text = "العمر غير صالح";
                    TxtAgeError.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(TxtDiscount.Text))
            {

                if (!int.TryParse(TxtDiscount.Text, out int discount) || discount < 0 || discount > 100)
                {
                    TxtDiscountError.Text = "الخصم يجب أن يكون من 0 إلى 100";
                    TxtDiscountError.Visibility = Visibility.Visible;

                    hasError = true;
                }
            }
        
            if (string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                TxtAddressError.Text = "برجاء إدخال العنوان";
                TxtAddressError.Visibility = Visibility.Visible;

                hasError = true;
            }

            if (PhonesList == null || PhonesList.Count == 0)
            {
                txtPhoneError.Text = "يجب إضافة هاتف واحد على الاقل";
                txtPhoneError.Visibility = Visibility.Visible;

                hasError = true;
            }

            if (DevicesList == null || DevicesList.Count == 0)
            {
               txtDeviceError.Text = "يجب إضافة جهاز واحد على الاقل";
                txtDeviceError.Visibility = Visibility.Visible;


                hasError = true;
            }

            if (CbSource.SelectedValue == null)
            {
                TxtSourceError.Text = "يجب اختيار مصدر العميل";
                TxtSourceError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (CbSource.SelectedValue != null && PanelCampaign.Visibility == Visibility.Visible)
            {
                if (CbCampaign.SelectedValue == null)
                {
                    TxtCampaignError.Text = "يجب اختيار حملة";
                    TxtCampaignError.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            return !hasError;
        }
        private void BtnAddPhoneToList_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtNewPhone.Text))
            {
                PhonesList.Add(TxtNewPhone.Text.Trim());
                TxtNewPhone.Clear();
            }
        }
        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbType.SelectedValue != null)
            {
                int selectedTypeId = (int)CbType.SelectedValue;


                var specs = _getSpecsByTypeIdHandler.Handle(selectedTypeId);

                CbSpec.ItemsSource = specs;

                CbSpec.SelectedIndex = -1;
            }
            else
            {
                CbSpec.ItemsSource = null;
            }
        }

        private void BtnRemovePhone_Click(object sender, RoutedEventArgs e)
        {
            var phone = (sender as System.Windows.Controls.Button)?.DataContext as string;
            if (phone != null) PhonesList.Remove(phone);
        }


        private void BtnAddDeviceToList_Click(object sender, RoutedEventArgs e)
        {
            if (CbType.SelectedItem is not TypeDto selectedType ||
                CbBrand.SelectedItem is not BrandDto selectedBrand ||
                CbSpec.SelectedItem is not SpecDto selectedSpec)
            {
                return;
            }

            DevicesList.Add(new DeviceCreateDto(selectedBrand.BrandID, selectedBrand.BrandName,
                selectedType.TypeID, selectedType.TypeName, selectedSpec.SpecID, selectedSpec.SpecName, TxtDeviceModel.Text, TxtSerial.Text));

            TxtDeviceModel.Clear();
            TxtSerial.Clear();

            CbType.SelectedIndex = -1;
            CbBrand.SelectedIndex = -1;
            CbSpec.SelectedIndex = -1;
        }

        
        private void BtnDeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;

            var deviceDto = button?.DataContext as DeviceCreateDto;

            if (deviceDto != null)
            {
                DevicesList.Remove(deviceDto);
            }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CbSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbSource.SelectedValue != null)
                {
                    int selectedSourceId = Convert.ToInt32(CbSource.SelectedValue);

                    var campaigns = _getCampaignLookup.Handle(selectedSourceId);

                    if (campaigns == null || !campaigns.Any())
                    {
                        CbCampaign.ItemsSource = null; 
                        PanelCampaign.Visibility = Visibility.Collapsed; 
                    }
                    else
                    {
                        CbCampaign.ItemsSource = campaigns;
                        CbCampaign.SelectedIndex = -1;
                        PanelCampaign.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    CbCampaign.ItemsSource = null;
                    PanelCampaign.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تحميل الحملات الإعلانية للمنصة: {ex.Message}", "خطأ تصفح", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbCampaign.SelectedItem is CampaignLookup selectedCampaign)
            {
                if (selectedCampaign.Discount > 0)
                {
                    TxtDiscount.Text = selectedCampaign.Discount.ToString();
                }
            }
            else
            {
                TxtDiscount.Text = "0";
            }
        }
    }
}