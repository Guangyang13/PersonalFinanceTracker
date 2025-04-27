using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PersonalFinanceTracker.Behaviors
{
    public class BindablePassowdBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty BindablePasswordProperty = DependencyProperty.Register("BindablePassword", typeof(string), typeof(BindablePassowdBehavior), 
            new PropertyMetadata(string.Empty));

        public string BindablePassword
        {
            get { return (string)GetValue(BindablePasswordProperty); }
            set { SetValue(BindablePasswordProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        { 
            BindablePassword = AssociatedObject.Password;
        }
    }
}
