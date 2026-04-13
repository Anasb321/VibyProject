using System.Collections.Generic;
using System.Linq;
using VibyApp.DB.Data;
using VibyApp.DB.Models;
using VibyApp.DB.Services;

namespace VibyApp.DB.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly VibyDbContext _context;

        public UserRepository(VibyDbContext context)
        {
            _context = context;
        }

        public List<User> ObtenirTout() {
            return _context.Users.ToList();
        }

        public User? ObtenirParId(int id) { 
            return _context.Users.Find(id); 
        }

        public User? ObtenirParUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public User? ObtenirParEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void Ajouter(User user)
        {
            user.MotDePasse = HashageService.HacherMDP(user.MotDePasse);

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User? VerifierConnexion(string userName, string motDePasseSaisi)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user == null) return null;
            return HashageService.VerifierMDP(motDePasseSaisi, user.MotDePasse) ? user : null;
        }

        public void Modifier(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Supprimer(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public bool ExisteDeja(string userName, string email)
        {
            return _context.Users.Any(u => u.UserName.ToLower() == userName.ToLower() || u.Email.ToLower() == email.ToLower());
        }
    }
}