using VibyApp.DB.Models;

namespace VibyApp.UI.Services
{
    public interface ICurrentUserService
    {
        User? CurrentUser { get; }
        void SetUser(User user);
        void ClearUser();
    }
}
