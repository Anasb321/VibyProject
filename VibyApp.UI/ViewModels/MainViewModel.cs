using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private HomeViewModel? _homeVM;

        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public PlayerBarViewModel PlayerBarViewModel { get; }

        public HomeViewModel HomeVM => _homeVM ??= _serviceProvider.GetRequiredService<HomeViewModel>();

        public IRelayCommand MoveToHomeCommand { get; }
        public IRelayCommand MoveToExploreCommand { get; }
        public IRelayCommand MoveToLibraryCommand { get; }
        public IRelayCommand MoveToProfileCommand { get; }
        public IRelayCommand MoveToLikedCommand { get; }

        public MainViewModel(
            PlayerBarViewModel playerBarViewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            PlayerBarViewModel = playerBarViewModel;

            _navigationService.AttachShell(this);

            MoveToHomeCommand = new RelayCommand(_navigationService.NavigateToHome);
            MoveToProfileCommand = new RelayCommand(_navigationService.NavigateToProfile);
            MoveToExploreCommand = new RelayCommand(_navigationService.NavigateToHome);
            MoveToLibraryCommand = new RelayCommand(_navigationService.NavigateToLibrary);
            MoveToLikedCommand = new RelayCommand(_navigationService.NavigateToLiked);
        }

        public void ShowInitialView() => _navigationService.NavigateToLogin();

        internal void SetCurrentView(object view) => CurrentView = view;
    }
}
