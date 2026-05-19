using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using VibyApp.UI.Models;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly DeezerService _deezerService;
        private readonly PlayerBarViewModel _playerBarViewModel;
        private readonly IFavoriteCoordinator _favoriteCoordinator;
        private readonly ICurrentUserService _currentUserService;
        private CancellationTokenSource? _searchCts;

        private bool _isLoading;
        private bool _isSearching;

        [ObservableProperty]
        private string _searchQuery = string.Empty;

        public ObservableCollection<TrackDisplayItem> TopTracks { get; } = new();
        public ObservableCollection<Artist> TopArtists { get; } = new();
        public ObservableCollection<GenreItem> Genres { get; } = new();
        public ObservableCollection<TrackDisplayItem> SearchResults { get; } = new();

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

        public HomeViewModel(
            DeezerService deezerService,
            PlayerBarViewModel playerBarViewModel,
            IFavoriteCoordinator favoriteCoordinator,
            ICurrentUserService currentUserService)
        {
            _deezerService = deezerService;
            _playerBarViewModel = playerBarViewModel;
            _favoriteCoordinator = favoriteCoordinator;
            _currentUserService = currentUserService;

            _favoriteCoordinator.FavoriteChanged += OnFavoriteChanged;

            _ = LoadDataAsync();
        }

        public async Task RefreshFavoritesAsync()
        {
            await _favoriteCoordinator.LoadFromDatabaseAsync();
            SyncFavoriteFlags(TopTracks);
            SyncFavoriteFlags(SearchResults);
        }

        private void OnFavoriteChanged(long deezerId, bool isFavorite)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateItemFavorite(TopTracks, deezerId, isFavorite);
                UpdateItemFavorite(SearchResults, deezerId, isFavorite);
            });
        }

        private static void UpdateItemFavorite(IEnumerable<TrackDisplayItem> items, long deezerId, bool isFavorite)
        {
            foreach (var item in items.Where(i => i.Track.Id == deezerId))
                item.IsFavorite = isFavorite;
        }

        [RelayCommand]
        private void PlayTrack(TrackDisplayItem? item)
        {
            if (item?.Track == null || string.IsNullOrWhiteSpace(item.Track.Preview))
                return;

            var queue = (IsSearchMode ? SearchResults : TopTracks).Select(t => t.Track);
            _playerBarViewModel.PlayTrack(item.Track, queue);
        }

        [RelayCommand]
        private async Task ToggleFavorite(TrackDisplayItem? item)
        {
            if (item?.Track == null || _currentUserService.CurrentUser == null)
                return;

            item.IsFavorite = await _favoriteCoordinator.ToggleAsync(item.Track);
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
                await Application.Current.Dispatcher.InvokeAsync(() =>
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

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    SearchResults.Clear();
                    foreach (var track in results)
                        SearchResults.Add(WrapTrack(track));

                    IsSearching = false;
                    OnPropertyChanged(nameof(IsSearchMode));
                });
            }
            catch (OperationCanceledException) { }
            catch
            {
                if (!token.IsCancellationRequested)
                    await Application.Current.Dispatcher.InvokeAsync(() => IsSearching = false);
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;

                if (_currentUserService.CurrentUser != null)
                    await _favoriteCoordinator.LoadFromDatabaseAsync();

                var chartTask = _deezerService.GetTop50Async();
                var genresTask = _deezerService.GetGenreNamesAsync();
                await Task.WhenAll(chartTask, genresTask);

                var (tracks, artists) = await chartTask;
                var genreNames = await genresTask;

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    TopTracks.Clear();
                    foreach (var track in tracks)
                        TopTracks.Add(WrapTrack(track));

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

        private TrackDisplayItem WrapTrack(Track track) => new(track, _favoriteCoordinator.IsFavorite(track.Id));

        private void SyncFavoriteFlags(IEnumerable<TrackDisplayItem> items)
        {
            foreach (var item in items)
                item.IsFavorite = _favoriteCoordinator.IsFavorite(item.Track.Id);
        }
    }
}
