using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VibyApp.DB.Data;
using VibyApp.DB.Models;
using VibyApp.DB.Services;

namespace VibyApp.DB.Repository
{
    public interface IUserRepository
    {

        Task<List<User>> ObtenirToutAsync();

        Task<User?> ObtenirParIdAsync(int id);

        Task<User?> ObtenirParUserNameAsync(string userName);

        Task<User?> ObtenirParEmailAsync(string email);

        Task AjouterUserAsync(User user);

        Task<User?> VerifierConnexionAsync(string userName, string motDePasseSaisi);

        Task ModifierUserAsync(User user);

        Task SupprimerUserAsync(int id);

        Task<bool> UserExisteDejaAsync(string userName, string email);

        Task<bool> AjouterMusiqueAuxFavorisAsync(int userId, int trackId);
    }
}