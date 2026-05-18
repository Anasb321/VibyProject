using VibyApp.DB.Repository;

namespace VibyApp.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;
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

        public MainViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            CurrentView = new LoginViewModel(this);
        }

        public void NavigateToHome()
        {
            CurrentView = new HomeViewModel();
        }

        public void NavigateToProfile()
        {
            CurrentView = new ProfileViewModel(_userRepository);
        }
    }
}