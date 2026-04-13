using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using VibyApp.Core.Services;
using VibyApp.DB.Data;
using VibyApp.DB.Models;
using VibyApp.DB.Repository;

namespace VibyApp.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
       
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
        public async Task Register()
        {
            ErrorMessage = "";

            // 1. Validation de base
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Les mots de passe ne correspondent pas.";
                return;
            }

            var authService = new AuthService();

            if (!authService.IsEmailValid(Email))
            {
                ErrorMessage = "L'adresse e-mail n'est pas valide.";
                return;
            }

            try
            {
                // 2. Initialisation de la DB et du Repo
                var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<VibyDbContext>();
                optionsBuilder.UseSqlite("Data Source=VibyApp.db");
                using var dbContext = new VibyDbContext(optionsBuilder.Options);
                dbContext.Database.EnsureCreated();
                var userRepository = new UserRepository(dbContext);

                // 3. Vérification si l'utilisateur existe déjà
                // Utilisation des méthodes synchrones présentes dans ton UserRepository
                if (userRepository.ExisteDeja(UserName, Email))
                {
                    ErrorMessage = "Le nom d'utilisateur ou l'email est déjà utilisé.";
                    return;
                }

                // 4. Création de l'utilisateur
                var newUser = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    UserName = UserName,
                    Email = Email,
                    MotDePasse = Password 
                };

                // 5. Sauvegarde
                userRepository.Ajouter(newUser);

                ErrorMessage = "Succès ! Redirection vers la connexion...";

                await Task.Delay(2000); // Petite pause pour lire le message
                GoToLogin();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur technique : " + ex.Message;
            }
        }
    }
}