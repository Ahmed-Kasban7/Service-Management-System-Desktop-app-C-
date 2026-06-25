using Application.DTOs.CampaignDTOs;
using Application.Features.CampaignManagement.Command;
using Application.Features.SourceManagement;
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

namespace Presentation.View.CampaignView
{
    /// <summary>
    /// Interaction logic for CreateCampaignWindow.xaml
    /// </summary>
    public partial class CreateCampaignWindow : Window
    {
        private readonly CreateCampaignHandler _createCampaignHandler;
        private readonly GetAllSourcesHandler _getAllSources;
        public CreateCampaignWindow(CreateCampaignHandler createCampaign, GetAllSourcesHandler getAllSources)
        {
            InitializeComponent();
            _createCampaignHandler = createCampaign;
            _getAllSources = getAllSources;
            Load_Sources();
        }

        private void Load_Sources()
        {
            try
            {
                CbSources.ItemsSource = _getAllSources.Handle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تحميل منصات الإعلان: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetErrors();

            bool isValid = true;
            int discount = 0;
            decimal cost = 0;

            if (string.IsNullOrWhiteSpace(TxtCampaignName.Text))
            {
                TxtNameError.Text = "اسم الحملة مطلوب";
                TxtNameError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (CbSources.SelectedValue == null)
            {
                TxtSourcesError.Text = "يجب اختيار منصة الإعلان";
                TxtSourcesError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(TxtCost.Text))
            {
                TxtCostError.Text = "تكلفة الحملة مطلوبة";
                TxtCostError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else if (!decimal.TryParse(TxtCost.Text, out cost) || cost < 0)
            {
                TxtCostError.Text = "برجاء إدخال تكلفة صحيحة";
                TxtCostError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (DpStartDate.SelectedDate == null)
            {
                TxtStartDateError.Text = "تاريخ بداية الحملة مطلوب";
                TxtStartDateError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (DpEndDate.SelectedDate == null)
            {
                TxtEndDateError.Text = "تاريخ نهاية الحملة مطلوب";
                TxtEndDateError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else if (DpStartDate.SelectedDate != null && DpEndDate.SelectedDate < DpStartDate.SelectedDate)
            {
                TxtEndDateError.Text = "تاريخ النهاية لا يمكن أن يكون قبل تاريخ البداية";
                TxtEndDateError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(TxtDiscount.Text))
            {
                if (!int.TryParse(TxtDiscount.Text, out discount) || discount < 0 || discount > 100)
                {
                    TxtDiscountError.Text = "برجاء ادخال نسبة صحيحة بين 0 و 100";
                    TxtDiscountError.Visibility = Visibility.Visible;
                    isValid = false;
                }

            }

            if (!isValid) return;

            var campaignDto = new CreateCampaignDto
            {
                CampaignName = TxtCampaignName.Text.Trim(),
                StartDate = DateOnly.FromDateTime(DpStartDate.SelectedDate.Value),
                EndDate = DateOnly.FromDateTime(DpEndDate.SelectedDate.Value),
                SourceId = Convert.ToInt32(CbSources.SelectedValue),
                CampaignCost = cost,
                Discount = discount,
                Note = string.IsNullOrWhiteSpace(TxtNotes.Text) ? null : TxtNotes.Text.Trim()
            };

            try
            {
                int newCampaignId = _createCampaignHandler.Handle(campaignDto);

                if (newCampaignId > 0)
                {
                    MessageBox.Show("تم إضافة الحملة الدعائية بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("فشل حفظ الحملة في قاعدة البيانات.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء الحفظ: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ResetErrors()
        {
            TxtNameError.Visibility = Visibility.Collapsed;
            TxtSourcesError.Visibility = Visibility.Collapsed;
            TxtCostError.Visibility = Visibility.Collapsed;
            TxtStartDateError.Visibility = Visibility.Collapsed;
            TxtEndDateError.Visibility = Visibility.Collapsed;
            TxtDiscountError.Visibility = Visibility.Collapsed;
        }
    }
}