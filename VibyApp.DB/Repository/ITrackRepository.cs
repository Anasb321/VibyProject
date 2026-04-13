using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{
    public interface ITrackRepository
    {
        List<Track> ObtenirTout();
        Track? ObtenirParId(int id);
        void Ajouter(Track track);
        void Modifier(Track track);
        void Supprimer(int id);
    }
}
