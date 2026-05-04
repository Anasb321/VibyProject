using VibyApp.DB.Data;
using VibyApp.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace VibyApp.DB.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly VibyDbContext _context;

        public ArtistRepository(VibyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Artist>> ObtenirToutAsync()
        {
            return await _context.Artists
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Artist?> ObtenirParIdAsync(int id)
        {
            return await _context.Artists
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AjouterAsync(Artist artist)
        {
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierAsync(Artist artist)
        {
            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
        }

        public async Task SupprimerAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Artist>> ObtenirFavorisParUtilisateurAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.FavoriteArtists)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.FavoriteArtists ?? new List<Artist>();
        }
    }
}
