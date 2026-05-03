namespace VibyApp.DB.Services
{
    public static class HashageService
    {

        public static string HacherMDP(string motDePasse)
        {
            return BCrypt.Net.BCrypt.HashPassword(motDePasse);
        }

        public static bool VerifierMDP(string motDePasseSaisi, string motDePasseStocke)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(motDePasseSaisi, motDePasseStocke);
            }
            catch
            {
                return false;
            }
        }
    }
}