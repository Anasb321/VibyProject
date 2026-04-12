using CommunityToolkit.Mvvm.Input;

namespace VibyApp.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private string _firstName = "";
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }
        private string _lastName = "";
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _confirmPassword = "";
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private readonly MainViewModel _mainVM;

        public RegisterViewModel(MainViewModel mainVM) 
        {
            _mainVM = mainVM;
        }

        [RelayCommand]
        public void GoToLogin()
        {
            _mainVM.CurrentView = new LoginViewModel(_mainVM);
        }

        [RelayCommand]
        public void Register()
        {
            // On réinitialise le message d'erreur à chaque tentative d'inscription
            ErrorMessage = "";

            // 1. Vérification que les champs ne sont pas vides
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                return;
            }

            var authService = new VibyApp.Core.Services.AuthService();

            if (!authService.IsEmailValid(Email))
            {
                ErrorMessage = "L'adresse e-mail n'est pas valide.";
                return;
            }

            if (Password.Length < 8)
            {
                ErrorMessage = "Le mot de passe doit faire au moins 8 caractères.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Les mots de passe ne correspondent pas.";
                return;
            }

            try
            {
                // 6. Hachage du mot de passe (Sécurité avant tout)
                string hashedPassword = authService.HashPassword(Password);

                // Plus tard ici : appel au Repository pour save en Base de Données

                ErrorMessage = "Succès ! Le compte va être créé.";
                // Optionnel : Rediriger vers le login après 2 secondes
            }
            catch (Exception)
            {
                ErrorMessage = "Une erreur est survenue lors de l'inscription.";
            }
        }
    }
}
