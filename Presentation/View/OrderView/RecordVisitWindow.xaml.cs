using Application.DTOs.SparePartDtos;
using Application.DTOs.VisitDTOs;
using Application.Features.EmployeeManagement.Queries;
using Application.Features.VisitManagement;
using Domain.Enums;
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
        private readonly CreateVisitHandler _createVisitHandler;
        private readonly GetAllEmployeesLookupHandler _getAllEmployees;
        private readonly int _appointmentId;

        public ObservableCollection<SparePartDto> SparePartsList { get; set; } = new();
        public RecordVisitWindow(CreateVisitHandler createVisitHandler, int appointmentId , GetAllEmployeesLookupHandler getAllEmployees) 
        {
            InitializeComponent();
            _createVisitHandler = createVisitHandler;
            _appointmentId = appointmentId;
            _getAllEmployees = getAllEmployees;

            ContainerSpareParts.ItemsSource = SparePartsList;
        }
        private void BtnSaveVisit_Click(object sender, RoutedEventArgs e)
        {
            TxtDiagnosisError.Visibility = Visibility.Collapsed;
            TxtCostError.Visibility = Visibility.Collapsed;
            TxtPaidError.Visibility = Visibility.Collapsed;
            TxtTransportError.Visibility = Visibility.Collapsed;
            txtPartTransportCostError.Visibility = Visibility.Collapsed;
            TxtPaidByEmployeeError.Visibility = Visibility.Collapsed;

            bool hasError = false;

            if (string.IsNullOrWhiteSpace(TxtDiagnosis.Text))
            {
                TxtDiagnosisError.Text = "التشخيص مطلوب";
                TxtDiagnosisError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!decimal.TryParse(TxtMaintenanceCost.Text, out decimal MaintenanceCost) || MaintenanceCost < 0)
            {
                TxtCostError.Text = "برجاء ادخال تكلفة صالحه";
                TxtCostError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!decimal.TryParse(TxtTransportCost.Text, out decimal transportCost) || transportCost < 0)
            {
                TxtTransportError.Text = "برجاء ادخال تكلفة صالحه";
                TxtTransportError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (!decimal.TryParse(TxtAmountPaid.Text, out decimal amountPaid) || amountPaid < 0)
            {
                TxtPaidError.Text = "برجاء ادخال تكلفة صالحه";
                TxtPaidError.Visibility = Visibility.Visible;
                hasError = true;
            }

            ETransportationBearer? bearer = null;
            decimal? partsTransCost = null;
            int? paidByEmployeeId = null;
            var validatedParts = new List<SparePartDto>();

            if (SparePartsList.Count > 0)
            {
                for (int i = 0; i < SparePartsList.Count; i++)
                {
                    var container = ContainerSpareParts.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                    if (container == null) continue;

                    var nameBox = FindChildByName<TextBox>(container, "TxtPartName");
                    var qtyBox = FindChildByName<TextBox>(container, "TxtQuantity");
                    var priceBox = FindChildByName<TextBox>(container, "TxtUnitPrice");

                    var nameError = FindChildByName<TextBlock>(container, "PartNameError");
                    var qtyError = FindChildByName<TextBlock>(container, "TxtQuantityError");
                    var priceError = FindChildByName<TextBlock>(container, "TxtUnitPriceError");

                    string partName = nameBox?.Text?.Trim() ?? "";
                    bool quantityValid = int.TryParse(qtyBox?.Text, out int parsedQuantity) && parsedQuantity > 0;
                    bool priceValid = decimal.TryParse(priceBox?.Text, out decimal parsedUnitPrice) && parsedUnitPrice >= 0;

                    if (string.IsNullOrWhiteSpace(partName))
                    {
                        if (nameError != null)
                        {
                            nameError.Text = "اسم القطعة مطلوب";
                            nameError.Visibility = Visibility.Visible;
                        }
                        hasError = true;
                    }
                    else
                    {
                        if (nameError != null) nameError.Visibility = Visibility.Collapsed;
                    }

                    if (!quantityValid)
                    {
                        if (qtyError != null)
                        {
                            qtyError.Text = "برجاء ادخال كمية صالحه";
                            qtyError.Visibility = Visibility.Visible;
                        }
                        hasError = true;
                    }
                    else
                    {
                        if (qtyError != null) qtyError.Visibility = Visibility.Collapsed;
                    }

                    if (!priceValid)
                    {
                        if (priceError != null)
                        {
                            priceError.Text = "برجاء ادخال تكلفة صالحه";
                            priceError.Visibility = Visibility.Visible;
                        }
                        hasError = true;
                    }
                    else
                    {
                        if (priceError != null) priceError.Visibility = Visibility.Collapsed;
                    }

                    if (quantityValid && priceValid && !string.IsNullOrWhiteSpace(partName))
                    {
                        validatedParts.Add(new SparePartDto(
                            PartName: partName,
                            Quantity: parsedQuantity,
                            UnitPrice: parsedUnitPrice
                        ));
                    }
                }

                if (!decimal.TryParse(TxtPartTransportCost.Text, out decimal parsedPartsCost) || parsedPartsCost < 0)
                {
                    txtPartTransportCostError.Text = "برجاء ادخال تكلفة صالحه";
                    txtPartTransportCostError.Visibility = Visibility.Visible;
                    hasError = true;
                }
                else
                {
                    partsTransCost = parsedPartsCost;
                }

                if (CbPaidBy.SelectedItem is ComboBoxItem selectedItem)
                {
                    string? tag = selectedItem.Tag?.ToString();
                    bearer = tag == "Employee" ? ETransportationBearer.EMPLOYEE : ETransportationBearer.COMPANY;

                    if (tag == "Employee")
                    {
                        if (CbPaidByEmployee.SelectedValue == null)
                        {
                            TxtPaidByEmployeeError.Text = "برجاء اختيار الموظف المسئول";
                            TxtPaidByEmployeeError.Visibility = Visibility.Visible;
                            hasError = true;
                        }
                        else
                        {
                            paidByEmployeeId = (int)CbPaidByEmployee.SelectedValue;
                        }
                    }
                }
            }

            if (hasError) return;

            var confirmResult = MessageBox.Show(
    "⚠️ تحذير نهائي\n\n" +
    "سيتم اعتماد التقرير المالي بشكل نهائي بعد الإنشاء.\n" +
    "لن يُسمح بأي تعديل بعد الانشاء.\n\n" +
    "هل أنت متأكد من الاستمرار؟",
    "تأكيد الإجراء",
    MessageBoxButton.YesNo,
    MessageBoxImage.Warning,
    MessageBoxResult.No);
            if (confirmResult != MessageBoxResult.Yes)
                return;

            try
            {
                var newVisitDto = new CreateVisistDto(
                    AppointmentID: _appointmentId,
                    Diagnosis: TxtDiagnosis.Text.Trim(),
                    ActionsTaken: TxtActionsTaken.Text.Trim(),
                    Notes: TxtNotes.Text.Trim(),
                    TotalCostToCustomer: MaintenanceCost,
                    TransportationCost: transportCost,
                    AmountPaid: amountPaid,
                    TransportationBearer: bearer,
                    PartsTransportationCost: partsTransCost,
                    PaidByEmployeeID: paidByEmployeeId,
                    SpareParts: validatedParts

                );

                var result = _createVisitHandler.Handle(newVisitDto);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم حفظ تقرير الزيارة بنجاح!", "نجاح العملية", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Error, "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}", "خطأ نظام", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private T FindChildByName<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T element && child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                {
                    return element;
                }

                var foundChild = FindChildByName<T>(child, childName);
                if (foundChild != null) return foundChild;
            }
            return null;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnAddPartRow_Click(object sender, RoutedEventArgs e)
        {
            {
                SparePartsList.Add(new SparePartDto(PartName: "", Quantity: 1, UnitPrice: 0.00m));
            }
        }

        private void BtnDeletePartRow_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var part = btn?.DataContext as SparePartDto;
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

                    if (CbPaidByEmployee != null && CbPaidByEmployee.ItemsSource == null)
                    {
                        CbPaidByEmployee.ItemsSource = _getAllEmployees.Handle();

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
