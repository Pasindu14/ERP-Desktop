using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ERP_Desktop.Helpers
{
    public class InputValidator
    {

        private readonly Dictionary<TextBox, string> _textBoxValidations = new Dictionary<TextBox, string>();
        private readonly Dictionary<TextBox, string> _errorMessages = new Dictionary<TextBox, string>(); // Stores error messages

        public InputValidator()
        {

        }

        public void RegisterTextBox(TextBox textBox, string fieldName)
        {
            if (!_textBoxValidations.ContainsKey(textBox))
            {
                _textBoxValidations.Add(textBox, fieldName);
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        public void UnregisterTextBox(TextBox textBox)
        {
            if (_textBoxValidations.ContainsKey(textBox))
            {
                textBox.TextChanged -= TextBox_TextChanged;
                _textBoxValidations.Remove(textBox);
                _errorMessages.Remove(textBox);
            }
        }

        public bool ValidateAll()
        {
            bool isValid = true;
            foreach (var textBox in _textBoxValidations.Keys)
            {
                if (!ValidateTextBox(textBox))
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public bool ValidateTextBox(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text) ||
                (_errorMessages.ContainsKey(textBox) && textBox.Text == _errorMessages[textBox]))
            {
                ShowNotification($"{_textBoxValidations[textBox]} cannot be empty!", false);
                ShowError(textBox, $"{_textBoxValidations[textBox]} cannot be empty!");
                return false;
            }
            return true;
        }

        private void ShowError(TextBox textBox, string message)
        {
            _errorMessages[textBox] = message; // Store error message for the TextBox
            textBox.Text = message;
            textBox.Foreground = Brushes.Red;
            textBox.GotFocus += ClearErrorOnFocus;
            textBox.BorderBrush = Brushes.Red;
            textBox.BorderThickness = new Thickness(2);
        }

        private void ClearErrorOnFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (_errorMessages.ContainsKey(textBox) && textBox.Text == _errorMessages[textBox])
                {
                    textBox.Clear(); // Only clear if it’s showing the error message
                }
                textBox.ClearValue(TextBox.ForegroundProperty);
                textBox.GotFocus -= ClearErrorOnFocus;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.ClearValue(TextBox.BorderBrushProperty);
                textBox.ClearValue(TextBox.BorderThicknessProperty);
                textBox.ClearValue(TextBox.ForegroundProperty);
                textBox.GotFocus -= ClearErrorOnFocus;
            }
        }

        public void ShowNotification(string message, bool isError)
        {
            StatusMessageHelper.ShowMessage(message, isError: isError);
        }
    }
}
