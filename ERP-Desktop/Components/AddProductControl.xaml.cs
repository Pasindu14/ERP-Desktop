using ERP_Desktop.DBModels;
using ERP_Desktop.Services;
using ERP_Desktop.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ERP_Desktop.Components
{
    /// <summary>
    /// Interaction logic for AddProductControl.xaml
    /// </summary>
    public partial class AddProductControl : UserControl
    {
        private readonly InputValidator _validator;
        private readonly ProductService _productService;

        public event EventHandler<tblProductMaster>? ProductAdded;

        // Inject ProductService in the constructor
        public AddProductControl()
        {
            InitializeComponent();
            _validator = new InputValidator();
            _validator.RegisterTextBox(txtUserGeneratedCode, "User-Generated Code");
            _validator.RegisterTextBox(txtProductName, "Product Name");
            _validator.RegisterTextBox(txtCostPrice, "Cost Price");
            _validator.RegisterTextBox(txtSalesPrice, "Sales Price");
            _validator.RegisterComboBox(cmbCategory, "Category");

            txtCostPrice.PreviewTextInput += InputHelper.DecimalOnlyTextBox_PreviewTextInput;
            txtSalesPrice.PreviewTextInput += InputHelper.DecimalOnlyTextBox_PreviewTextInput;
            // Create ERPDesktopContext and ProductService here
            var context = new ERPDesktopContext();
            _productService = new ProductService(context);
        }

        private async void AddProduct(object sender, RoutedEventArgs e)
        {
            var userGeneratedCode = txtUserGeneratedCode.Text.Trim();
            var productName = txtProductName.Text.Trim();
            var productDescription = txtProductDescription.Text.Trim();
            var costPrice = decimal.TryParse(txtCostPrice.Text.Trim(), out var cost) ? cost : 0;
            var salesPrice = decimal.TryParse(txtSalesPrice.Text.Trim(), out var sales) ? sales : 0;
            var prodCat = (string)cmbCategory.SelectedValue; // Assuming category selection is by ID
            var prodStatus = chkStatus.IsChecked == true ? 1 : 0;

            if (_validator.ValidateAll())
            {
                AddProductButton.IsEnabled = false;

                try
                {
                    // Create a new product object
                    var newProduct = new tblProductMaster
                    {
                        prod_code_usergen = userGeneratedCode,
                        prod_name = productName,
                        prod_desc = productDescription,
                        prod_cost_price = costPrice,
                        prod_sales_price = salesPrice,
                        prod_cat = prodCat,
                        prod_status = prodStatus
                    };

                    // Insert the product into the database
                    bool isInserted = await _productService.InsertProductAsync(newProduct);
                    AddProductButton.IsEnabled = true;

                    if (isInserted)
                    {
                        // Raise an event that the product was added successfully
                        ProductAdded?.Invoke(this, newProduct);
                        txtUserGeneratedCode.Clear();
                        txtProductName.Clear();
                        txtProductDescription.Clear();
                        txtCostPrice.Clear();
                        txtSalesPrice.Clear();
                        cmbCategory.SelectedIndex = -1;

                        StatusMessageHelper.ShowMessage("Product added successfully.", false);
                    }
                    else
                    {
                        StatusMessageHelper.ShowMessage("Failed to add product. Please try again.", true);
                    }
                }
                catch (Exception ex)
                {
                    AddProductButton.IsEnabled = true;
                    StatusMessageHelper.ShowMessage($"An error occurred: {ex.Message}", true);
                }
            }
            else
            {
                StatusMessageHelper.ShowMessage("Please enter valid product details.", true);
            }
        }


    }
}
