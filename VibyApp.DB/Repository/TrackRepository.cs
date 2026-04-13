using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public List<Track> ObtenirTout()
        {
            return _context.Tracks.AsNoTracking().ToList();
        }

        public Track? ObtenirParId(int id)
        {
            return _context.Tracks.AsNoTracking().FirstOrDefault(t => t.Id == id);
        }

        public void Ajouter(Track track)
        {
            _context.Tracks.Add(track);
            _context.SaveChanges();
        }

        public void Modifier(Track track)
        {
            _context.Tracks.Update(track);
            _context.SaveChanges();
        }

        public void Supprimer(int id)
        {
            var track = _context.Tracks.Find(id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                _context.SaveChanges();
            }
        }
    }
}