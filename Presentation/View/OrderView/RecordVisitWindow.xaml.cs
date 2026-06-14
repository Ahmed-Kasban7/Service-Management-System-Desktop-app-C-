using Application.DTOs.SparePartDtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for RecordVisitWindow.xaml
    /// </summary>
    public partial class RecordVisitWindow : Window
    {
        public ObservableCollection<SparePartDTO> SparePartsList { get; set; } = new();

        public RecordVisitWindow()
        {
            InitializeComponent();
            ContainerSpareParts.ItemsSource = SparePartsList;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveVisit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void BtnAddPartRow_Click(object sender, RoutedEventArgs e)
        {
            {
                SparePartsList.Add(new SparePartDTO());
            }
        }

        private void BtnDeletePartRow_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var part = btn?.DataContext as SparePartDTO;
            if (part != null)
                SparePartsList.Remove(part);
        }

        private void CbPaidBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbPaidBy == null || PanelPaidByEmployee == null) return;

            if (CbPaidBy.SelectedItem is ComboBoxItem selectedItem)
            {
                string? tag = selectedItem.Tag?.ToString();

                if (tag == "Employee")
                {
                    PanelPaidByEmployee.Visibility = Visibility.Visible;

                    // تحميل الموظفين مباشرة باسم الكومبو بوكس التاني
                    if (CbPaidByEmployee != null && CbPaidByEmployee.ItemsSource == null)
                    {
                        // فك الكومنت هنا لما تشغل الهاندلر بتاعك
                        // CbPaidByEmployee.ItemsSource = _employeesLookupHandler.Handle("TECHNICIAN");
                    }
                }
                else
                {
                    PanelPaidByEmployee.Visibility = Visibility.Collapsed;

                    if (CbPaidByEmployee != null)
                    {
                        CbPaidByEmployee.SelectedValue = null;
                    }
                }
            }
        }
    }
}
