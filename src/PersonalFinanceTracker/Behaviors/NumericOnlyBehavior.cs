using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace PersonalFinanceTracker.Behaviors
{
    public class NumericOnlyBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }

        private void AssociatedObject_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != "." 
                || (e.Text == "." && AssociatedObject.Text.Contains("."))
                || (AssociatedObject.Text.Contains(".") 
                && AssociatedObject.Text.IndexOf(".") < AssociatedObject.CaretIndex
                && AssociatedObject.Text.Length - AssociatedObject.Text.IndexOf(".") - 1 >= 2))
            {
                e.Handled = true;
            }
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                if (AssociatedObject.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = false;
            }
            else if (e.Key < Key.D0 && e.Key > Key.D9)
            {
                e.Handled = true;
            }
        }
    }
}
