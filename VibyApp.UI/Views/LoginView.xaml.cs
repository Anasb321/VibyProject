using System.Windows;
using System.Windows.Controls;
using VibyApp.UI.ViewModels;

namespace VibyApp.UI.Views
{
    public partial class LoginView : UserControl
    {

        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Alterne entre l'affichage masqué (PasswordBox) et clair (TextBox) du mot de passe.
        /// </summary>
        private void TogglePassword(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Visibility == Visibility.Visible)
            {
                txtPasswordVisible.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
            }
            else // Sinon, on revient au mode masqué
            {
                txtPassword.Password = txtPasswordVisible.Text;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Déclenche la logique de connexion du ViewModel.
        /// </summary>
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel vm)
            {
                // On récupère le mot de passe du champ actuellement affiché
                string password = (txtPassword.Visibility == Visibility.Visible)
                    ? txtPassword.Password
                    : txtPasswordVisible.Text;

                await vm.LoginAction(password);
            }
        }

        /// <summary>
        /// Redirige l'utilisateur vers la vue d'inscription.
        /// </summary>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LoginViewModel vm)
            {
                vm.GoToRegister();
            }
        }
    }
}