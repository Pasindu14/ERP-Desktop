using ERP_Desktop.Helpers;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP_Desktop.Components
{
    /// <summary>
    /// Interaction logic for AddCategoryControl.xaml
    /// </summary>
    public partial class AddCategoryControl : UserControl
    {

        public AddCategoryControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = txtNewCategory.Text.Trim();
            if (!string.IsNullOrEmpty(categoryName))
            {
                CategoryAdded?.Invoke(this, categoryName); // Trigger event with new category name
                txtNewCategory.Clear();
                StatusMessageHelper.ShowMessage("Category added successfully!", isError: false);
            }
            else
            {
                StatusMessageHelper.ShowMessage("Category name cannot be empty.", isError: true);
            }
        }

        public event EventHandler<string>? CategoryAdded;
    }
}
