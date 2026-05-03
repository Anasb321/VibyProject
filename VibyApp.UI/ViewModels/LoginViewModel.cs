using CommunityToolkit.Mvvm.Input;
using VibyApp.DB.Data;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;
namespace VibyApp.UI.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;
        private readonly DeezerService _deezerService;

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

        // On injecte le service ici via le constructeur
        public LoginViewModel(MainViewModel mainVM, DeezerService deezerService)
        {
            _mainVM = mainVM;
            _deezerService = deezerService;
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
                using var dbContext = new VibyDbContext();
                var userRepository = new UserRepository(dbContext);

                var isConnected = await userRepository.VerifyConnexionAsync(Identifier, password);

                if (isConnected != null)
                {
                    _mainVM.CurrentView = new HomeViewModel(_deezerService);
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
        public void GoToRegister()
        {
            _mainVM.CurrentView = new RegisterViewModel(_mainVM, _deezerService);
        }
    }
}