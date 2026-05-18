using CommunityToolkit.Mvvm.Input;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly DeezerService _deezerService;

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

        // ViewModels persistants
        public PlayerBarViewModel PlayerBarViewModel { get; }
        public HomeViewModel HomeVM { get; }
        private bool _isPlayerVisible;
        public bool IsPlayerVisible
        {
            get => _isPlayerVisible;
            set { _isPlayerVisible = value; OnPropertyChanged(); }
        }

        // Commandes pour les boutons de la Sidebar
        public IRelayCommand MoveToHomeCommand { get; }
        public IRelayCommand MoveToExploreCommand { get; }
        public IRelayCommand MoveToLibraryCommand { get; }
        public IRelayCommand MoveToProfileCommand { get; }

        // Le constructeur reçoit HomeViewModel et Deezer Service via l'injection de dépendances
        public MainViewModel(HomeViewModel homeVM, DeezerService deezerService, IPlaylistRepository playlistRepository, IUserRepository userRepository, PlayerBarViewModel playerBarViewModel)
        {
            _deezerService = deezerService;
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
            HomeVM = homeVM;
            PlayerBarViewModel = playerBarViewModel;
            PlayerBarViewModel.PlaybackStarted += () =>
            {
                IsPlayerVisible = true;
            };

            // On commence sur le Login au démarrage (ou Home si tu préfères)
            CurrentView = new LoginViewModel(this, _deezerService);

            // Initialisation des commandes
            MoveToHomeCommand = new RelayCommand(() => CurrentView = HomeVM);

            // Navigation vers les autres vues en passant les dépendances nécessaires
            MoveToProfileCommand = new RelayCommand(() => CurrentView = new ProfileViewModel(_userRepository));

            MoveToExploreCommand = new RelayCommand(() => CurrentView = HomeVM);

            MoveToLibraryCommand = new RelayCommand(() => CurrentView = new LibraryViewModel(_playlistRepository));
        }
    }
}