using ERP_Desktop.Helpers;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ERP_Desktop
{
    public partial class MainWindow : MetroWindow
    {
        private readonly InputValidator _validator;
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            _validator = new InputValidator(Status);
            CategoryDataGrid.ItemsSource = Categories; //
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _validator.RegisterTextBox(txtCategory, "Category Name");

            bool asd=_validator.ValidateAll();
            if (_validator.ValidateAll())
            {
                Console.WriteLine(txtCategory.Text);
                // Process the valid data here
                _validator.ShowNotification("Data saved successfully!", false);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _validator.UnregisterTextBox(txtCategory);
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
            AddCategoryFlyout.IsOpen = !AddCategoryFlyout.IsOpen;
        }

        private void OnCategoryAdded(object sender, string newCategory)
        {
            Categories.Add(newCategory); // Add new category to collection, DataGrid updates automatically
            Status.IsOpen = false;
        }

        // Method to show status message in Status flyout
        public void ShowStatusMessage(string message)
        {
            // Update the Status flyout content with the provided message
            Status.Content = new TextBlock
            {
                Text = message,
                FontSize = 10,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Open the flyout
            Status.IsOpen = true;
        }
    }
}
