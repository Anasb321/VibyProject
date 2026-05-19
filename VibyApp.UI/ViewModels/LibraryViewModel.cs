using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<Playlist> _playlists = new();
        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set
            {
                _playlists = value;
                OnPropertyChanged();
            }
        }

        private Playlist _currentPlaylist = new();
        public Playlist CurrentPlaylist
        {
            get => _currentPlaylist;
            set
            {
                _currentPlaylist = value;
                OnPropertyChanged();
            }
        }

        private bool _isModalOpen;
        public bool IsModalOpen
        {
            get => _isModalOpen;
            set
            {
                _isModalOpen = value;
                OnPropertyChanged();
            }
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public IRelayCommand OpenCreatePlaylistCommand { get; }
        public IRelayCommand SavePlaylistCommand { get; }
        public IRelayCommand CloseModalCommand { get; }
        public IRelayCommand SelectImageCommand { get; }
        public IRelayCommand<Playlist> DeletePlaylistCommand { get; }
        public IRelayCommand<Playlist> OpenPlaylistCommand { get; }

        public LibraryViewModel(
            IServiceScopeFactory scopeFactory,
            ICurrentUserService currentUserService,
            INavigationService navigationService)
        {
            _scopeFactory = scopeFactory;
            _currentUserService = currentUserService;
            _navigationService = navigationService;

            OpenCreatePlaylistCommand = new RelayCommand(OpenCreatePlaylist);
            CloseModalCommand = new RelayCommand(CloseModal);
            SavePlaylistCommand = new RelayCommand(async () => await SavePlaylist());
            SelectImageCommand = new RelayCommand(SelectImage);
            DeletePlaylistCommand = new RelayCommand<Playlist>(async (p) => await DeletePlaylist(p));
            OpenPlaylistCommand = new RelayCommand<Playlist>(OpenPlaylist);

            _ = LoadUserPlaylistsAsync();
        }

        private async Task LoadUserPlaylistsAsync()
        {
            if (_currentUserService.CurrentUser == null)
                return;

            using var scope = _scopeFactory.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
            var data = await playlistRepository.GetAllByUserIdAsync(_currentUserService.CurrentUser.Id);

            Playlists.Clear();
            foreach (var playlist in data)
                Playlists.Add(playlist);
        }

        private void OpenCreatePlaylist()
        {
            CurrentPlaylist = new Playlist
            {
                UserId = _currentUserService.CurrentUser?.Id ?? 0
            };

            ErrorMessage = "";
            IsModalOpen = true;
        }

        private async Task SavePlaylist()
        {
            if (IsSaving) return;

            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(CurrentPlaylist.Name))
            {
                ErrorMessage = "Le nom est obligatoire";
                return;
            }

            bool exists = Playlists.Any(p =>
                p.Name.Trim().ToLower() ==
                CurrentPlaylist.Name.Trim().ToLower());

            if (exists)
            {
                ErrorMessage = "Une playlist avec ce nom existe déjà";
                return;
            }

            try
            {
                IsSaving = true;

                using var scope = _scopeFactory.CreateScope();
                var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
                await playlistRepository.AddAsync(CurrentPlaylist);
                Playlists.Add(CurrentPlaylist);

                IsModalOpen = false;
                CurrentPlaylist = new Playlist();
            }
            finally
            {
                IsSaving = false;
            }
        }

        private async Task DeletePlaylist(Playlist? playlist)
        {
            if (playlist == null)
                return;

            using var scope = _scopeFactory.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
            await playlistRepository.DeleteAsync(playlist.Id);
            Playlists.Remove(playlist);
        }

        private void OpenPlaylist(Playlist? playlist)
        {
            if (playlist == null)
                return;

            _navigationService.NavigateToPlaylistDetail(playlist);
        }

        private void SelectImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Images (*.png;*.jpg)|*.png;*.jpg"
            };

            if (dialog.ShowDialog() == true)
            {
                CurrentPlaylist.PlaylistPicture = dialog.FileName;
                OnPropertyChanged(nameof(CurrentPlaylist));
            }
        }

        private void CloseModal()
        {
            IsModalOpen = false;
            ErrorMessage = "";
        }
    }
}