using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using VibyApp.DB.Data;
using VibyApp.DB.Repository;
using VibyApp.DB.Services;

namespace VibyApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;

        private string _email = "";
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        public LoginViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
        }

        public async Task LoginAction(string password)
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                return;
            }

            try
            {
                var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<VibyDbContext>();
                optionsBuilder.UseSqlite("Data Source=VibyApp.db");

                using var dbContext = new VibyDbContext(optionsBuilder.Options);
                var userRepository = new UserRepository(dbContext);

                var userFound = userRepository.ObtenirParEmail(Email);

                if (userFound != null)
                {
                    var isConnected = userRepository.VerifierConnexion(userFound.UserName, password);

                    if (isConnected != null)
                    {
                        
                        _mainVM.CurrentView = new HomeViewModel();
                    }
                    else
                    {
                        ErrorMessage = "Mot de passe ou email incorrect.";
                    }
                }
                else
                {
                    ErrorMessage = "Mot de passe ou email incorrect.";
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = "Erreur technique : " + ex.Message;
            }
        }

        [RelayCommand]
        public void GoToRegister()
        {
            _mainVM.CurrentView = new RegisterViewModel(_mainVM);
        }
    }
}
