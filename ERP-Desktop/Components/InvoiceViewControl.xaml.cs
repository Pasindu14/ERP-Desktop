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
        private readonly InputValidator _validatorDateRange;
        public InvoiceViewControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _invoiceService = new InvoiceService(context);
            Loaded += InvoiceViewControl_Loaded;
            _validatorDateRange = new InputValidator();
        }

        private void InvoiceViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            _validatorDateRange.RegisterDatePicker(dpFromDate, "From Date");
            _validatorDateRange.RegisterDatePicker(dpToDate, "To Date");
        }

        private async void FilterInvoicesByDate(object sender, RoutedEventArgs e)
        {
            TextBoxHelper.SetWatermark(cmbInvoices, "Select an invoice");
            if (_validatorDateRange.ValidateAll())
            {
                var fromDate = dpFromDate.SelectedDate!.Value;
                var toDate = dpToDate.SelectedDate!.Value;

                if (fromDate > toDate)
                {
                    StatusMessageHelper.ShowMessage("'From' date cannot be later than 'To' date.", true);
                    return;
                }

                // Fetch invoices in the selected date range
                var invoices = await _invoiceService.FetchInvoicesByDateRangeAsync(fromDate, toDate);
                cmbInvoices.ItemsSource = invoices;

                // Clear displayed details if no invoices found
                if (!invoices.Any())
                {
                    InvoiceLinesDataGrid.ItemsSource = null;
                    txtTotalAmount.Visibility = Visibility.Hidden;
                    txtDate.Visibility = Visibility.Hidden;
                    StatusMessageHelper.ShowMessage("No invoices found for the selected date range.", true);
                }
            }
            else {
                StatusMessageHelper.ShowMessage("Please select both 'From' and 'To' dates.", true);
                return;
            }

        }

        private async void GenerateInvoiceView(object sender, RoutedEventArgs e)
        {
            if (cmbInvoices.SelectedItem is not tblInvoiceMaster selectedInvoice)
            {
                StatusMessageHelper.ShowMessage("Please select an invoice.", true);
                return;
            }

            var lineItems = await _invoiceService.FetchInvoiceLinesByInvoiceIdAsync(selectedInvoice.invoice_id);
            InvoiceLinesDataGrid.ItemsSource = lineItems;

            var totalAmount = lineItems.Sum(line => line.LineTotal);
            txtTotalAmount.Text = $"Total: {totalAmount:F2}";
            txtDate.Text = $"Invoice Date: {selectedInvoice.invoice_date.ToString()}";
            txtTotalAmount.Visibility = Visibility.Visible;
            txtDate.Visibility = Visibility.Visible;
        }

        private async void DeleteInvoice(object sender, RoutedEventArgs e)
        {
            if (cmbInvoices.SelectedItem is not tblInvoiceMaster selectedInvoice)
            {
                StatusMessageHelper.ShowMessage("Please select an invoice to delete.", true);
                return;
            }

            var result = await this.TryFindParent<MetroWindow>().ShowMessageAsync(
                "Confirm Deletion",
                "Are you sure you want to delete this invoice?",
                MessageDialogStyle.AffirmativeAndNegative,
                new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" }
            );

            if (result != MessageDialogResult.Affirmative) return;

            bool isDeleted = await _invoiceService.DeleteInvoiceAsync(selectedInvoice.invoice_id);
            if (isDeleted)
            {
                StatusMessageHelper.ShowMessage("Invoice deleted successfully.", false);

                var fromDate = dpFromDate.SelectedDate!.Value;
                var toDate = dpToDate.SelectedDate!.Value;
                var invoices = await _invoiceService.FetchInvoicesByDateRangeAsync(fromDate, toDate);
                cmbInvoices.ItemsSource = invoices;

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
