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

        public PurchaseOrderViewControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _purchaseOrderService = new PurchaseOrderService(context);
            Loaded += PurchaseOrderViewControl_Loaded;
        }

        private async void PurchaseOrderViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Load all purchase orders into ComboBox when control loads
            await LoadPurchaseOrders();
        }

        private async Task LoadPurchaseOrders()
        {
            var purchaseOrders = await _purchaseOrderService.FetchAllPurchaseOrdersAsync();
            cmbPurchaseOrders.ItemsSource = purchaseOrders;
        }

        private async void GeneratePurchaseOrderView(object sender, RoutedEventArgs e)
        {
            // Check if a purchase order is selected
            if (cmbPurchaseOrders.SelectedItem is not tblPurchaseOrderMaster selectedPurchaseOrder)
            {
                StatusMessageHelper.ShowMessage("Please select a purchase order.", true);
                return;
            }

            // Fetch purchase order line items and display in DataGrid
            var lineItems = await _purchaseOrderService.FetchPurchaseOrderLinesByOrderIdAsync(selectedPurchaseOrder.purchase_order_id);
            PurchaseOrderLinesDataGrid.ItemsSource = lineItems;

            // Calculate and display total amount
            var totalAmount = lineItems.Sum(line => line.line_total);
            txtTotalAmount.Text = $"Total: {totalAmount:F2}";
            txtDate.Text = $"Order Date: {selectedPurchaseOrder.purchase_order_date.ToString()}";
            txtTotalAmount.Visibility = Visibility.Visible;
            txtDate.Visibility = Visibility.Visible;
        }

        private async void DeletePurchaseOrder(object sender, RoutedEventArgs e)
        {
            // Check if a purchase order is selected
            if (cmbPurchaseOrders.SelectedItem is not tblPurchaseOrderMaster selectedPurchaseOrder)
            {
                StatusMessageHelper.ShowMessage("Please select a purchase order to delete.", true);
                return;
            }

            // Confirm the deletion with the user
            var result = await this.TryFindParent<MetroWindow>().ShowMessageAsync(
                "Confirm Deletion",
                "Are you sure you want to delete this purchase order?",
                MessageDialogStyle.AffirmativeAndNegative,
                new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" }
            );

            if (result != MessageDialogResult.Affirmative)
            {
                return; // Exit if the user selects "No"
            }

            // Attempt to delete the purchase order
            bool isDeleted = await _purchaseOrderService.DeletePurchaseOrderAsync(selectedPurchaseOrder.purchase_order_id);
            if (isDeleted)
            {
                StatusMessageHelper.ShowMessage("Purchase order deleted successfully.", false);

                // Refresh the purchase order list and clear displayed details
                await LoadPurchaseOrders();
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
