using System.Windows;
using System.Windows.Controls;
using VibyApp.ViewModels;

namespace VibyApp.UI.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void TogglePassword(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Visibility == Visibility.Visible)
            {
                // Passer en mode visible
                txtPasswordVisible.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
            }
            else
            {
                // Passer en mode caché
                txtPassword.Password = txtPasswordVisible.Text;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel vm)
            {
                // On récupère l'email
                vm.Email = txtEmail.Text;

                // On récupère le bon mot de passe selon quel champ est affiché
                string passwordToVerify = (txtPassword.Visibility == Visibility.Visible)
                    ? txtPassword.Password
                    : txtPasswordVisible.Text;

                // On lance l'action de login
                vm.LoginAction(passwordToVerify);
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