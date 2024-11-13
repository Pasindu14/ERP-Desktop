using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using ERP_Desktop.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop.Components
{
    /// <summary>
    /// Interaction logic for PurchaseOrderSummaryViewControl.xaml
    /// </summary>
    public partial class PurchaseOrderSummaryViewControl : UserControl
    {
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly InputValidator _validatorDates;

        public PurchaseOrderSummaryViewControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _purchaseOrderService = new PurchaseOrderService(context);
            _validatorDates = new InputValidator();
            _validatorDates.RegisterDatePicker(dpFromDate, "From Date");
            _validatorDates.RegisterDatePicker(dpToDate, "To Date");
        }

        private async void FilterPurchaseOrdersByDate(object sender, RoutedEventArgs e)
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

                // Fetch and display filtered purchase orders
                var dailyPurchases = await _purchaseOrderService.FetchDailyPurchaseReportAsync(fromDate, toDate);
                PurchaseOrdersDataGrid.ItemsSource = dailyPurchases;

                // Calculate and display total purchase amount for filtered orders
                var totalPurchaseAmount = dailyPurchases.Sum(purchase => purchase.TotalPurchaseAmount);
                txtTotalPurchaseAmount.Text = $"Total Purchase: {totalPurchaseAmount:F2}";
                txtTotalPurchaseAmount.Visibility = dailyPurchases.Any() ? Visibility.Visible : Visibility.Hidden;

                // Show result message
                if (dailyPurchases.Count == 0)
                {
                    StatusMessageHelper.ShowMessage("No data found for this criteria");
                }
            }
            else
            {
                StatusMessageHelper.ShowMessage("Please select date", true);
            }
        }
    }
}
