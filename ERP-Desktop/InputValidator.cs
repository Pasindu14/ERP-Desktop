using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ERP_Desktop.Helpers
{
    public class InputValidator
    {
        private readonly Dictionary<TextBox, string> _textBoxValidations = new();
        private readonly Dictionary<ComboBox, string> _comboBoxValidations = new();
        private readonly Dictionary<DatePicker, string> _datePickerValidations = new(); // DatePicker validations
        private readonly Dictionary<Control, string> _errorMessages = new();

        public void RegisterTextBox(TextBox textBox, string fieldName)
        {
            if (!_textBoxValidations.ContainsKey(textBox))
            {
                _textBoxValidations.Add(textBox, fieldName);
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        public void RegisterComboBox(ComboBox comboBox, string fieldName)
        {
            if (!_comboBoxValidations.ContainsKey(comboBox))
            {
                _comboBoxValidations.Add(comboBox, fieldName);
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
            }
        }

        public void RegisterDatePicker(DatePicker datePicker, string fieldName)
        {
            if (!_datePickerValidations.ContainsKey(datePicker))
            {
                _datePickerValidations.Add(datePicker, fieldName);
                datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
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

        public void UnregisterComboBox(ComboBox comboBox)
        {
            if (_comboBoxValidations.ContainsKey(comboBox))
            {
                comboBox.SelectionChanged -= ComboBox_SelectionChanged;
                _comboBoxValidations.Remove(comboBox);
                _errorMessages.Remove(comboBox);
            }
        }

        public void UnregisterDatePicker(DatePicker datePicker)
        {
            if (_datePickerValidations.ContainsKey(datePicker))
            {
                datePicker.SelectedDateChanged -= DatePicker_SelectedDateChanged;
                _datePickerValidations.Remove(datePicker);
                _errorMessages.Remove(datePicker);
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

            foreach (var comboBox in _comboBoxValidations.Keys)
            {
                if (!ValidateComboBox(comboBox))
                {
                    isValid = false;
                }
            }

            foreach (var datePicker in _datePickerValidations.Keys)
            {
                if (!ValidateDatePicker(datePicker))
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
                ShowError(textBox, $"{_textBoxValidations[textBox]} cannot be empty!");
                return false;
            }
            return true;
        }

        public bool ValidateComboBox(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null ||
                (_errorMessages.ContainsKey(comboBox) && comboBox.Text == _errorMessages[comboBox]))
            {
                ShowError(comboBox, $"{_comboBoxValidations[comboBox]} must be selected!");
                return false;
            }
            return true;
        }

        public bool ValidateDatePicker(DatePicker datePicker)
        {
            if (!datePicker.SelectedDate.HasValue ||
                (_errorMessages.ContainsKey(datePicker) && datePicker.SelectedDate == null))
            {
                ShowError(datePicker, $"{_datePickerValidations[datePicker]} must be selected!");
                return false;
            }
            return true;
        }

        private void ShowError(Control control, string message)
        {
            _errorMessages[control] = message;
            control.ToolTip = message;
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(2);

            if (control is TextBox textBox)
            {
                textBox.Text = message;
                textBox.Foreground = Brushes.Red;
                textBox.GotFocus += ClearErrorOnFocus;
            }
            else if (control is ComboBox comboBox || control is DatePicker datePicker)
            {
                control.GotFocus += ClearErrorOnFocus;
            }
        }

        private void ClearErrorOnFocus(object sender, RoutedEventArgs e)
        {
            if (sender is Control control)
            {
                control.ClearValue(Border.BorderBrushProperty);
                control.ClearValue(Border.BorderThicknessProperty);

                if (control is TextBox textBox)
                {
                    textBox.Clear();
                    textBox.ClearValue(TextBox.ForegroundProperty);
                }

                control.GotFocus -= ClearErrorOnFocus;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.ClearValue(Border.BorderBrushProperty);
                textBox.ClearValue(Border.BorderThicknessProperty);
                textBox.ClearValue(TextBox.ForegroundProperty);
                textBox.GotFocus -= ClearErrorOnFocus;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                comboBox.ClearValue(Border.BorderBrushProperty);
                comboBox.ClearValue(Border.BorderThicknessProperty);
                comboBox.GotFocus -= ClearErrorOnFocus;
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                datePicker.ClearValue(Border.BorderBrushProperty);
                datePicker.ClearValue(Border.BorderThicknessProperty);
                datePicker.GotFocus -= ClearErrorOnFocus;
            }
        }

        public void ShowNotification(string message, bool isError)
        {
            StatusMessageHelper.ShowMessage(message, isError: isError);
        }
    }
}
