using Application.Common;
using Application.DTOs.CampaignDTOs;
using Application.DTOs.CustomerDTOs;
using Application.Features.BrandManagement.Queries;
using Application.Features.CampaignManagement.Command;
using Application.Features.CampaignManagement.Queries;
using Application.Features.CustomerManagement.Queries;
using Application.Features.CustomerManagment.Commands;
using Application.Features.SourceManagement;
using Application.Features.SpecManagement.Queries;
using Application.Features.TypeManagement.Queries;
using Infrastructure.Repositories;
using Presentation.View.Customer_View;
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
namespace Presentation.View.CampaignView
{
    /// <summary>
    /// Interaction logic for CampaignUC.xaml
    /// </summary>
    public partial class CampaignUC : UserControl
    {
        private int CurrentPage = 1;
        private const int ROWPERPAGE = 8;
        private int TotalPages;
        private int TotalCustomers;
        private bool IsSearching = false;
        private string CurrentSearchText = "";
        private CreateCampaignHandler _createCampaignHandler;
        private GetPagedCampaignSummariesHandler _getPagedCampaignSummariesHandler;
        private SearchCampaignsPaged _searchCampaignsPaged;
        private GetAllSourcesHandler _getSourcesHandler;
        private GetCampaignDetailsHandler  _getCampaignDetailsHandler;
        private DeleteCampaignHandler _deleteCampaign;
        private UpdateCampaignHandler _updateCampaignHandler;
        private GetCampaignCustomersCount _getCampaignCustomersCount;
        public CampaignUC()
        {
            InitializeComponent();

        }
        public void InitializeServices(CreateCampaignHandler createCampaignHandler, 
            GetAllSourcesHandler getAllSources, GetPagedCampaignSummariesHandler getPagedCampaign , SearchCampaignsPaged searchCampaigns ,
            GetCampaignDetailsHandler detailsHandler , DeleteCampaignHandler deleteCampaign , UpdateCampaignHandler updateCampaign , GetCampaignCustomersCount getCampaignCustomers)
        {

            _createCampaignHandler = createCampaignHandler;
            _getSourcesHandler = getAllSources;
            _getPagedCampaignSummariesHandler = getPagedCampaign;
            _searchCampaignsPaged = searchCampaigns;
            _getCampaignDetailsHandler = detailsHandler;
            _deleteCampaign = deleteCampaign;
            _updateCampaignHandler = updateCampaign;
            _getCampaignCustomersCount = getCampaignCustomers;
            _createCampaignHandler.CampaignCreated += LoadAndBindCampaigns;

            LoadAndBindCampaigns();
        }

        private void BtnCreateCampaign_Click(object sender, RoutedEventArgs e)
        {
            var createCampaign = new CreateCampaignWindow(_createCampaignHandler, _getSourcesHandler)
            {
                Owner = Window.GetWindow(this)
            };
            createCampaign.ShowDialog();
        }

        private PagedResult<CampaignSummariesDto> LoadCampaigns()
        {
            try
            {
                return _getPagedCampaignSummariesHandler.Handle(CurrentPage, ROWPERPAGE);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"حدث خطأ اثناء تحميل بيانات الحملات");
                return new PagedResult<CampaignSummariesDto>(
                    new List<CampaignSummariesDto>(), 0, 1, ROWPERPAGE);
            }
        }

        public void LoadAndBindCampaigns()
        {
            PagedResult<CampaignSummariesDto> result;

            if (IsSearching && !string.IsNullOrWhiteSpace(CurrentSearchText))
            {
                result = SearchCampaigns(CurrentSearchText, CurrentPage, ROWPERPAGE);
            }
            else
            {
                result = LoadCampaigns();
            }

            Bind(result);
        }

        private PagedResult<CampaignSummariesDto> SearchCampaigns(string searchText, int page, int rowsPerPage)
        {
            try
            {
                return _searchCampaignsPaged.Handle(searchText, page, rowsPerPage);
            
            }
            catch
            {
                MessageBox.Show("حدث خطأ اثناء البحث عن الحملات");
                return new PagedResult<CampaignSummariesDto>(
                    new List<CampaignSummariesDto>(), 0, 1, rowsPerPage);
            }
        }

        private void Bind(PagedResult<CampaignSummariesDto> result)
        {
            if (result == null) return;

            DgCampaign.ItemsSource = result.Items;
            TxtPageInfo.Text = CurrentPage.ToString();
            TxtCampaignCountNumber.Text = result.TotalCount.ToString();

            BtnNextPage.IsEnabled = result.HasNextPage;
            BtnPrevPage.IsEnabled = result.HasPreviousPage;

        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (!BtnNextPage.IsEnabled) return;

            CurrentPage++;
            LoadAndBindCampaigns();
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage <= 1) return;

            CurrentPage--;
            LoadAndBindCampaigns();
        }

        private void DgCampaign_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (DgCampaign.SelectedItem is CampaignSummariesDto selectedCampaign)
            {
                var campaignDetailsUC = new CampaignDetailsUC(selectedCampaign.Id , _getCampaignDetailsHandler , 
                    _deleteCampaign , _updateCampaignHandler, _getSourcesHandler  , _getCampaignCustomersCount );

                campaignDetailsUC.BackRequested += (s, args) =>
                {
                    CampaignDetailsHolder.Visibility = Visibility.Collapsed;
                    CampaignDetailsHolder.Content = null;
                    CampaignContainer.Visibility = Visibility.Visible;
                    LoadAndBindCampaigns();
                };

                CampaignDetailsHolder.Content = campaignDetailsUC;
                CampaignContainer.Visibility = Visibility.Collapsed;
                CampaignDetailsHolder.Visibility = Visibility.Visible;
            }
        }

        private void ResetSearch()
        {
            IsSearching = false;
            CurrentSearchText = "";
            CurrentPage = 1;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrentPage = 1;

                if (string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    ResetSearch();
                }
                else
                {
                    IsSearching = true;
                    CurrentSearchText = SearchBox.Text;
                }

                LoadAndBindCampaigns();
            }
        }
    }
}