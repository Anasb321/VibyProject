using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Playlist>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Playlist?> GetByIdAsync(int id)
        {
            return await _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Playlist playlist)
        {
            var trackedEntity = _context.Playlists.Local.FirstOrDefault(p => p.Id == playlist.Id);
            
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTrackToPlaylistAsync(int playlistId, Track track)
        {
            if (track == null) return;

            var existingTrack = await _context.Tracks
            .FirstOrDefaultAsync(t => t.DeezerId == track.DeezerId);

            Track trackToLink;

            if (existingTrack == null)
            {
                _context.Tracks.Add(track);
                await _context.SaveChangesAsync();
                trackToLink = track;
            }
            else
            {
                trackToLink = existingTrack;
            }

            var playlist = await _context.Playlists
            .Include(p => p.Tracks)
            .FirstOrDefaultAsync(p => p.Id == playlistId);

            if (playlist != null)
            {
                if (!playlist.Tracks.Any(t => t.DeezerId == trackToLink.DeezerId))
                {
                    playlist.Tracks.Add(trackToLink);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveTrackFromPlaylistAsync(int playlistId, int trackId)
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

        public async Task<List<Track>> GetTracksFromPlaylistAsync(int playlistId)
        {
            return await _context.Playlists
                .AsNoTracking()
                .Where(p => p.Id == playlistId)
                .SelectMany(p => p.Tracks)
                .ToListAsync();
        }
    }
}