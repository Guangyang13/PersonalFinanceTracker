using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceTracker.Interfaces.Navigation;
using PersonalFinanceTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Services.Navigation
{
    public partial class NavigationService : INavigationService
    {
        private IServiceProvider _serviceProvider = null!;

        public ObservableObject CurrentViewModel { get; set; } = null!;
        public event Action<ObservableObject> OnViewChanged = null!;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ObservableObject
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<TViewModel>();
            OnViewChanged?.Invoke(CurrentViewModel);
        }
    }
}
