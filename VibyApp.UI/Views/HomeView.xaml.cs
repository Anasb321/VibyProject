using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace VibyApp.UI.Views;

public class DeezerService
{
    private readonly HttpClient _httpClient;

    public DeezerService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<(List<Track> TopTracks, List<Artist> TopArtists)> GetTop10Async()
    {
        try
        {
            var response = await _httpClient.GetStringAsync("https://api.deezer.com/chart/0/");
            var chartData = JsonSerializer.Deserialize<DeezerChartResponse>(response);

            var topTracks = chartData?.Tracks?.Data?.Take(10).ToList() ?? new List<Track>();
            var topArtists = chartData?.Artists?.Data?.Take(10).ToList() ?? new List<Artist>();

            return (topTracks, topArtists);
        }
        catch
        {
            return (new List<Track>(), new List<Artist>());
        }
    }
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
}