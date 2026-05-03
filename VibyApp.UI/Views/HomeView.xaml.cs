using System.Windows;
using System.Windows.Controls;
using VibyApp.ViewModels;
using VibyApp.API;

namespace VibyApp.UI.Views
{
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

        // --- LOGIQUE DE SCROLLING ---
        private void ScrollTracksLeft_Click(object sender, RoutedEventArgs e) =>
            TracksScroll.ScrollToHorizontalOffset(TracksScroll.HorizontalOffset - 600);

        private void ScrollTracksRight_Click(object sender, RoutedEventArgs e) =>
            TracksScroll.ScrollToHorizontalOffset(TracksScroll.HorizontalOffset + 600);

        private void ScrollArtistsLeft_Click(object sender, RoutedEventArgs e) =>
            ArtistsScroll.ScrollToHorizontalOffset(ArtistsScroll.HorizontalOffset - 600);

        private void ScrollArtistsRight_Click(object sender, RoutedEventArgs e) =>
            ArtistsScroll.ScrollToHorizontalOffset(ArtistsScroll.HorizontalOffset + 600);

        private void ScrollGenresLeft_Click(object sender, RoutedEventArgs e) =>
            GenresScroll.ScrollToHorizontalOffset(GenresScroll.HorizontalOffset - 600);

        private void ScrollGenresRight_Click(object sender, RoutedEventArgs e) =>
            GenresScroll.ScrollToHorizontalOffset(GenresScroll.HorizontalOffset + 600);

        private void Scroll_Changed(object sender, ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer sv)
            {
                if (sv == TracksScroll)
                {
                    TracksLeftBtn.IsEnabled = sv.HorizontalOffset > 0;
                    TracksRightBtn.IsEnabled = sv.HorizontalOffset < (sv.ExtentWidth - sv.ViewportWidth - 1);
                }
                else if (sv == ArtistsScroll)
                {
                    ArtistsLeftBtn.IsEnabled = sv.HorizontalOffset > 0;
                    ArtistsRightBtn.IsEnabled = sv.HorizontalOffset < (sv.ExtentWidth - sv.ViewportWidth - 1);
                }
                else if (sv == GenresScroll)
                {
                    GenresLeftBtn.IsEnabled = sv.HorizontalOffset > 0;
                    GenresRightBtn.IsEnabled = sv.HorizontalOffset < (sv.ExtentWidth - sv.ViewportWidth - 1);
                }
            }
        }
    }
}