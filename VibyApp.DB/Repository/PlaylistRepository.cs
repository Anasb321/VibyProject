using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public List<Playlist> ObtenirToutParUtilisateur(int userId)
        {
            return _context.Playlists
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Include(p => p.Tracks)
                .ToList();
        }

        public Playlist? ObtenirParId(int id)
        {
            return _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Ajouter(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
        }

        public void Modifier(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            _context.SaveChanges();
        }

        public void Supprimer(int id)
        {
            var playlist = _context.Playlists.Find(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                _context.SaveChanges();
            }
        }

        public void AjouterTrackAPlaylist(int playlistId, int trackId)
        {
            var playlist = _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefault(p => p.Id == playlistId);

            var track = _context.Tracks.Find(trackId);

            if (playlist != null && track != null)
            {
                if (!playlist.Tracks.Any(t => t.Id == trackId))
                {
                    playlist.Tracks.Add(track);
                    _context.SaveChanges();
                }
            }
        }

        public void RetirerTrackDePlaylist(int playlistId, int trackId)
        {
            var playlist = _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefault(p => p.Id == playlistId);

            if (playlist != null)
            {
                var trackToRemove = playlist.Tracks.FirstOrDefault(t => t.Id == trackId);

                if (trackToRemove != null)
                {
                    playlist.Tracks.Remove(trackToRemove);
                    _context.SaveChanges();
                }
            }
        }

        public List<Track> ObtenirTracksDePlaylist(int playlistId)
        {
            return _context.Playlists
                .AsNoTracking()
                .Where(p => p.Id == playlistId)
                .SelectMany(p => p.Tracks)
                .ToList();
        }
    }
}