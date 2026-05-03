using System.Collections.ObjectModel;
using System.Windows.Controls;
using VibyApp.API;


namespace VibyApp.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly DeezerGenre deezerGenre =  new DeezerGenre();
        public ObservableCollection<string> Genres { get; set; } = new ObservableCollection<string>();

        public HomeViewModel() 
       {
            LoadGenres();
       }
        private async void LoadGenres()
        {
            var genres = await deezerGenre.GetGenresAsync();
            foreach (var genre in genres)
            {
                System.Diagnostics.Debug.WriteLine($"API Deezer - Genre reçu : {genre}");
                Genres.Add(genre);
            }

        }
    }
}
