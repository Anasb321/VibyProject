using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;
using VibyProject;

namespace VibyApp.UI.ViewModels
{
    public partial class PlaylistDetailViewModel : BaseViewModel
    {
        private readonly DeezerService _deezerService = new();

        private readonly IPlaylistRepository _playlistRepository;

        [ObservableProperty]
        private Playlist _playlist;

        [ObservableProperty]
        private bool _isSearchOpen;

        [ObservableProperty]
        private string _searchText;

        public ObservableCollection<Track> SearchResults { get; } = new();

        public ObservableCollection<Track> Tracks { get; } = new();

        public PlaylistDetailViewModel(IPlaylistRepository repo, Playlist playlist)
        {
            _playlistRepository = repo;
            _playlist = playlist;

            _ = LoadTracksAsync();
        }

        partial void OnSearchTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > 2)
            {
                _ = PerformSearchAsync(value);
            }
        }

        private async Task PerformSearchAsync(string query)
        {
            var results = await _deezerService.SearchTracksAsync(query);
            SearchResults.Clear();
            foreach (var track in results) SearchResults.Add(track);
        }

        [RelayCommand]
        private async Task LoadTracksAsync()
        {
            var tracks = await _playlistRepository.GetTracksFromPlaylistAsync(Playlist.Id);
            Tracks.Clear();
            foreach (var t in tracks)
                Tracks.Add(t);
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            await _playlistRepository.UpdateAsync(Playlist);
        }

        [RelayCommand]
        private async Task DeletePlaylistAsync()
        {
            if (Playlist == null) return;

            await _playlistRepository.DeleteAsync(Playlist.Id);

            // Retour à la bibliothèque
            if (App.Current.MainWindow.DataContext is MainViewModel mainVM)
            {
                mainVM.CurrentView = new LibraryViewModel(_playlistRepository);
            }
        }

        [RelayCommand]
        private void ChangeImage()
        {
            var dialog = new OpenFileDialog { Filter = "Images (*.png;*.jpg)|*.png;*.jpg" };
            if (dialog.ShowDialog() == true)
            {
                Playlist.PlaylistPicture = dialog.FileName;
                OnPropertyChanged(nameof(Playlist));
            }
        }

        [RelayCommand]
        private async Task AddTrackAsync()
        {
            IsSearchOpen = true;
        }

        [RelayCommand]
        private async Task RemoveTrackAsync(Track track)
        {
            if (track == null) return;
            await _playlistRepository.RemoveTrackFromPlaylistAsync(Playlist.Id, track.Id);
            Tracks.Remove(track);
        }

        [RelayCommand]
        private void NavigateBack()
        {
            if (App.Current.MainWindow.DataContext is MainViewModel mainVM)
            {
                mainVM.CurrentView = new LibraryViewModel(_playlistRepository);
            }
        }

        [RelayCommand]
        private async Task AddTrackToPlaylistAsync(Track selectedTrack)
        {
            if (selectedTrack == null) return;

            await _playlistRepository.AddTrackToPlaylistAsync(Playlist.Id, selectedTrack);

            var updatedTracks = await _playlistRepository.GetTracksFromPlaylistAsync(Playlist.Id);

            Tracks.Clear();
            foreach (var track in updatedTracks)
            {
                Tracks.Add(track);
            }

            IsSearchOpen = false;
            SearchText = string.Empty;
        }
    }
}