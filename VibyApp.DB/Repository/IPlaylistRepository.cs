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

        Task AddTrackToPlaylistAsync(int playlistId, int trackId);
        Task RemoveTrackFromPlaylistAsync(int playlistId, int trackId);
        Task<List<Track>> GetTracksFromPlaylistAsync(int playlistId);
    }

}
