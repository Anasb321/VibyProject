using VibyApp.DB.Models;

namespace VibyApp.UI.Services
{
    public interface INavigationService
    {
        void AttachShell(object shellViewModel);
        void NavigateTo(object viewModel);
        void NavigateToLogin();
        void NavigateToRegister();
        void NavigateToHome();
        void NavigateToLibrary();
        void NavigateToProfile();
        void NavigateToLiked();
        Task NavigateToHomeAfterLoginAsync();
        void NavigateToPlaylistDetail(Playlist playlist);
    }
}
