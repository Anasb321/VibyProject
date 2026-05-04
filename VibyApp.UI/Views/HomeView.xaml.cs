using System.Windows;
using System.Windows.Controls;

namespace VibyApp.UI.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
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
<<<<<<< HEAD
}

public partial class HomeView : UserControl, INotifyPropertyChanged
{
    private readonly DeezerService _deezerService;

    private bool _isLoading;

    public HomeView()
    {
        InitializeComponent();

        _deezerService = new DeezerService();
        TopTracks = new ObservableCollection<Track>();
        TopArtists = new ObservableCollection<Artist>();

        DataContext = this;
        Loaded += HomeView_Loaded;
    }

    public ObservableCollection<Track> TopTracks { get; set; }
    public ObservableCollection<Artist> TopArtists { get; set; }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    private async void HomeView_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            IsLoading = true;
            var (tracks, artists) = await _deezerService.GetTop10Async();
            
            TopTracks.Clear();
            foreach (var track in tracks) TopTracks.Add(track);

            TopArtists.Clear();
            foreach (var artist in artists) TopArtists.Add(artist);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            IsLoading = false;
        }
    } 

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 
public class DeezerChartResponse
{
    [JsonPropertyName("tracks")] public ChartSection<Track> Tracks { get; set; }

    [JsonPropertyName("artists")] public ChartSection<Artist> Artists { get; set; }
}

public class ChartSection<T>
{
    [JsonPropertyName("data")] public List<T> Data { get; set; }
}

public class Track
{
    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("artist")] public Artist Artist { get; set; }

    [JsonPropertyName("album")] public Album Album { get; set; }

    [JsonPropertyName("link")] public string LinkTrack { get; set; }
}

public class Artist
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("picture_medium")] public string PictureUrl { get; set; }

    [JsonPropertyName("link")] public string LinkArtist { get; set; }
}

public class Album
{
    [JsonPropertyName("cover_medium")] public string CoverUrl { get; set; }
=======
>>>>>>> 929c4447f7a40b91e774002a5d8e373a0b7c49a5
}