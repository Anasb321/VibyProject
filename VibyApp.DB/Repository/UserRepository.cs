using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<User>> ObtenirToutAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> ObtenirParIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> ObtenirParUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User?> ObtenirParEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AjouterUserAsync(User user)
        {
            user.MotDePasse = HashageService.HacherMDP(user.MotDePasse);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> VerifierConnexionAsync(string userName, string motDePasseSaisi)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return null;

            return HashageService.VerifierMDP(motDePasseSaisi, user.MotDePasse) ? user : null;
        }

        public async Task ModifierUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task SupprimerUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserExisteDejaAsync(string userName, string email)
        {
            return await _context.Users.AnyAsync(u =>
                u.UserName.ToLower() == userName.ToLower() ||
                u.Email.ToLower() == email.ToLower());
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
    }
}