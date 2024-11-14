using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using ERP_Desktop.Services;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop.Components
{
    /// <summary>
    /// Interaction logic for AddInvoiceControl.xaml
    /// </summary>
    public partial class AddInvoiceControl : UserControl
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly InvoiceService _invoiceService;

        // List to hold the invoice line items
        private List<tblInvoiceLine> _invoiceItems;
        private readonly InputValidator _validatorProduct;
        private readonly InputValidator _validatorInvoice;

        public AddInvoiceControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _categoryService = new CategoryService(context);
            _productService = new ProductService(context);
            _invoiceService = new InvoiceService(context);

            txtQuantity.PreviewTextInput += InputHelper.DecimalOnlyTextBox_PreviewTextInput;
            _invoiceItems = new List<tblInvoiceLine>();
            Loaded += AddInvoiceControl_Loaded;


            _validatorProduct = new InputValidator();
            _validatorInvoice = new InputValidator();
            _validatorProduct.RegisterTextBox(txtQuantity, "Quantity");
            _validatorProduct.RegisterComboBox(cmbProduct, "Product");

            _validatorInvoice.RegisterTextBox(txtInvoiceNumber, "Invoice");
            _validatorInvoice.RegisterDatePicker(dpInvoiceDate, "Date");

        }


        private async Task LoadProducts()
        {
            var products = await _productService.FetchAllActiveProductsAsync();
            var productWrapper = products.Select(c => new Wrapper.ProductDisplayWrapper(c));
            cmbProduct.ItemsSource = productWrapper;
        }

        private async void AddInvoiceControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProducts();

        }

        private async void AddProductItem(object sender, RoutedEventArgs e)
        {
            if (_validatorProduct.ValidateAll())
            {
                // Get selected product and quantity
                var selectedProduct = cmbProduct.SelectedItem as Wrapper.ProductDisplayWrapper;
                if (selectedProduct == null || !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    StatusMessageHelper.ShowMessage("Please select a product and enter a valid quantity.", true);
                    return;
                }

                // Fetch product details from the database using the product code
                var product = await _productService.FetchProductByCodeAsync(selectedProduct.ProdCode);
                if (product == null)
                {
                    StatusMessageHelper.ShowMessage("Selected product not found in the database.", true);
                    return;
                }

                // Check if enough stock is available
                if (product.stock < quantity)
                {
                    StatusMessageHelper.ShowMessage($"Insufficient stock for {product.prod_name}. Available stock: {product.stock}.", true);
                    return;
                }

                // Calculate total for this line item
                decimal price = product.prod_sales_price ?? 0;
                decimal total = price * quantity;

                // Check if the product is already in the invoice list
                var existingLineItem = _invoiceItems.FirstOrDefault(i => i.prod_code == product.prod_code);
                if (existingLineItem != null)
                {
                    // Update the quantity and total of the existing item
                    existingLineItem.quantity += quantity;
                    existingLineItem.line_total += total;
                }
                else
                {
                    // Create a new InvoiceLineItem and add it to the list
                    var lineItem = new tblInvoiceLine
                    {
                        prod_code = product.prod_code,
                        quantity = quantity,
                        old_price = product.prod_sales_price ?? 0,
                        current_price = product.prod_sales_price ?? 0,
                        line_total = total
                    };
                    _invoiceItems.Add(lineItem);
                }

                // Wrap _invoiceItems for display and set as source for DataGrid
                var wrappedItems = _invoiceItems.Select(i => new Wrapper.InvoiceLineDisplayWrapper(i, product)).ToList();
                InvoiceDataGrid.ItemsSource = wrappedItems;
                InvoiceDataGrid.Items.Refresh(); // Refresh the DataGrid display to reflect changes

                // Clear inputs for the next item
                cmbProduct.SelectedItem = null;
                txtQuantity.Clear();

            }
            else
            {
                StatusMessageHelper.ShowMessage("Please enter valid details.", true);
            }
        }

        private async void CreateInvoice(object sender, RoutedEventArgs e)
        {
            if (_invoiceItems.Count == 0) {
                StatusMessageHelper.ShowMessage("Items can't be empty", true);
                return;
            }

            if (_validatorInvoice.ValidateAll())
            {
                // Create an invoice master object with data from the UI
                var invoice = new tblInvoiceMaster
                {
                    invoice_number = txtInvoiceNumber.Text.Trim(),
                    invoice_date = dpInvoiceDate.SelectedDate.HasValue ? DateOnly.FromDateTime(dpInvoiceDate.SelectedDate.Value) : DateOnly.FromDateTime(DateTime.Now),
                    total_amount = _invoiceItems.Sum(item => item.line_total), // Sum of line items' totals
                    status = 1 // Set default status, for example, "Pending"
                };

                // Call the CreateInvoiceAsync method and handle the result
                bool isInvoiceCreated = await _invoiceService.CreateInvoiceAsync(invoice, _invoiceItems);

                if (isInvoiceCreated)
                {
                    StatusMessageHelper.ShowMessage("Invoice created successfully.", false);

                    // Clear the form and invoice items list
                    txtInvoiceNumber.Clear();
                    dpInvoiceDate.SelectedDate = DateTime.Now;
                    _invoiceItems.Clear();
                    InvoiceDataGrid.ItemsSource = null; // Clear the DataGrid
                    InvoiceDataGrid.Items.Refresh();
                }
            }
            else
            {
                StatusMessageHelper.ShowMessage("Please enter valid details.", true);
            }
        }
    }
}
