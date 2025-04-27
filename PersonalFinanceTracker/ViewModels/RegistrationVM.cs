using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceTracker.Interfaces.Auth;
using PersonalFinanceTracker.Interfaces.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PersonalFinanceTracker.ViewModels
{
    public partial class RegistrationVM : ObservableObject
    {
        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private Visibility _statusMessageVisibility = Visibility.Collapsed;

        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;


        public RegistrationVM(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (Username == string.Empty || Password == string.Empty || ConfirmPassword == string.Empty)
            {
                StatusMessage = "Username or Password cannot be empty. Please try again.";
                StatusMessageVisibility = Visibility.Visible;
                return;
            }


            if (Password != ConfirmPassword)
            {
                StatusMessage = "Passwords do not match. Please try again.";
                StatusMessageVisibility = Visibility.Visible;
                return;
            }

            if (!await _authService.IsUserRegisteredAsync(Username))
            {
                StatusMessage = "User already exists. Please try again.";
                StatusMessageVisibility = Visibility.Visible;
                return;
            }


            if (!await _authService.RegisterAsync(Username, Password))
            {
                StatusMessage = "Registration failed. Please try again.";
                StatusMessageVisibility = Visibility.Visible;
                return;
            }
            Cancel();
        }

        [RelayCommand]
        private void Cancel()
        {
            _navigationService.NavigateTo<LoginVM>();
        }


    }
}
