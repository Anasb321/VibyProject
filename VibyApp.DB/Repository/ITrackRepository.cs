using Microsoft.EntityFrameworkCore;
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
        Task<List<Track>> ObtenirToutAsync();

        Task<Track?> ObtenirParIdAsync(int id);

        Task AjouterAsync(Track track);

        Task ModifierAsync(Track track);

        Task SupprimerAsync(int id);
    }
}
