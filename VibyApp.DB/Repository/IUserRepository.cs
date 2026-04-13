using System.Collections.Generic;
using VibyApp.DB.Models;

namespace VibyApp.DB.Repository
{
    public interface IUserRepository
    {
        List<User> ObtenirTout();

        User? ObtenirParId(int id);

        User? ObtenirParUserName(string userName);
        User? ObtenirParEmail(string email);

        void Ajouter(User user);
        void Modifier(User user);
        void Supprimer(int id);

        bool ExisteDeja(string userName, string email);
    }
}