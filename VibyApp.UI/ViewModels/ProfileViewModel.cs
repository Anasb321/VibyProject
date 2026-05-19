using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICurrentUserService _currentUserService;
        private User? _currentUser;

        [ObservableProperty]
        private string _username = "Chargement...";

        [ObservableProperty]
        private string _userEmail = "...";

        [ObservableProperty]
        private string _fullName = "...";

        public ProfileViewModel(IServiceScopeFactory scopeFactory, ICurrentUserService currentUserService)
        {
            _scopeFactory = scopeFactory;
            _currentUserService = currentUserService;
            _ = LoadUserDataAsync();
        }

        private async Task LoadUserDataAsync()
        {
            if (_currentUserService.CurrentUser == null)
                return;

            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetByIdAsync(_currentUserService.CurrentUser.Id);
            if (user != null)
            {
                _currentUser = user;
                Username = user.UserName;
                UserEmail = user.Email;
                FullName = $"{user.FirstName} {user.LastName}";
            }
        }

        [RelayCommand]
        private async Task UpdateProfile()
        {
            if (_currentUser == null)
                return;

            _currentUser.UserName = Username;
            _currentUser.Email = UserEmail;

            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            await userRepository.UpdateAsync(_currentUser);
        }
    }
}
