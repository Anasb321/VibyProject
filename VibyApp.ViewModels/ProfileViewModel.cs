using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;

namespace VibyApp.ViewModels
{
    
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IUserRepository _userRepository;

        private User? _currentUser;

        [ObservableProperty]
        private string _username = "Chargement...";

        [ObservableProperty]
        private string _userEmail = "...";

        [ObservableProperty]
        private string _fullName = "...";

        public ProfileViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _ = LoadUserDataAsync();
        }
        private async Task LoadUserDataAsync()
        {
            var user = await _userRepository.GetByIdAsync(1);
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
            if (_currentUser != null)
            {
              
                _currentUser.UserName = Username;
                _currentUser.Email = UserEmail;

                await _userRepository.UpdateAsync(_currentUser);
            }
        }
    }
}