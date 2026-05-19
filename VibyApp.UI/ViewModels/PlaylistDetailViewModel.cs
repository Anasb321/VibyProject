using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public partial class PlaylistDetailViewModel : BaseViewModel
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INavigationService _navigationService;
        private readonly DeezerService _deezerService;

        [ObservableProperty]
        private Playlist _playlist;

        [ObservableProperty]
        private bool _isSearchOpen;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public ObservableCollection<Track> SearchResults { get; } = new();
        public ObservableCollection<Track> Tracks { get; } = new();

        public PlaylistDetailViewModel(
            IServiceScopeFactory scopeFactory,
            INavigationService navigationService,
            Playlist playlist,
            DeezerService deezerService)
        {
            _scopeFactory = scopeFactory;
            _navigationService = navigationService;
            _deezerService = deezerService;
            _playlist = playlist;

            _ = LoadTracksAsync();
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(SearchText)
                && !string.IsNullOrEmpty(SearchText)
                && SearchText.Length > 2)
            {
                _ = PerformSearchAsync(SearchText);
            }
        }

        private async Task PerformSearchAsync(string query)
        {
            var results = await _deezerService.SearchTracksAsync(query);
            SearchResults.Clear();
            foreach (var track in results)
                SearchResults.Add(track);
        }

        [RelayCommand]
        private async Task LoadTracksAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
            var tracks = await playlistRepository.GetTracksFromPlaylistAsync(Playlist.Id);
            Tracks.Clear();
            foreach (var t in tracks)
                Tracks.Add(t);
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
            await playlistRepository.UpdateAsync(Playlist);
        }

        [RelayCommand]
        private async Task DeletePlaylistAsync()
        {
            if (Playlist == null) return;

            using var scope = _scopeFactory.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
            await playlistRepository.DeleteAsync(Playlist.Id);

            _navigationService.NavigateToLibrary();
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
        private Task AddTrackAsync()
        {
            IsSearchOpen = true;
            return Task.CompletedTask;
        }

        [RelayCommand]
        private async Task RemoveTrackAsync(Track track)
        {
            if (track == null) return;

            using var scope = _scopeFactory.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
            await playlistRepository.RemoveTrackFromPlaylistAsync(Playlist.Id, track.Id);
            Tracks.Remove(track);
        }

        [RelayCommand]
        private void NavigateBack() => _navigationService.NavigateToLibrary();

        [RelayCommand]
        private async Task AddTrackToPlaylistAsync(Track selectedTrack)
        {
            if (selectedTrack == null) return;

            using var scope = _scopeFactory.CreateScope();
            var playlistRepository = scope.ServiceProvider.GetRequiredService<IPlaylistRepository>();
            await playlistRepository.AddTrackToPlaylistAsync(Playlist.Id, selectedTrack);

            var updatedTracks = await playlistRepository.GetTracksFromPlaylistAsync(Playlist.Id);

            Tracks.Clear();
            foreach (var track in updatedTracks)
                Tracks.Add(track);

            IsSearchOpen = false;
            SearchText = string.Empty;
        }
    }
}
