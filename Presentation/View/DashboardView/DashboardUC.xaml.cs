using Application.DTOs.Treasury;
using Application.Features.TreasuryManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation.View.DashboardView
{
    public partial class DashboardUC : UserControl
    {
        private  GetBalanceHandler _getBalanceHandler;

        public DashboardUC()
        {
            InitializeComponent();
     
        }

        public void InitializeServices(GetBalanceHandler getBalanceHandler)
        {
            _getBalanceHandler = getBalanceHandler;

            LoadDashboard();

        }

        private void LoadDashboard()
        {
            try
            {
                var curBalance = _getBalanceHandler.Handle();
                txtcurrentbalance.Text = curBalance.balance.ToString("F0") + " ج.م";

                txtlastupdated.Text = $"آخر تحديث: {curBalance.LastUpdate}";

                //var data = _dashboardService.GetDashboardData(daysRange);
                //_viewModel.Populate(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل تحميل بيانات الصفحة الرئيسية.\n{ex.Message}",
                                "خطأ",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            int days = Btn30Days.IsChecked == true ? 30 : 7;
            LoadDashboard();
        }

        private void PeriodToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (Btn30Days == null) return;

            int days = Btn30Days.IsChecked == true ? 30 : 7;
            LoadDashboard();
        }

        // ---------- التنقل من بطاقات المقاييس العلوية ----------
      
        private void OrdersCard_Click(object sender, MouseButtonEventArgs e) { }
        private void AppointmentsCard_Click(object sender, MouseButtonEventArgs e) {}

        // ---------- التنقل من صفوف حركات الخزنة (Drill-down) ----------
        private void ReferenceLink_Click(object sender, MouseButtonEventArgs e)
        { 

      
        }

        private void ViewAll_Click(object sender, MouseButtonEventArgs e)
        {
           // ViewAllTransactionsRequested?.Invoke();
        }
    }
}