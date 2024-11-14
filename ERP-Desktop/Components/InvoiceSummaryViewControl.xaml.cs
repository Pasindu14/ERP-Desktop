using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using ERP_Desktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop.Components
{
    /// <summary>
    /// Interaction logic for InvoiceSummaryViewControl.xaml
    /// </summary>
    public partial class InvoiceSummaryViewControl : UserControl
    {
        private readonly InvoiceService _invoiceService;
        private readonly InputValidator _validatorDates;

        public InvoiceSummaryViewControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _invoiceService = new InvoiceService(context);
            _validatorDates = new InputValidator();
            _validatorDates.RegisterDatePicker(dpFromDate, "From Data");
            _validatorDates.RegisterDatePicker(dpToDate, "To Data");
        }

        private async void FilterInvoicesByDate(object sender, RoutedEventArgs e)
        {

            if (_validatorDates.ValidateAll())
            {
                var fromDate = dpFromDate.SelectedDate!.Value;
                var toDate = dpToDate.SelectedDate!.Value;

                if (fromDate > toDate)
                {
                    StatusMessageHelper.ShowMessage("'From' date cannot be later than 'To' date.", true);
                    return;
                }

                // Fetch and display filtered invoices
                var dailySales = await _invoiceService.FetchDailySalesReportAsync(fromDate, toDate);
                InvoicesDataGrid.ItemsSource = dailySales;

                // Calculate and display total amount for filtered invoices
                var totalSales = dailySales.Sum(sale => sale.TotalSales);
                var totalProfit = dailySales.Sum(sale => sale.TotalProfit);

                txtTotalSales.Text = $"Total Sales: {totalSales:F2}";
                //txtTotalAmount.Text = $"Total: {totalAmount:C2}";
                txtTotalSales.Visibility = dailySales.Any() ? Visibility.Visible : Visibility.Hidden;

                txtTotalProfit.Text = $"Total Profit: {totalProfit:F2}";
                //txtTotalAmount.Text = $"Total: {totalAmount:C2}";
                txtTotalProfit.Visibility = dailySales.Any() ? Visibility.Visible : Visibility.Hidden;

                // Show result message
                if (dailySales.Count == 0)
                {
                    StatusMessageHelper.ShowMessage("No data found for this criteria");
                }
            }
            else {
                StatusMessageHelper.ShowMessage("Please select date", true);
                return;
            }

        }

     
    }
}
