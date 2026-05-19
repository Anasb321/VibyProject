using UiTrack = VibyApp.UI.Models.Track;

namespace VibyApp.UI.Services
{
    public interface IFavoriteCoordinator
    {
        event Action<long, bool>? FavoriteChanged;

        Task LoadFromDatabaseAsync();
        void Clear();
        bool IsFavorite(long deezerId);
        Task<bool> ToggleAsync(UiTrack track);
    }
}
