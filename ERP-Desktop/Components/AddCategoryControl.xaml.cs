using ERP_Desktop.DBModels;
using ERP_Desktop.Services;
using ERP_Desktop.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop.Components
{
    /// <summary>
    /// Interaction logic for AddCategoryControl.xaml
    /// </summary>
    public partial class AddCategoryControl : UserControl
    {
        private readonly InputValidator _validator;
        private readonly CategoryService _categoryService;

        public event EventHandler<tblCategoryMaster>? CategoryAdded;
        // Inject CategoryService in the constructor
        public AddCategoryControl()
        {
            InitializeComponent();
            _validator = new InputValidator();
            _validator.RegisterTextBox(txtCategoryCode, "Category Code");
            _validator.RegisterTextBox(txtCategoryName, "Category Name");

            // Create ERPDesktopContext and CategoryService here
            var context = new ERPDesktopContext();
            _categoryService = new CategoryService(context);
        }

        private async void AddCategory(object sender, RoutedEventArgs e)
        {
            var categoryName = txtCategoryName.Text.Trim();
            var categoryCode = txtCategoryCode.Text.Trim();
            var catStatus = chkStatus.IsChecked == true ? 1 : 0;

            if (_validator.ValidateAll())
            {
                AddCategoryButton.IsEnabled = false;

                try
                {
                    // Create a new category object
                    var newCategory = new tblCategoryMaster
                    {
                        cat_code = categoryCode, // Assuming cat_code is a unique string identifier
                        cat_name = categoryName,
                        cat_status = catStatus // Set a default status
                    };

                    // Insert the category into the database
                    bool isInserted = await _categoryService.InsertCategoryAsync(newCategory);
                    AddCategoryButton.IsEnabled = true;

                    if (isInserted)
                    {
                        // Raise an event that the category was added successfully
                        CategoryAdded?.Invoke(this, newCategory);
                        txtCategoryCode.Clear();
                        txtCategoryName.Clear();

                        StatusMessageHelper.ShowMessage("Category added successfully.", false);
                    }
                    else
                    {
                        StatusMessageHelper.ShowMessage("Failed to add category. Please try again.", true);
                    }
                }
                catch (Exception ex)
                {
                    AddCategoryButton.IsEnabled = true;
                    StatusMessageHelper.ShowMessage($"An error occurred: {ex.Message}", true);
                }
            }
            else
            {
                StatusMessageHelper.ShowMessage("Please enter a valid category name.", true);
            }
        }
    }
}
