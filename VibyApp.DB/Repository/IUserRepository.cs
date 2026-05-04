using Microsoft.EntityFrameworkCore;
using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByUserNameAsync(string userName);

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(string userName, string email);

        Task<bool> AjouterMusiqueAuxFavorisAsync(int userId, int trackId);

        Task<bool> RetirerMusiqueDesFavorisAsync(int userId, int trackId);

        Task<bool> AjouterArtisteAuxFavorisAsync(int userId, int artistId);
    }
}