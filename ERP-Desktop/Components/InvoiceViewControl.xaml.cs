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
    public partial class InvoiceViewControl : UserControl
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceViewControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _invoiceService = new InvoiceService(context);
            Loaded += InvoiceViewControl_Loaded;
        }

        private async void InvoiceViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Load all invoices into ComboBox when control loads
            await LoadInvoices();
        }

        private async Task LoadInvoices()
        {
            var invoices = await _invoiceService.FetchAllInvoicesAsync();
            cmbInvoices.ItemsSource = invoices;
        }


        private async void GenerateInvoiceView(object sender, RoutedEventArgs e)
        {
            // Check if an invoice is selected
            if (cmbInvoices.SelectedItem is not tblInvoiceMaster selectedInvoice)
            {
                StatusMessageHelper.ShowMessage("Please select an invoice.", true);
                return;
            }

            // Fetch invoice line items and display in DataGrid
            var lineItems = await _invoiceService.FetchInvoiceLinesByInvoiceIdAsync(selectedInvoice.invoice_id);
            InvoiceLinesDataGrid.ItemsSource = lineItems;

            // Calculate and display total amount
            var totalAmount = lineItems.Sum(line => line.LineTotal);
            txtTotalAmount.Text = $"Total: {totalAmount:F2}";
            txtDate.Text = $"Invoice Date: {selectedInvoice.invoice_date.ToString()}";
            txtTotalAmount.Visibility = Visibility.Visible;
            txtDate.Visibility = Visibility.Visible;

        }

        private async void DeleteInvoice(object sender, RoutedEventArgs e)
        {
            // Check if an invoice is selected
            if (cmbInvoices.SelectedItem is not tblInvoiceMaster selectedInvoice)
            {
                StatusMessageHelper.ShowMessage("Please select an invoice to delete.", true);
                return;
            }

            // Confirm the deletion with the user
            // Show confirmation dialog using MahApps
            var result = await this.TryFindParent<MetroWindow>().ShowMessageAsync(
                "Confirm Deletion",
                "Are you sure you want to delete this invoice?",
                MessageDialogStyle.AffirmativeAndNegative,
                new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" }
            );

            if (result != MessageDialogResult.Affirmative)
            {
                return; // Exit if the user selects "No"
            }

            // Attempt to delete the invoice
            bool isDeleted = await _invoiceService.DeleteInvoiceAsync(selectedInvoice.invoice_id);
            if (isDeleted)
            {
                StatusMessageHelper.ShowMessage("Invoice deleted successfully.", false);

                // Refresh the invoice list and clear displayed details
                await LoadInvoices();
                InvoiceLinesDataGrid.ItemsSource = null;
                txtTotalAmount.Visibility = Visibility.Hidden;
                txtDate.Visibility = Visibility.Hidden;
            }
            else
            {
                StatusMessageHelper.ShowMessage("Failed to delete invoice. Please try again.", true);
            }
        }

    }
}
