using Microsoft.Extensions.DependencyInjection;
using VibyApp.DB.Repository;
using VibyApp.UI.Helpers;
using UiTrack = VibyApp.UI.Models.Track;

namespace VibyApp.UI.Services
{
    public class FavoriteCoordinator : IFavoriteCoordinator
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly HashSet<long> _favoriteIds = new();

        public event Action<long, bool>? FavoriteChanged;

        public FavoriteCoordinator(IServiceScopeFactory scopeFactory, ICurrentUserService currentUserService)
        {
            _scopeFactory = scopeFactory;
            _currentUserService = currentUserService;
        }

        public async Task LoadFromDatabaseAsync()
        {
            _favoriteIds.Clear();

            if (_currentUserService.CurrentUser == null)
                return;

            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var tracks = await userRepository.GetFavoriteTracksAsync(_currentUserService.CurrentUser.Id);
            foreach (var track in tracks)
                _favoriteIds.Add(track.DeezerId);
        }

        public void Clear() => _favoriteIds.Clear();

        public bool IsFavorite(long deezerId) => _favoriteIds.Contains(deezerId);

        public async Task<bool> ToggleAsync(UiTrack track)
        {
            if (_currentUserService.CurrentUser == null)
                return false;

            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var dbTrack = TrackMapper.ToDbTrack(track);
            var isFavorite = await userRepository.ToggleFavoriteTrackAsync(
                _currentUserService.CurrentUser.Id, dbTrack);

            if (isFavorite)
                _favoriteIds.Add(track.Id);
            else
                _favoriteIds.Remove(track.Id);

            FavoriteChanged?.Invoke(track.Id, isFavorite);
            return isFavorite;
        }
    }
}
