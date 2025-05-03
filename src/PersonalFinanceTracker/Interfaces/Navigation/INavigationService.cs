using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Interfaces.Navigation
{
    public interface INavigationService
    {
        ObservableObject CurrentViewModel { get; }
        event Action<ObservableObject> OnViewChanged;

        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }
}
