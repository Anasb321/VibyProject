using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<Track>> ObtenirToutAsync()
        {
            return await _context.Tracks.AsNoTracking().ToListAsync();
        }

        public async Task<Track?> ObtenirParIdAsync(int id)
        {
            return await _context.Tracks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AjouterAsync(Track track)
        {
            await _context.Tracks.AddAsync(track);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierAsync(Track track)
        {
            _context.Tracks.Update(track);
            await _context.SaveChangesAsync();
        }

        public async Task SupprimerAsync(int id)
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