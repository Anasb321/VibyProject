using System.Collections.ObjectModel;
using System.Windows;
using Top10Trend.Models;
using Top10Trend.Services;

namespace top10Trend.ViewModel;

public partial class MainWindow : Window
{
    private readonly DeezerService _deezerService;

    public ObservableCollection<Track> TopTracks { get; set; }
    public ObservableCollection<Artist> TopArtists { get; set; }

    public MainWindow()
    {
        _deezerService = new DeezerService();
            
        TopTracks = new ObservableCollection<Track>();
        TopArtists = new ObservableCollection<Artist>();
            
        // On définit le contexte de données pour le Binding XAML
        this.DataContext = this;

        // Appel asynchrone lors du chargement de la fenêtre
        this.Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var (tracks, artists) = await _deezerService.GetTop10Async();

        foreach (var track in tracks)
        {
            TopTracks.Add(track);
        }

        foreach (var artist in artists)
        {
            TopArtists.Add(artist);
        }
    }
}