using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace WpfApp
{
    class HashageService
    {
        public static string HacherMDP(string motDePasse)
        {
            return BCrypt.Net.BCrypt.HashPassword(motDePasse);
        }

        public static bool VerifierMDP(string motDePasseSaisi, string motDePasseStocke)
        {
            return BCrypt.Net.BCrypt.Verify(motDePasseSaisi, motDePasseStocke);
        }
    }
}
