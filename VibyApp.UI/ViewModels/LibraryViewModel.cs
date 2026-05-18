using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;
using VibyProject;

namespace VibyApp.UI.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        private readonly IPlaylistRepository _playlistRepository;

        // LISTE DES PLAYLISTS
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

        // PLAYLIST EN COURS (MODAL)
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

        // MODAL STATE
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

        // LOADING STATE (SAVE)
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

        // ERROR MESSAGE
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

        // COMMANDS
        public IRelayCommand OpenCreatePlaylistCommand { get; }
        public IRelayCommand SavePlaylistCommand { get; }
        public IRelayCommand CloseModalCommand { get; }
        public IRelayCommand SelectImageCommand { get; }

        public IRelayCommand<Playlist> DeletePlaylistCommand { get; }
        public IRelayCommand<Playlist> OpenPlaylistCommand { get; }

        public LibraryViewModel(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;

            // UI
            OpenCreatePlaylistCommand = new RelayCommand(OpenCreatePlaylist);
            CloseModalCommand = new RelayCommand(CloseModal);
            SavePlaylistCommand = new RelayCommand(async () => await SavePlaylist());
            SelectImageCommand = new RelayCommand(SelectImage);

            // Playlist actions
            DeletePlaylistCommand = new RelayCommand<Playlist>(async (p) => await DeletePlaylist(p));
            OpenPlaylistCommand = new RelayCommand<Playlist>(OpenPlaylist);

            _ = LoadUserPlaylistsAsync();
        }

        // LOAD DATA
        private async Task LoadUserPlaylistsAsync()
        {
            var data = await _playlistRepository.GetAllByUserIdAsync(1);

            Playlists.Clear();

            foreach (var playlist in data)
                Playlists.Add(playlist);
        }

        // OPEN MODAL
        private void OpenCreatePlaylist()
        {
            CurrentPlaylist = new Playlist
            {
                UserId = 1
            };

            ErrorMessage = "";
            IsModalOpen = true;
        }

        // SAVE PLAYLIST
        private async Task SavePlaylist()
        {
            if (IsSaving) return;

            // Reset erreur
            ErrorMessage = "";

            // VALIDATION 1 : NOM VIDE
            if (string.IsNullOrWhiteSpace(CurrentPlaylist.Name))
            {
                ErrorMessage = "Le nom est obligatoire";
                return;
            }

            // VALIDATION 2 : DOUBLON
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

                await _playlistRepository.AddAsync(CurrentPlaylist);
                Playlists.Add(CurrentPlaylist);

                // reset + fermeture
                IsModalOpen = false;
                CurrentPlaylist = new Playlist();
            }
            finally
            {
                IsSaving = false;
            }
        }

        // DELETE
        private async Task DeletePlaylist(Playlist? playlist)
        {
            if (playlist == null)
                return;

            await _playlistRepository.DeleteAsync(playlist.Id);
            Playlists.Remove(playlist);
        }

        // OPEN DETAIL PAGE
        private void OpenPlaylist(Playlist? playlist)
        {
            if (playlist == null)
                return;

            var vm = new PlaylistDetailViewModel(_playlistRepository, playlist);

            var mainVM = App.Current.MainWindow.DataContext as MainViewModel;

            if (mainVM != null)
                mainVM.CurrentView = vm;
        }

        // IMAGE PICKER
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

        // CLOSE MODAL
        private void CloseModal()
        {
            IsModalOpen = false;
            ErrorMessage = "";
        }
    }
}