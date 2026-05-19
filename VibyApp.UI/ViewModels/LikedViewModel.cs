using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using VibyApp.DB.Repository;
using VibyApp.UI.Helpers;
using VibyApp.UI.Models;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public partial class LikedViewModel : BaseViewModel
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IFavoriteCoordinator _favoriteCoordinator;
        private readonly ICurrentUserService _currentUserService;
        private readonly PlayerBarViewModel _playerBarViewModel;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TrackDisplayItem> FavoriteTracks { get; } = new();

        public LikedViewModel(
            IServiceScopeFactory scopeFactory,
            IFavoriteCoordinator favoriteCoordinator,
            ICurrentUserService currentUserService,
            PlayerBarViewModel playerBarViewModel)
        {
            _scopeFactory = scopeFactory;
            _favoriteCoordinator = favoriteCoordinator;
            _currentUserService = currentUserService;
            _playerBarViewModel = playerBarViewModel;

            _favoriteCoordinator.FavoriteChanged += OnFavoriteChanged;
            _ = LoadFavoritesAsync();
        }

        public async Task LoadFavoritesAsync()
        {
            if (_currentUserService.CurrentUser == null)
                return;

            try
            {
                IsLoading = true;
                await _favoriteCoordinator.LoadFromDatabaseAsync();

                using var scope = _scopeFactory.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var tracks = await userRepository.GetFavoriteTracksAsync(_currentUserService.CurrentUser.Id);

                FavoriteTracks.Clear();
                foreach (var dbTrack in tracks)
                    FavoriteTracks.Add(new TrackDisplayItem(TrackMapper.ToUiTrack(dbTrack), isFavorite: true));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnFavoriteChanged(long deezerId, bool isFavorite)
        {
            if (!isFavorite)
            {
                var item = FavoriteTracks.FirstOrDefault(f => f.Track.Id == deezerId);
                if (item != null)
                    FavoriteTracks.Remove(item);
            }
        }

        [RelayCommand]
        private void PlayTrack(TrackDisplayItem? item)
        {
            if (item?.Track == null || string.IsNullOrWhiteSpace(item.Track.Preview))
                return;

            var queue = FavoriteTracks.Select(f => f.Track);
            _playerBarViewModel.PlayTrack(item.Track, queue);
        }

        [RelayCommand]
        private async Task ToggleFavorite(TrackDisplayItem? item)
        {
            if (item == null || _currentUserService.CurrentUser == null)
                return;

            var isFavorite = await _favoriteCoordinator.ToggleAsync(item.Track);
            if (!isFavorite)
                FavoriteTracks.Remove(item);
        }
    }
}
