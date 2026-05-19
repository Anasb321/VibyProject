using Microsoft.Extensions.DependencyInjection;
using VibyApp.DB.Models;
using VibyApp.UI.ViewModels;

namespace VibyApp.UI.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFavoriteCoordinator _favoriteCoordinator;
        private MainViewModel? _shell;

        public NavigationService(
            IServiceProvider serviceProvider,
            ICurrentUserService currentUserService,
            IFavoriteCoordinator favoriteCoordinator)
        {
            _serviceProvider = serviceProvider;
            _currentUserService = currentUserService;
            _favoriteCoordinator = favoriteCoordinator;
        }

        public void AttachShell(object shellViewModel)
        {
            _shell = shellViewModel as MainViewModel
                ?? throw new ArgumentException("Le shell doit être un MainViewModel.", nameof(shellViewModel));
        }

        public void NavigateTo(object viewModel)
        {
            if (_shell == null)
                throw new InvalidOperationException("NavigationService non initialisé. Appelez AttachShell d'abord.");

            _shell.SetCurrentView(viewModel);
        }

        public void NavigateToLogin()
        {
            _shell?.PlayerBarViewModel.StopPlayback();
            _favoriteCoordinator.Clear();
            NavigateTo(_serviceProvider.GetRequiredService<LoginViewModel>());
        }

        public void NavigateToRegister() => NavigateTo(_serviceProvider.GetRequiredService<RegisterViewModel>());

        public void NavigateToHome()
        {
            if (_shell == null) return;
            NavigateTo(_shell.HomeVM);
            _ = _shell.HomeVM.RefreshFavoritesAsync();
        }

        public void NavigateToLibrary() =>
            NavigateTo(_serviceProvider.GetRequiredService<LibraryViewModel>());

        public void NavigateToProfile() => NavigateTo(_serviceProvider.GetRequiredService<ProfileViewModel>());

        public void NavigateToLiked()
        {
            if (_currentUserService.CurrentUser == null)
            {
                NavigateToLogin();
                return;
            }

            var likedVm = _serviceProvider.GetRequiredService<LikedViewModel>();
            NavigateTo(likedVm);
            _ = likedVm.LoadFavoritesAsync();
        }

        public async Task NavigateToHomeAfterLoginAsync()
        {
            if (_shell == null) return;
            _shell.PlayerBarViewModel.StopPlayback();
            await _favoriteCoordinator.LoadFromDatabaseAsync();
            NavigateTo(_shell.HomeVM);
            await _shell.HomeVM.RefreshFavoritesAsync();
        }

        public void NavigateToPlaylistDetail(Playlist playlist) =>
            NavigateTo(new PlaylistDetailViewModel(
                _serviceProvider.GetRequiredService<IServiceScopeFactory>(),
                this,
                playlist,
                _serviceProvider.GetRequiredService<DeezerService>()));
    }
}
