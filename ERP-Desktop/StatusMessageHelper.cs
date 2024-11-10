using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ERP_Desktop.Helpers
{
    public static class StatusMessageHelper
    {
        public static void ShowMessage(string message, bool isError = false)
        {
            // Get the current MainWindow instance
            if (Application.Current.MainWindow is Home home)
            {
                // Create TextBlock for message with appropriate styling based on success or error
                var messageTextBlock = new TextBlock
                {
                    Text = message,
                    FontSize = 10,
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,

                };

                // Update the Status flyout content with the message
                home.Status.Content = messageTextBlock;

                // Set header to indicate the message type
                home.Status.Background= Brushes.Red;

                // Open the flyout
                home.Status.IsOpen = true;
            }
        }
    }
}
