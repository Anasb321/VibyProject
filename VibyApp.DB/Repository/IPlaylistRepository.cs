using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{

    public interface IPlaylistRepository
    {
        List<Playlist> ObtenirToutParUtilisateur(int userId);
        Playlist? ObtenirParId(int id);
        void Ajouter(Playlist playlist);
        void Modifier(Playlist playlist);
        void Supprimer(int id);
        void AjouterTrackAPlaylist(int playlistId, int trackId);
        void RetirerTrackDePlaylist(int playlistId, int trackId);
        List<Track> ObtenirTracksDePlaylist(int playlistId);
    }

}
