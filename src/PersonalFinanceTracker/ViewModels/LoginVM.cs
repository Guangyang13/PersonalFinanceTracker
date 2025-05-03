using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceTracker.Interfaces.Auth;
using PersonalFinanceTracker.Interfaces.Navigation;
using PersonalFinanceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PersonalFinanceTracker.ViewModels
{
    public partial class LoginVM : ObservableObject
    {
        [ObservableProperty]
        private string _username = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private string _statusMessage = string.Empty;
        [ObservableProperty]
        private Visibility _statusMessageVisibility = Visibility.Collapsed;

        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        public LoginVM(IAuthService authService, INavigationService navigationService)
        {           
            _authService = authService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {

            if (!await _authService.LoginAsync(Username, Password))
            {
                StatusMessage = "Login failed. Please check your credentials and try again.";
                StatusMessageVisibility = Visibility.Visible;
                return;
            }

            StatusMessageVisibility = Visibility.Collapsed;
            _navigationService.NavigateTo<TransactionFormVM>();

        }

        [RelayCommand]
        private void NavigateToRegistration()
        {
            _navigationService.NavigateTo<RegistrationVM>();
        }

    }
}
