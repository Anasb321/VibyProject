using System.Collections.ObjectModel;
using VibyApp.UI.Models;
using VibyApp.UI.Services;
using VibyProject;
namespace VibyApp.UI.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly DeezerService _deezerService;
        private bool _isLoading;

        public ObservableCollection<Track> TopTracks { get; set; } = new();
        public ObservableCollection<Artist> TopArtists { get; set; } = new();

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public HomeViewModel(DeezerService deezerService)
        {
            _deezerService = deezerService;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                var (tracks, artists) = await _deezerService.GetTop50Async();

                // On s'assure de modifier la collection sur le thread UI
                App.Current.Dispatcher.Invoke(() =>
                {
                    TopTracks.Clear();
                    foreach (var t in tracks) TopTracks.Add(t);
                    TopArtists.Clear();
                    foreach (var a in artists) TopArtists.Add(a);
                });
            }
            finally { IsLoading = false; }
        }
    }
}