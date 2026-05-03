using System.Windows;
using System.Windows.Controls;
using VibyApp.UI.ViewModels;

namespace VibyApp.UI.Views
{
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.DataContext is MainViewModel mainVM)
            {
                mainVM.MoveToHomeCommand.Execute(null);
            }
        }
    }
}
