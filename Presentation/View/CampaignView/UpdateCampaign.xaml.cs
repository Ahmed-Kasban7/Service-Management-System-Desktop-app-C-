using Application.DTOs.CampaignDTOs;
using Application.Features.CampaignManagement.Command;
using Application.Features.CampaignManagement.Queries;
using Application.Features.SourceManagement;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View.CampaignView
{
    
    public partial class UpdateCampaign : Window
    {
        private readonly UpdateCampaignHandler _updateCampaignHandler;
        private readonly GetAllSourcesHandler _getAllSources;
        private readonly GetCampaignCustomersCount _getCampaignCustomers;
      
        private readonly int _campaignId;
        private  int _customerCount;

        public UpdateCampaign(UpdateCampaignDto currentCampaign, UpdateCampaignHandler updateCampaign,
            GetAllSourcesHandler getAllSources , GetCampaignCustomersCount campaignCustomersCount)
        {
            InitializeComponent();
            _updateCampaignHandler = updateCampaign;
            _getAllSources = getAllSources;
            _campaignId = currentCampaign.CampaignId;
            _getCampaignCustomers = campaignCustomersCount;

            Load_Sources();
            LoadCurrentValues(currentCampaign);
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

        private void LoadCurrentValues(UpdateCampaignDto currentCampaign)
        {
            TxtCampaignName.Text = currentCampaign.CampaignName;
            DpStartDate.SelectedDate = currentCampaign.StartDate.ToDateTime(TimeOnly.MinValue);
            DpEndDate.SelectedDate = currentCampaign.EndDate.ToDateTime(TimeOnly.MinValue);
            TxtDiscount.Text = currentCampaign.Discount.ToString();
            TxtNotes.Text = currentCampaign.Note ?? string.Empty;
            CbSources.SelectedValue = currentCampaign.SourceId;

            _customerCount = _getCampaignCustomers.Handle(_campaignId);


            if (_customerCount > 0)
            {
                CbSources.IsEnabled = false;   
                DpStartDate.IsEnabled = false;
                DpEndDate.IsEnabled = true;   
            }
            else
            {
                CbSources.IsEnabled = true;   
                DpStartDate.IsEnabled = true;
                DpEndDate.IsEnabled = true;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetErrors();

            bool isValid = true;
            int discount = 0;

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

            var updatedCampaign = new UpdateCampaignDto(
                _campaignId,
                TxtCampaignName.Text.Trim(),
                DateOnly.FromDateTime(DpStartDate.SelectedDate.Value),
                DateOnly.FromDateTime(DpEndDate.SelectedDate.Value),
                Convert.ToInt32(CbSources.SelectedValue),
                discount,
                string.IsNullOrWhiteSpace(TxtNotes.Text) ? null : TxtNotes.Text.Trim()
            );

            try
            {
                bool isUpdated = _updateCampaignHandler.Handle(updatedCampaign);

                if (isUpdated)
                {
                    MessageBox.Show("تم تعديل الحملة بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("فشل تعديل الحملة في قاعدة البيانات.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            TxtStartDateError.Visibility = Visibility.Collapsed;
            TxtEndDateError.Visibility = Visibility.Collapsed;
            TxtDiscountError.Visibility = Visibility.Collapsed;
        }
    }
}
