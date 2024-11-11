using ERP_Desktop.DBModels;
using ERP_Desktop.Services;
using ERP_Desktop.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop.Components
{
    public partial class UpdateCategoryControl : UserControl
    {
        private readonly InputValidator _validator;
        private readonly CategoryService _categoryService;
        private tblCategoryMaster? _currentCategory; // Store the current category details
        public event EventHandler<tblCategoryMaster>? CategoryUpdated;

        public UpdateCategoryControl()
        {
            InitializeComponent();
            _validator = new InputValidator();
            _validator.RegisterTextBox(txtUpdateCategoryName, "Category Name");

            // Initialize CategoryService
            var context = new ERPDesktopContext();
            _categoryService = new CategoryService(context);
        }

        // Method to load category details directly
        public void LoadCategoryDetails(tblCategoryMaster category)
        {
            _currentCategory = category;
            txtUpdateCategoryCode.Text = category.cat_code; // Set the code as readonly or static
            txtUpdateCategoryName.Text = category.cat_name;
            chkStatus.IsChecked=category.cat_status==1?true:false;
        }

        // Update the category based on modified details
        private async void UpdateCategory(object sender, RoutedEventArgs e)
        {
            // Update the category name and status from the UI input
            _currentCategory!.cat_name = txtUpdateCategoryName.Text.Trim();
            _currentCategory!.cat_status = chkStatus.IsChecked == true ? 1 : 0;

            // Validate before updating
            if (_validator.ValidateAll())
            {
                UpdateCategoryButton.IsEnabled = false;

                try
                {
                    // Perform update operation
                    bool isUpdated = await _categoryService.UpdateCategoryAsync(_currentCategory);
                    UpdateCategoryButton.IsEnabled = true;

                    if (isUpdated)
                    {
                        StatusMessageHelper.ShowMessage("Category updated successfully.", false);
                        CategoryUpdated?.Invoke(this, _currentCategory);
                    }
                    else
                    {
                        StatusMessageHelper.ShowMessage("Failed to update category. Please try again.", true);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any unexpected errors
                    UpdateCategoryButton.IsEnabled = true;
                    StatusMessageHelper.ShowMessage($"An error occurred: {ex.Message}", true);
                }
            }
            else
            {
                StatusMessageHelper.ShowMessage("Please enter a valid category name.", true);
            }
        }

        // Event to notify parent control of an update

    }
}
