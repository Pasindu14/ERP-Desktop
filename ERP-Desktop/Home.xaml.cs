using ControlzEx.Standard;
using ERP_Desktop.Components;
using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using ERP_Desktop.Services;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP_Desktop
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : MetroWindow
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public Home()
        {
            InitializeComponent();
            var context = new ERPDesktopContext();
            _categoryService = new CategoryService(context);
            _productService = new ProductService(context);
            Loaded += Home_Loaded;
            Status.CloseButtonVisibility = Visibility.Hidden;
        }

        private async void Home_Loaded(object sender, RoutedEventArgs e)
        {
            CategoryDataGrid.ItemsSource = await _categoryService.FetchAllCategoriesAsync(); //
            ProductDataGrid.ItemsSource = await _productService.FetchAllProductsAsync();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Show the input form panel
            InputFormPanel.Visibility = Visibility.Visible;
            txtCategory.Clear();
        }

        private void ToggleAddCategoryForm_Click(object sender, RoutedEventArgs e)
        {
            // Create and set up the AddCategoryControl
            var addCategoryControl = new AddCategoryControl();

            // Subscribe to the CategoryAdded event
            addCategoryControl.CategoryAdded += OnCategoryAdded!;

            // Set the content of the AddCategoryFlyout
            ManageCategoryFlyout.Content = addCategoryControl;
            ManageCategoryFlyout.Header = "Add New Category";
            ManageCategoryFlyout.IsOpen = true;
        }

        private async void OnCategoryAdded(object? sender, tblCategoryMaster newCategory)
        {
            // Close the flyout
            ManageCategoryFlyout.IsOpen = false;

            // Refresh the DataGrid
            await LoadCategories();

            // Unsubscribe from the event to prevent memory leaks
            if (sender is AddCategoryControl addControl)
            {
                addControl.CategoryAdded -= OnCategoryAdded!;
            }
        }

        private void ToggleUpdateCategoryForm_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var category = (tblCategoryMaster)button.DataContext;

            // Create and set up the UpdateCategoryControl
            var updateCategoryControl = new UpdateCategoryControl();
            updateCategoryControl.LoadCategoryDetails(category);

            // Subscribe to the CategoryUpdated event
            updateCategoryControl.CategoryUpdated += OnCategoryUpdated;

            // Set the content of the UpdateCategoryFlyout
            ManageCategoryFlyout.Content = updateCategoryControl;
            ManageCategoryFlyout.Header = "Update Category";
            ManageCategoryFlyout.IsOpen = true;
        }

        private async void OnCategoryUpdated(object? sender, tblCategoryMaster updatedCategory)
        {
            // Close the flyout
            ManageCategoryFlyout.IsOpen = false;

            // Refresh the DataGrid
            await LoadCategories(); // Assuming you have a method to reload the categories

            // Unsubscribe from the event to prevent memory leaks
            if (sender is UpdateCategoryControl updateControl)
            {
                updateControl.CategoryUpdated -= OnCategoryUpdated;
            }
        }

        private async Task LoadCategories()
        {
            using (var context = new ERPDesktopContext())
            {
                var categoryService = new CategoryService(context);
                var categories = await categoryService.FetchAllCategoriesAsync();
                CategoryDataGrid.ItemsSource = categories;
            }
        }

        // Method to toggle the Add Product form
        private async void ToggleAddProductForm_Click(object sender, RoutedEventArgs e)
        {
            var addProductControl = new AddProductControl();

            // Subscribe to the ProductAdded event
            addProductControl.ProductAdded += OnProductAdded!;
            addProductControl.cmbCategory.ItemsSource = await _categoryService.FetchAllCategoriesAsync();

            // Set the content of the ManageProductFlyout
            ManageProductFlyout.Content = addProductControl;
            ManageProductFlyout.Header = "Add New Product";
            ManageProductFlyout.IsOpen = true;
        }

        // Event handler for when a product is added
        private async void OnProductAdded(object? sender, tblProductMaster newProduct)
        {
            // Close the flyout after adding the product
            ManageProductFlyout.IsOpen = false;

            // Refresh the product list
            await LoadProducts();

            // Unsubscribe from the event to prevent memory leaks
            if (sender is AddProductControl addControl)
            {
                addControl.ProductAdded -= OnProductAdded!;
            }
        }

        // Method to toggle the Update Product form
        private async void ToggleUpdateProductForm_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var product = (tblProductMaster)button.DataContext;

            // Create and set up the UpdateProductControl
            var updateProductControl = new UpdateProductControl();

            // Fetch all categories to populate the ComboBox in the UpdateProductControl
            var categories = await _categoryService.FetchAllCategoriesAsync();
            updateProductControl.LoadProductDetails(product, categories);

            // Subscribe to the ProductUpdated event
            updateProductControl.ProductUpdated += OnProductUpdated;

            // Set the content of the ManageProductFlyout for updating a product
            ManageProductFlyout.Content = updateProductControl;
            ManageProductFlyout.Header = "Update Product";
            ManageProductFlyout.IsOpen = true;
        }
        // Event handler for when a product is updated
        private async void OnProductUpdated(object? sender, tblProductMaster updatedProduct)
        {
            // Close the flyout after updating the product
            ManageProductFlyout.IsOpen = false;

            // Refresh the product list
            await LoadProducts();

            // Unsubscribe from the event to prevent memory leaks
            if (sender is UpdateProductControl updateControl)
            {
                updateControl.ProductUpdated -= OnProductUpdated;
            }
        }
        // Method to reload the products into the ProductDataGrid
        private async Task LoadProducts()
        {
            var products = await _productService.FetchAllProductsAsync();
            ProductDataGrid.ItemsSource = products;
        }

    }



}
