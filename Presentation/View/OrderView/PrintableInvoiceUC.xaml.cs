using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Presentation.View.OrderView
{
    /// <summary>
    /// Interaction logic for PrintableInvoiceUC.xaml
    /// </summary>
    public partial class PrintableInvoiceUC : UserControl
    {
        public void LoadInvoice() //(InvoicePrintDto dto)
        {
        //    TxtInvoiceNumber.Text = dto.InvoiceNumber;
        //    TxtDate.Text = dto.Date.ToString("dd MMMM yyyy", new CultureInfo("ar-EG"));
        //    TxtWarranty.Text = dto.WarrantyPeriod;
        //    TxtCustomerName.Text = dto.CustomerName;
        //    TxtCustomerAddress.Text = dto.CustomerAddress;
        //    TxtCustomerPhone.Text = dto.CustomerPhone;
        //    TxtDeviceName.Text = dto.DeviceName;
        //    TxtProblem.Text = dto.Problem;
        //    TxtSubtotal.Text = dto.Subtotal.ToString("N2");
        //    TxtDiscount.Text = dto.Discount.ToString("N2");
        //    TxtTotal.Text = dto.Total.ToString("N2");

        //    // قطع الغيار — بيملا 5 صفوف دايماً زي الفاتورة الأصلية
        //    var rows = new List<InvoiceRowDto>();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        if (i < dto.Parts.Count)
        //            rows.Add(new InvoiceRowDto
        //            {
        //                RowNumber = i + 1,
        //                PartName = dto.Parts[i].PartName,
        //                Quantity = dto.Parts[i].Quantity.ToString(),
        //                UnitPrice = dto.Parts[i].UnitPrice.ToString("N2"),
        //                TotalPrice = dto.Parts[i].TotalPrice.ToString("N2")
        //            });
        //        else
        //            rows.Add(new InvoiceRowDto
        //            {
        //                RowNumber = i + 1,
        //                PartName = "-",
        //                Quantity = "-",
        //                UnitPrice = "-",
        //                TotalPrice = "-"
        //            });
        //    }
        //    PartsItemsControl.ItemsSource = rows;

        //    if (File.Exists(dto.LogoPath))
        //        ImgLogo.Source = new BitmapImage(new Uri(dto.LogoPath));

        //    // QR Code — محتاج library زي QRCoder
        //    // ImgQR.Source = GenerateQR(dto.InvoiceNumber);
        //
        }
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //var invoice = new PrintableInvoiceUC();
            //invoice.LoadInvoice(_invoiceDto);

            //var pd = new PrintDialog();
            //if (pd.ShowDialog() == true)
            //{
            //    invoice.Measure(new Size(pd.PrintableAreaWidth, double.PositiveInfinity));
            //    invoice.Arrange(new Rect(new Size(pd.PrintableAreaWidth, invoice.DesiredSize.Height)));
            //    invoice.UpdateLayout();
            //    pd.PrintVisual(invoice, $"فاتورة رقم {_invoiceDto.InvoiceNumber}");
            //}
        }
    }
}
