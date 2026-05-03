using System.Text.RegularExpressions;

namespace VibyApp.UI.Services
{
    public class AuthService
    {
        /// <summary>
        /// Vérifie si le format de l'email est correct
        /// </summary>
        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}