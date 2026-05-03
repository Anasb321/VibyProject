using System.Windows;
using System.Windows.Controls;
using VibyApp.ViewModels;
using VibyApp.API;

namespace VibyApp.UI.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private readonly DeezerRechercher rechercheService = new DeezerRechercher();
        public HomeView()
        {
            InitializeComponent();

            this.DataContext = new VibyApp.ViewModels.HomeViewModel();
        }
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox searchBox && searchBox.Text == "Recherche")
            {
                searchBox.Text = string.Empty;
            }
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox searchBox)
            {
                string query = searchBox.Text;

     
                if (!string.IsNullOrWhiteSpace(query) && query != "Recherche" && query.Length >= 3)
                {
                    var resultats = await rechercheService.SearchTracksAsync(query);

                    
                    GenresList.ItemsSource = resultats;
                }
            }
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
