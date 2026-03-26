using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp
{
    class InscriptionViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _userName;
        private string _email;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(nameof(UserName)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public ICommand InscriptionCommand { get; }

        public InscriptionViewModel()
        {
            InscriptionCommand = new RelayCommand(ExecuterInscription, PeutSInscrire);
        }

        private bool PeutSInscrire(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(UserName) &&
                   !string.IsNullOrWhiteSpace(Name);
        }

        private void ExecuterInscription(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string passwordSaisi = passwordBox?.Password;

            if (string.IsNullOrWhiteSpace(passwordSaisi) || passwordSaisi.Length < 4)
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 4 caractères.");
                return;
            }

            string passwordHash = HashageService.HacherMDP(passwordSaisi);

            Utilisateur nouvelUtilisateur = new Utilisateur
            {
                Name = this.Name,
                UserName = this.UserName,
                Email = this.Email,
                MotDePasse = passwordHash
            };

            MessageBox.Show($"Inscription réussie pour {nouvelUtilisateur.Name} !");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}