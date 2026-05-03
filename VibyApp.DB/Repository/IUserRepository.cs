using VibyApp.DB.Data;
using VibyApp.DB.Models;
using VibyApp.DB.Services;

namespace VibyApp.DB.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);

        // For Login
        Task<User?> GetByUserNameAsync(string userName);

        // For Registration/Forgot Password
        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        // Check for unique constraints on username and email during registration
        Task<bool> ExistsAsync(string userName, string email);
    }
}