using Microsoft.EntityFrameworkCore;
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
        Task<List<Playlist>> ObtenirToutParUtilisateurAsync(int userId);

        Task<Playlist?> ObtenirParIdAsync(int id);

        Task AjouterAsync(Playlist playlist);

        Task ModifierAsync(Playlist playlist);

        Task SupprimerAsync(int id);

        Task AjouterTrackAPlaylistAsync(int playlistId, int trackId);

        Task RetirerTrackDePlaylistAsync(int playlistId, int trackId);

        Task<List<Track>> ObtenirTracksDePlaylistAsync(int playlistId);
    }

}
