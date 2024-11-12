using ERP_Desktop.DBModels;
using ERP_Desktop.Services;
using ERP_Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop.Components
{


    public partial class UpdateProductControl : UserControl
    {
        private readonly InputValidator _validator;
        private readonly ProductService _productService;
        private tblProductMaster? _currentProduct; // Store the current product details
        public event EventHandler<tblProductMaster>? ProductUpdated;

        public UpdateProductControl()
        {
            InitializeComponent();
            _validator = new InputValidator();

            // Register textboxes for validation
            _validator.RegisterTextBox(txtUpdateUserGeneratedCode, "User-Generated Code");
            _validator.RegisterTextBox(txtUpdateProductName, "Product Name");
            _validator.RegisterTextBox(txtUpdateCostPrice, "Cost Price");
            _validator.RegisterTextBox(txtUpdateSalesPrice, "Sales Price");
            txtUpdateCostPrice.PreviewTextInput += InputHelper.DecimalOnlyTextBox_PreviewTextInput;
            txtUpdateSalesPrice.PreviewTextInput += InputHelper.DecimalOnlyTextBox_PreviewTextInput;
            // Initialize ProductService
            var context = new ERPDesktopContext();
            _productService = new ProductService(context);
        }

        // Method to load product details directly
        public void LoadProductDetails(tblProductMaster product, IEnumerable<tblCategoryMaster> categories)
        {
            _currentProduct = product;

            txtUpdateUserGeneratedCode.Text = product.prod_code_usergen;
            txtUpdateProductName.Text = product.prod_name;
            txtUpdateProductDescription.Text = product.prod_desc;
            txtUpdateCostPrice.Text = product.prod_cost_price.ToString();
            txtUpdateSalesPrice.Text = product.prod_sales_price.ToString();
            chkProductStatus.IsChecked = product.prod_status == 1;

            // Bind categories to ComboBox and set selected category
            var categoryWrappers = categories.Select(c => new Wrapper.CategoryDisplayWrapper(c));
            cmbUpdateCategory.ItemsSource = categoryWrappers;
            cmbUpdateCategory.SelectedValue = product.prod_cat;
        }

        // Update the product based on modified details
        private async void UpdateProduct(object sender, RoutedEventArgs e)
        {
            // Update the product properties from the UI input
            _currentProduct!.prod_code_usergen = txtUpdateUserGeneratedCode.Text.Trim();
            _currentProduct!.prod_name = txtUpdateProductName.Text.Trim();
            _currentProduct!.prod_desc = txtUpdateProductDescription.Text.Trim();
            _currentProduct!.prod_cost_price = decimal.TryParse(txtUpdateCostPrice.Text.Trim(), out var costPrice) ? costPrice : 0;
            _currentProduct!.prod_sales_price = decimal.TryParse(txtUpdateSalesPrice.Text.Trim(), out var salesPrice) ? salesPrice : 0;
            _currentProduct!.prod_cat = (string)cmbUpdateCategory.SelectedValue;
            _currentProduct!.prod_status = chkProductStatus.IsChecked == true ? 1 : 0;

            // Validate before updating
            if (_validator.ValidateAll())
            {
                UpdateProductButton.IsEnabled = false;

                try
                {
                    // Perform update operation
                    bool isUpdated = await _productService.UpdateProductAsync(_currentProduct);
                    UpdateProductButton.IsEnabled = true;

                    if (isUpdated)
                    {
                        StatusMessageHelper.ShowMessage("Product updated successfully.", false);
                        ProductUpdated?.Invoke(this, _currentProduct);
                    }
                    else
                    {
                        StatusMessageHelper.ShowMessage("Failed to update product. Please try again.", true);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any unexpected errors
                    UpdateProductButton.IsEnabled = true;
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
