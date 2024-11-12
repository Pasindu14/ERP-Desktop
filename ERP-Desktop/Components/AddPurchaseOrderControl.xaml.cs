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
    public partial class AddPurchaseOrderControl : UserControl
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly PurchaseOrderService _purchaseOrderService;

        // List to hold the purchase order line items
        private List<tblPurchaseOrderLine> _purchaseOrderItems;
        private readonly InputValidator _validatorProduct;
        private readonly InputValidator _validatorOrder;

        public AddPurchaseOrderControl()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _categoryService = new CategoryService(context);
            _productService = new ProductService(context);
            _purchaseOrderService = new PurchaseOrderService(context);

            txtQuantity.PreviewTextInput += InputHelper.DecimalOnlyTextBox_PreviewTextInput;
            _purchaseOrderItems = new List<tblPurchaseOrderLine>();
            Loaded += AddPurchaseOrderControl_Loaded;

            _validatorProduct = new InputValidator();
            _validatorOrder = new InputValidator();
            _validatorProduct.RegisterTextBox(txtQuantity, "Quantity");
            _validatorProduct.RegisterComboBox(cmbProduct, "Product");

            _validatorOrder.RegisterTextBox(txtPurchaseOrderNumber, "Purchase Order Number");
            _validatorOrder.RegisterDatePicker(dpPurchaseOrderDate, "Date");
        }

        private async Task LoadProducts()
        {
            var products = await _productService.FetchAllProductsAsync();
            var productWrapper = products.Select(c => new Wrapper.ProductDisplayWrapper(c));
            cmbProduct.ItemsSource = productWrapper;
        }

        private async void AddPurchaseOrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProducts();
        }

        private async void AddProductItemToOrder(object sender, RoutedEventArgs e)
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

                // Calculate total for this line item
                decimal price = product.prod_cost_price ?? 0;
                decimal total = price * quantity;

                // Check if the product is already in the purchase order list
                var existingLineItem = _purchaseOrderItems.FirstOrDefault(i => i.prod_code == product.prod_code);
                if (existingLineItem != null)
                {
                    // Update the quantity and total of the existing item
                    existingLineItem.quantity += quantity;
                    existingLineItem.line_total += total;
                }
                else
                {
                    // Create a new PurchaseOrderLine item and add it to the list
                    var lineItem = new tblPurchaseOrderLine
                    {
                        prod_code = product.prod_code,
                        quantity = quantity,
                        unit_price = product.prod_cost_price ?? 0,
                        line_total = total
                    };
                    _purchaseOrderItems.Add(lineItem);
                }

                // Wrap _purchaseOrderItems for display and set as source for DataGrid
                var wrappedItems = _purchaseOrderItems.Select(i => new Wrapper.PurchaseOrderLineDisplayWrapper(i, product)).ToList();
                PurchaseOrderDataGrid.ItemsSource = wrappedItems;
                PurchaseOrderDataGrid.Items.Refresh(); // Refresh the DataGrid display to reflect changes

                // Clear inputs for the next item
                cmbProduct.SelectedItem = null;
                txtQuantity.Clear();
            }
            else
            {
                StatusMessageHelper.ShowMessage("Please enter valid details.", true);
            }
        }

        private async void CreatePurchaseOrder(object sender, RoutedEventArgs e)
        {
            if (_purchaseOrderItems.Count == 0)
            {
                StatusMessageHelper.ShowMessage("Items can't be empty", true);
                return;
            }

            if (_validatorOrder.ValidateAll())
            {
                // Create a purchase order master object with data from the UI
                var purchaseOrder = new tblPurchaseOrderMaster
                {
                    purchase_order_number = txtPurchaseOrderNumber.Text.Trim(),
                    purchase_order_date = dpPurchaseOrderDate.SelectedDate.HasValue ? DateOnly.FromDateTime(dpPurchaseOrderDate.SelectedDate.Value) : DateOnly.FromDateTime(DateTime.Now),
                    total_amount = _purchaseOrderItems.Sum(item => item.line_total), // Sum of line items' totals
                    status = 1// Set default status, for example, "Pending"
                };

                // Call the CreatePurchaseOrderAsync method and handle the result
                bool isPurchaseOrderCreated = await _purchaseOrderService.CreatePurchaseOrderAsync(purchaseOrder, _purchaseOrderItems);

                if (isPurchaseOrderCreated)
                {
                    StatusMessageHelper.ShowMessage("Purchase order created successfully.", false);

                    // Clear the form and purchase order items list
                    txtPurchaseOrderNumber.Clear();
                    dpPurchaseOrderDate.SelectedDate = DateTime.Now;
                    _purchaseOrderItems.Clear();
                    PurchaseOrderDataGrid.ItemsSource = null; // Clear the DataGrid
                    PurchaseOrderDataGrid.Items.Refresh();
                }
            }
            else
            {
                StatusMessageHelper.ShowMessage("Please enter valid details.", true);
            }
        }
    }
}
