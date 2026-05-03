using CommunityToolkit.Mvvm.Input;
using VibyApp.DB.Data;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;
using VibyApp.UI.Views;

namespace VibyApp.UI.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); } }


        private string _firstName = "";
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }


        private string _lastName = "";
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }


        private string _userName = "";
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }


        private string _email = "";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }


        private string _password = "";
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }


        private string _confirmPassword = "";
        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; OnPropertyChanged(); } }


        private string _errorMessage = "";
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }

        private readonly MainViewModel _mainVM;
        private readonly DeezerService _deezerService;

        public RegisterViewModel(MainViewModel mainVM, DeezerService deezerService)
        {
            _mainVM = mainVM;
            _deezerService = deezerService;
        }

        [RelayCommand]
        public void GoToLogin()
        {
            _mainVM.CurrentView = new LoginViewModel(_mainVM, _deezerService);
        }

        [RelayCommand]
        public async Task Register()
        {
            if (_isBusy) return;

            try
            {
                _isBusy = true;
                ErrorMessage = "";

                // 1. Validation des données
                if (!ValidateInputs()) return;

                // 2. Traitement Data
                using var dbContext = new VibyDbContext();
                var userRepository = new UserRepository(dbContext);

                // 3. Vérifications d'unicité
                if (await userRepository.GetByEmailAsync(Email) != null)
                {
                    ErrorMessage = "Cette adresse e-mail est déjà associée à un compte.";
                    return;
                }

                if (await userRepository.GetByUserNameAsync(UserName) != null)
                {
                    ErrorMessage = "Ce nom d'utilisateur est déjà pris.";
                    return;
                }

                // 4. Mapping & Sauvegarde
                var newUser = new User
                {
                    FirstName = FirstName.Trim(),
                    LastName = LastName.Trim(),
                    UserName = UserName.Trim(),
                    Email = Email.Trim().ToLower(),
                    MotDePasse = Password
                };

                await userRepository.AddAsync(newUser);

                ErrorMessage = "Compte créé avec succès !";
                await Task.Delay(1500);
                GoToLogin();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Détail technique : " + ex.Message;
            }
            finally
            {
                _isBusy = false;
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(UserName) ||
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Tous les champs marqués d'une étoile sont obligatoires.";
                return false;
            }

            // 2. Validation de la longueur (Nom)
            if (LastName.Trim().Length < 2)
            {
                ErrorMessage = "Le nom doit contenir au moins 2 caractères.";
                return false;
            }

            // 3. Validation de la longueur (Prénom)
            if (FirstName.Trim().Length < 2)
            {
                ErrorMessage = "Le prénom doit contenir au moins 2 caractères.";
                return false;
            }

            // 4. Validation de la longueur du Pseudo
            if (UserName.Trim().Length < 3)
            {
                ErrorMessage = "Le nom d'utilisateur doit contenir au moins 3 caractères.";
                return false;
            }

            // 5. Force du mot de passe (Minimum 6 caractères)
            if (Password.Length < 6)
            {
                ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.";
                return false;
            }

            // 6. Correspondance des mots de passe
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "La confirmation du mot de passe ne correspond pas.";
                return false;
            }

            // 7. Format de l'email
            var authService = new AuthService();
            if (!authService.IsEmailValid(Email))
            {
                ErrorMessage = "Le format de l'e-mail est invalide.";
                return false;
            }

            return true;
        }
    }
}