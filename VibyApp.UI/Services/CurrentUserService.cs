using VibyApp.DB.Models;

namespace VibyApp.UI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public User? CurrentUser { get; private set; }

        public void SetUser(User user) => CurrentUser = user;

        public void ClearUser() => CurrentUser = null;
    }
}
