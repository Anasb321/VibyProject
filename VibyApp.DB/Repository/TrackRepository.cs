using Microsoft.EntityFrameworkCore;
using VibyApp.DB.Data;
using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{
    public class TrackRepository : ITrackRepository
    {
        private readonly VibyDbContext _context;

        public TrackRepository(VibyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Track>> GetAllAsync()
        {
            return await _context.Tracks.
                AsNoTracking()
                .ToListAsync();
        }

        public async Task<Track?> GetByIdAsync(int id)
        {
            return await _context.Tracks.
                AsNoTracking().
                FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Track?> GetByDeezerIdAsync(long deezerId)
        {
            return await _context.Tracks.
                AsNoTracking().
                FirstOrDefaultAsync(t => t.DeezerId == deezerId);
        }

        public async Task AddAsync(Track track)
        {
            await _context.Tracks.AddAsync(track);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Track track)
        {
            _context.Tracks.Update(track);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                await _context.SaveChangesAsync();
            }
        }
    }
}