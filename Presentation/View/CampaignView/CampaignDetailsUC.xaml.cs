using Application.DTOs.CampaignDTOs;
using Application.Features.CampaignManagement.Command;
using Application.Features.CampaignManagement.Queries;
using Application.Features.SourceManagement;
using Infrastructure.Repositories;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Properties;
namespace Presentation.View.CampaignView
{
    public partial class CampaignDetailsUC : UserControl
    {
        public event EventHandler BackRequested;
        private readonly int _campaignId;
        private readonly GetCampaignDetailsHandler _getCampaignDetailsHandler;
        public readonly DeleteCampaignHandler _deleteCampaign;
        private readonly UpdateCampaignHandler _updateCampaignHandler;
        private readonly GetAllSourcesHandler _getAllSourcesHandler;
        private CampaignDetailsDto _currentCampaignDetails;

        public CampaignDetailsUC(int campaignId, GetCampaignDetailsHandler getCampaignDetailsHandler, DeleteCampaignHandler deleteCampaign, UpdateCampaignHandler updateCampaignHandler, GetAllSourcesHandler getAllSourcesHandler)
        {
            InitializeComponent();
            _campaignId = campaignId;
            _getCampaignDetailsHandler = getCampaignDetailsHandler;
            _deleteCampaign = deleteCampaign;
            _updateCampaignHandler = updateCampaignHandler;
            _getAllSourcesHandler = getAllSourcesHandler;
            LoadCampaignDetails();
        }

        private void LoadCampaignDetails()
        {
            try
            {
                CampaignDetailsDto campaignDetails = _getCampaignDetailsHandler.Handle(_campaignId);

                if (campaignDetails == null)
                {
                    MessageBox.Show("عذراً، لم يتم العثور على تفاصيل هذه الحملة.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _currentCampaignDetails = campaignDetails;

                TxtCampaignNameHeader.Text = campaignDetails.CampaignName;
                TxtSourceName.Text = campaignDetails.SourceName;
                TxtStartDate.Text = campaignDetails.StartDate.ToString("dd/MM/yyyy");
                TxtEndDate.Text = campaignDetails.EndDate.ToString("dd/MM/yyyy");
                TxtDiscount.Text = $"{campaignDetails.Discount}%";
                TxtCampaignCost.Text = $"{campaignDetails.CampaignCost:f0} ج.م";
                TxtNotes.Text = campaignDetails.Note ?? "لا توجد ملاحظات لهذه الحملة.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء تحميل تفاصيل الحملة: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void BtnBack_Click(object sender, RoutedEventArgs e) => BackRequested?.Invoke(this, EventArgs.Empty);

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCampaignDetails == null)
            {
                MessageBox.Show("لا يمكن تعديل الحملة قبل تحميل بياناتها.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                CampaignDetailsDto campaignDetails = _getCampaignDetailsHandler.Handle(_campaignId);

                var sources = _getAllSourcesHandler.Handle();
                var matchedSource = sources.FirstOrDefault(s => s.SourceName == campaignDetails.SourceName);

                if (matchedSource == null)
                {
                    MessageBox.Show("تعذر تحديد منصة الإعلان الحالية لهذه الحملة.", "تنبيه", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var updateDto = new UpdateCampaignDto(
                    _campaignId,
                    campaignDetails.CampaignName,
                    campaignDetails.StartDate,
                    campaignDetails.EndDate,
                    matchedSource.SourceId,
                    campaignDetails.Discount,
                    campaignDetails.Note
                );

                var updateWindow = new UpdateCampaign(updateDto, _updateCampaignHandler, _getAllSourcesHandler)
                {
                    Owner = Window.GetWindow(this)
                };

                bool? result = updateWindow.ShowDialog();

                if (result == true)
                {
                    LoadCampaignDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء فتح شاشة التعديل: {ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("هل أنت متأكد من حذف هذه الحملة.",
                                         "تأكيد الحذف", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _deleteCampaign.Handle(_campaignId);
                    MessageBox.Show("تم حذف الحملة بنجاح.", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                    BackRequested?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("لا يمكن حذف هذه الحملة لوجود عملاء مسجلين عليها حالياً", "فشل", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}