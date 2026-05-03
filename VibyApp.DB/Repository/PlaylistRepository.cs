using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibyApp.DB.Data;
using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly VibyDbContext _context;

        public PlaylistRepository(VibyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Playlist>> ObtenirToutParUtilisateurAsync(int userId)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Include(p => p.Tracks)
                .ToListAsync();
        }

        public async Task<Playlist?> ObtenirParIdAsync(int id)
        {
            return await _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AjouterAsync(Playlist playlist)
        {
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task ModifierAsync(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task SupprimerAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AjouterTrackAPlaylistAsync(int playlistId, int trackId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            var track = await _context.Tracks.FindAsync(trackId);

            if (playlist != null && track != null)
            {
                if (!playlist.Tracks.Any(t => t.Id == trackId))
                {
                    playlist.Tracks.Add(track);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RetirerTrackDePlaylistAsync(int playlistId, int trackId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist != null)
            {
                var trackToRemove = playlist.Tracks.FirstOrDefault(t => t.Id == trackId);

                if (trackToRemove != null)
                {
                    playlist.Tracks.Remove(trackToRemove);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Track>> ObtenirTracksDePlaylistAsync(int playlistId)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(p => p.Id == playlistId)
                .SelectMany(p => p.Tracks)
                .ToListAsync();
        }
    }
}