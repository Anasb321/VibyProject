using System.Windows;
using System.Windows.Controls;
using VibyApp.ViewModels;

namespace VibyApp.UI.Views
{
    public partial class LoginView : UserControl
    {

        private bool _showPassword = false;
        public LoginView()
        {
            InitializeComponent();
        }

        private void TogglePassword(object sender, RoutedEventArgs e)
        {
            if (!_showPassword)
            {
                txtPasswordVisible.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
                _showPassword = true;
            }
            else
            {
                txtPassword.Password = txtPasswordVisible.Text;

                txtPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                _showPassword = false;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel vm)
            {
                // 1. On donne l'email au ViewModel
                vm.Email = txtEmail.Text;

                // 2. On récupère le mot de passe du bon champ
                string password = _showPassword ? txtPasswordVisible.Text : txtPassword.Password;

                // 3. On demande au ViewModel de vérifier
                vm.LoginAction(password);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel vm)
            {
                vm.GoToRegister();
            }
        }
    }
}