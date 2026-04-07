using System.Windows;

namespace VibyApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        bool showPassword = false;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void TogglePassword(object sender, RoutedEventArgs e)
        {
            if (!showPassword)
            {
                txtPasswordVisible.Text = txtPassword.Password;

                txtPassword.Visibility = Visibility.Hidden;
                txtPasswordVisible.Visibility = Visibility.Visible;

                showPassword = true;
            }
            else
            {
                txtPassword.Password = txtPasswordVisible.Text;

                txtPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Hidden;

                showPassword = false;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (email == "test@gmail.com" && password == "1234")
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                lblError.Text = "Email ou mot de passe invalide";
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            InscriptionWindow inscription = new InscriptionWindow();
            inscription.Show();
            this.Close();
        }
    }
}