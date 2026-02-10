using Infrastructure.Data;
using Infrastructure;
using Presentation.View.Customer_View;
using System.Windows;
using Application.DTOs;

namespace Presentation;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            DatabaseInitializer.InitializeDatabase();

            var customerRepo = new CustomerRepository();

        
            var customerWindow = new CustomerListView(customerRepo);

            customerWindow.Show();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                ex.InnerException?.Message ?? ex.Message,
                "خطأ حقيقي من SQL",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );

            Current.Shutdown();
        }
    }
}