using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{
    public interface ITrackRepository
    {
        Task<List<Track>> GetAllAsync();
        Task<Track?> GetByIdAsync(int id);

        //  DeezerId integration
        Task<Track?> GetByDeezerIdAsync(long deezerId);

        Task AddAsync(Track track);
        Task UpdateAsync(Track track);
        Task DeleteAsync(int id);
    }
}
