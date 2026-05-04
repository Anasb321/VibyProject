using System.Windows;
using System.Windows.Controls;
using VibyApp.DB.Repository;
using VibyApp.DB.Data;

namespace VibyApp.UI.Views
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
            var context = new VibyDbContext();
            var repository = new UserRepository(context);

            this.DataContext = new VibyApp.ViewModels.ProfileViewModel(repository);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.DataContext is VibyApp.UI.ViewModels.MainViewModel mainVM)
            { 
                mainVM.MoveToHomeCommand.Execute(null);
            }
        }
    }
}
