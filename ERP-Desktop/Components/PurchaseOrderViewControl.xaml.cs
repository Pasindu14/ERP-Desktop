using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using ERP_Desktop.Services;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop.Components
{
    public partial class PurchaseOrderViewControl : UserControl
    {
        private readonly PurchaseOrderService _purchaseOrderService;
        private readonly InputValidator _validatorDateRange;

        public PurchaseOrderViewControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _purchaseOrderService = new PurchaseOrderService(context);
            _validatorDateRange = new InputValidator();

            Loaded += PurchaseOrderViewControl_Loaded;
        }

        private void PurchaseOrderViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            _validatorDateRange.RegisterDatePicker(dpFromDate, "From Date");
            _validatorDateRange.RegisterDatePicker(dpToDate, "To Date");
        }

        private async void FilterPurchaseOrdersByDate(object sender, RoutedEventArgs e)
        {
            TextBoxHelper.SetWatermark(cmbPurchaseOrders, "Select a purchase order");
            if (_validatorDateRange.ValidateAll())
            {
                var fromDate = dpFromDate.SelectedDate!.Value;
                var toDate = dpToDate.SelectedDate!.Value;

                if (fromDate > toDate)
                {
                    StatusMessageHelper.ShowMessage("'From' date cannot be later than 'To' date.", true);
                    return;
                }

                // Fetch purchase orders in the selected date range
                var purchaseOrders = await _purchaseOrderService.FetchPurchaseOrdersByDateRangeAsync(fromDate, toDate);
                cmbPurchaseOrders.ItemsSource = purchaseOrders;

                if (!purchaseOrders.Any())
                {

                    PurchaseOrderLinesDataGrid.ItemsSource = null;
                    txtTotalAmount.Visibility = Visibility.Hidden;
                    txtDate.Visibility = Visibility.Hidden;
                    StatusMessageHelper.ShowMessage("No purchase orders found for the selected date range.", true);
                }

            }
            else
            {
                StatusMessageHelper.ShowMessage("Please select both 'From' and 'To' dates.", true);
            }
        }

        private async void GeneratePurchaseOrderView(object sender, RoutedEventArgs e)
        {
            if (cmbPurchaseOrders.SelectedItem is not tblPurchaseOrderMaster selectedPurchaseOrder)
            {
                StatusMessageHelper.ShowMessage("Please select a purchase order.", true);
                return;
            }

            var lineItems = await _purchaseOrderService.FetchPurchaseOrderLinesByOrderIdAsync(selectedPurchaseOrder.purchase_order_id);
            PurchaseOrderLinesDataGrid.ItemsSource = lineItems;

            var totalAmount = lineItems.Sum(line => line.line_total);
            txtTotalAmount.Text = $"Total: {totalAmount:F2}";
            txtDate.Text = $"Order Date: {selectedPurchaseOrder.purchase_order_date.ToString()}";
            txtTotalAmount.Visibility = Visibility.Visible;
            txtDate.Visibility = Visibility.Visible;
        }

        private async void DeletePurchaseOrder(object sender, RoutedEventArgs e)
        {
            if (cmbPurchaseOrders.SelectedItem is not tblPurchaseOrderMaster selectedPurchaseOrder)
            {
                StatusMessageHelper.ShowMessage("Please select a purchase order to delete.", true);
                return;
            }

            var result = await this.TryFindParent<MetroWindow>().ShowMessageAsync(
                "Confirm Deletion",
                "Are you sure you want to delete this purchase order?",
                MessageDialogStyle.AffirmativeAndNegative,
                new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" }
            );

            if (result != MessageDialogResult.Affirmative) return;

            bool isDeleted = await _purchaseOrderService.DeletePurchaseOrderAsync(selectedPurchaseOrder.purchase_order_id);
            if (isDeleted)
            {
                StatusMessageHelper.ShowMessage("Purchase order deleted successfully.", false);

                // Reload and filter purchase orders
                var fromDate = dpFromDate.SelectedDate!.Value;
                var toDate = dpToDate.SelectedDate!.Value;
                var purchaseOrders = await _purchaseOrderService.FetchPurchaseOrdersByDateRangeAsync(fromDate, toDate);
                cmbPurchaseOrders.ItemsSource = purchaseOrders;

                PurchaseOrderLinesDataGrid.ItemsSource = null;
                txtTotalAmount.Visibility = Visibility.Hidden;
                txtDate.Visibility = Visibility.Hidden;
            }
            else
            {
                StatusMessageHelper.ShowMessage("Failed to delete purchase order. Please try again.", true);
            }
        }
    }
}
