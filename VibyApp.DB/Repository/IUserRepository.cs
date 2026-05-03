using VibyApp.DB.Models;

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
        Task<bool> DeleteAsync(int id);

        // Check for unique constraints on username and email during registration
        Task<bool> ExistsAsync(string userName, string email);
    }
}