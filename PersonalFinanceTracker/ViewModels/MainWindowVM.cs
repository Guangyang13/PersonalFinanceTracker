using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceTracker.Interfaces.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceTracker.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        private string _title = "Personal Finance Tracker";

        [ObservableProperty]
        private ObservableObject _currentView = null!;

        private INavigationService _navigationService;

        public MainWindowVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.OnViewChanged += (viewModel) => CurrentView = viewModel;

            _navigationService.NavigateTo<LoginVM>();
        }

    }
}
