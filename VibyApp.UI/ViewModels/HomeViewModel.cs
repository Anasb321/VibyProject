using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VibyApp.UI.Models;
using VibyApp.UI.Services;
using VibyProject;

namespace VibyApp.UI.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly DeezerService _deezerService;
        private readonly PlayerBarViewModel _playerBarViewModel;
        private CancellationTokenSource? _searchCts;

        private bool _isLoading;
        private bool _isSearching;

        [ObservableProperty]
        private string _searchQuery = string.Empty;

        public ObservableCollection<Track> TopTracks { get; } = new();
        public ObservableCollection<Artist> TopArtists { get; } = new();
        public ObservableCollection<GenreItem> Genres { get; } = new();
        public ObservableCollection<Track> SearchResults { get; } = new();

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public bool IsSearching
        {
            get => _isSearching;
            set { _isSearching = value; OnPropertyChanged(); }
        }

        public bool IsSearchMode => !string.IsNullOrWhiteSpace(SearchQuery) && SearchQuery.Trim().Length >= 2;

        public HomeViewModel(DeezerService deezerService, PlayerBarViewModel playerBarViewModel)
        {
            _deezerService = deezerService;
            _playerBarViewModel = playerBarViewModel;
            _ = LoadDataAsync();
        }

        [RelayCommand]
        private void PlayTrack(Track? track)
        {
            if (track == null || string.IsNullOrWhiteSpace(track.Preview))
                return;

            var queue = IsSearchMode ? SearchResults : TopTracks;
            _playerBarViewModel.PlayTrack(track, queue);
        }

        [RelayCommand]
        private void SearchGenre(string? genreName)
        {
            if (string.IsNullOrWhiteSpace(genreName))
                return;

            SearchQuery = genreName;
        }

        partial void OnSearchQueryChanged(string value)
        {
            OnPropertyChanged(nameof(IsSearchMode));
            _ = HandleSearchQueryChangedAsync(value);
        }

        private async Task HandleSearchQueryChangedAsync(string query)
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
            {
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    SearchResults.Clear();
                    IsSearching = false;
                    OnPropertyChanged(nameof(IsSearchMode));
                });
                return;
            }

            try
            {
                IsSearching = true;
                await Task.Delay(350, token);

                var results = await _deezerService.SearchTracksUiAsync(query, limit: 30);
                if (token.IsCancellationRequested)
                    return;

                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    SearchResults.Clear();
                    foreach (var track in results)
                        SearchResults.Add(track);

                    IsSearching = false;
                    OnPropertyChanged(nameof(IsSearchMode));
                });
            }
            catch (OperationCanceledException)
            {
                // Recherche annulée par une nouvelle frappe
            }
            catch
            {
                if (!token.IsCancellationRequested)
                {
                    await App.Current.Dispatcher.InvokeAsync(() => IsSearching = false);
                }
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;

                var chartTask = _deezerService.GetTop50Async();
                var genresTask = _deezerService.GetGenreNamesAsync();
                await Task.WhenAll(chartTask, genresTask);

                var (tracks, artists) = await chartTask;
                var genreNames = await genresTask;

                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    TopTracks.Clear();
                    foreach (var track in tracks)
                        TopTracks.Add(track);

                    TopArtists.Clear();
                    foreach (var artist in artists)
                        TopArtists.Add(artist);

                    Genres.Clear();
                    for (var i = 0; i < genreNames.Count; i++)
                    {
                        Genres.Add(new GenreItem
                        {
                            Name = genreNames[i],
                            Background = GenrePalette.CreateBrush(i)
                        });
                    }
                });
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
