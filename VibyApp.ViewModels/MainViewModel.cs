using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VibyApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private object _currentView = null!;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            CurrentView = new LoginViewModel(this);
        }

        public void NavigateToHome()
        {
            CurrentView = new HomeViewModel();
        }
        public void NavigateToProfile()
        {
            CurrentView = new ProfileViewModel();
        }
    }
}