using System.Windows;
using System.Windows.Controls;
using VibyApp.ViewModels;

namespace VibyApp.UI.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.DataContext is MainViewModel mainVM)
            {
                mainVM.NavigateToProfile();
            }
        }
    }
}
