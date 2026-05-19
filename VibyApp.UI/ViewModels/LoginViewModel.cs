using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICurrentUserService _currentUserService;

        private string _identifier = "";
        public string Identifier
        {
            get => _identifier;
            set { _identifier = value; OnPropertyChanged(); }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public LoginViewModel(
            INavigationService navigationService,
            IServiceScopeFactory scopeFactory,
            ICurrentUserService currentUserService)
        {
            _navigationService = navigationService;
            _scopeFactory = scopeFactory;
            _currentUserService = currentUserService;
        }

        public async Task LoginAction(string password)
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Identifier) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                return;
            }

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var user = await userRepository.VerifyConnexionAsync(Identifier, password);

                if (user != null)
                {
                    _currentUserService.SetUser(user);
                    await _navigationService.NavigateToHomeAfterLoginAsync();
                }
                else
                {
                    ErrorMessage = "Identifiants incorrects.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur technique : " + ex.Message;
            }
        }

        [RelayCommand]
        public void GoToRegister() => _navigationService.NavigateToRegister();
    }
}
