using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{
    internal interface IArtistRepository
    {
        Task<List<Artist>> ObtenirToutAsync();

        Task<Artist?> ObtenirParIdAsync(int id);

        Task AjouterAsync(Artist artist);

        Task ModifierAsync(Artist artist);

        Task SupprimerAsync(int id);

        Task<List<Artist>> ObtenirFavorisParUtilisateurAsync(int userId);
    }
}