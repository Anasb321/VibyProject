using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        public void LoginAction(string password)
        {
            if (Email == "test@gmail.com" && password == "1234")
            {
                ErrorMessage = "";
                _mainVM.CurrentView = new HomeViewModel();
            }
            else
            {
                ErrorMessage = "Email ou mot de passe invalide";
            }
        }

        [RelayCommand]
        public void GoToRegister()
        {
            _mainVM.CurrentView = new RegisterViewModel(_mainVM);
        }
    }
}
