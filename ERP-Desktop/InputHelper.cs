using System.Windows.Controls;
using System.Windows.Input;

namespace ERP_Desktop.Helpers
{
    public static class InputHelper
    {
        public static void DecimalOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Allow only one decimal point
                if (e.Text == "." && textBox.Text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }

                // Allow only numbers and a decimal point
                e.Handled = !char.IsDigit(e.Text, 0) && e.Text != ".";
            }
        }
    }
}
