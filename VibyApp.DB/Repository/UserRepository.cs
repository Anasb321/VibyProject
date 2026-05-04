using Microsoft.EntityFrameworkCore;
using VibyApp.DB.Data;
using VibyApp.DB.Models;
using VibyApp.DB.Services;

namespace VibyApp.DB.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly VibyDbContext _context;

        public UserRepository(VibyDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task AddAsync(User user)
        {
            user.MotDePasse = HashageService.HacherMDP(user.MotDePasse);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<User?> VerifyConnexionAsync(string identifiant, string motDePasseSaisi)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == identifiant || u.UserName == identifiant);

            if (user == null) return null;

            return HashageService.VerifierMDP(motDePasseSaisi, user.MotDePasse) ? user : null;
        }
        public async Task<bool> ExistsAsync(string userName, string email)
        {
            return await _context.Users.AnyAsync(u =>
            u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase) ||
            u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<bool> AjouterMusiqueAuxFavorisAsync(int userId, int trackId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteTracks)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var track = await _context.Tracks.FindAsync(trackId);

            if (user == null || track == null) return false;

            if (!user.FavoriteTracks.Any(t => t.Id == trackId))
            {
                user.FavoriteTracks.Add(track);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RetirerMusiqueDesFavorisAsync(int userId, int trackId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteTracks)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            var trackToRemove = user.FavoriteTracks.FirstOrDefault(t => t.Id == trackId);

            if (trackToRemove != null)
            {
                user.FavoriteTracks.Remove(trackToRemove);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AjouterArtisteAuxFavorisAsync(int userId, int artistId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteArtists)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var artist = await _context.Artists.FindAsync(artistId);

            if (user == null || artist == null) return false;

            if (!user.FavoriteArtists.Any(a => a.Id == artistId))
            {
                user.FavoriteArtists.Add(artist);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RetirerArtisteDesFavorisAsync(int userId, int artistId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteArtists)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            var artistToRemove = user.FavoriteArtists.FirstOrDefault(t => t.Id == artistId);

            if (artistToRemove != null)
            {
                user.FavoriteArtists.Remove(artistToRemove);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}