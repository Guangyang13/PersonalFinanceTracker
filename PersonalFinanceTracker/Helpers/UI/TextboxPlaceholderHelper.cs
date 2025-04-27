using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PersonalFinanceTracker.Helpers.UI
{
    public class TextboxPlaceholderHelper
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached("PlaceholderText", typeof(string), typeof(TextboxPlaceholderHelper), new PropertyMetadata("", OnPlaceholderTextChanged));

        public static string GetPlaceholderText(DependencyObject obj) => (string)obj.GetValue(PlaceholderProperty);
        public static void SetPlaceholderText(DependencyObject obj, string value) => obj.SetValue(PlaceholderProperty, value);

        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textbox)
            {
                textbox.GotFocus -= RemovePlaceholder;
                textbox.LostFocus -= ShowPlaceholder;

                textbox.GotFocus += RemovePlaceholder;
                textbox.LostFocus += ShowPlaceholder;

                ShowPlaceholder(textbox, null);
            }
        }

        private static void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private static void ShowPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = GetPlaceholderText(textBox);
                textBox.Foreground = Brushes.Gray;
            }
        }
    }
}
