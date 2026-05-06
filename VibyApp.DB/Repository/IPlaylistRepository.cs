using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{

    public interface IPlaylistRepository
    {
        Task<List<Playlist>> GetAllByUserIdAsync(int userId);
        Task<Playlist?> GetByIdAsync(int id);
        Task AddAsync(Playlist playlist);
        Task UpdateAsync(Playlist playlist);
        Task DeleteAsync(int id);

        // Task for managing tracks in playlists
        Task AddTrackToPlaylistAsync(int playlistId, Track track);
        Task RemoveTrackFromPlaylistAsync(int playlistId, int trackId);
        Task<List<Track>> GetTracksFromPlaylistAsync(int playlistId);
    }

}
