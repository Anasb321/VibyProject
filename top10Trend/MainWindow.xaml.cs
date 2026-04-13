using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Top10Trend.Models;
using Top10Trend.Services;

namespace Top10Trend;
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
            
       
        this.DataContext = this;

  
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

    private async void RefreshButton(object sender, RoutedEventArgs e)
    {
        // 1. (Optional) Disable the button so the user doesn't click it twice
        var button = sender as Button;
        if (button != null) button.IsEnabled = false;

        try 
        {
            // 2. Fetch the new data
            var (tracks, artists) = await _deezerService.GetTop10Async();

            // 3. Clear the old data so the list refreshes completely
            TopTracks.Clear();
            TopArtists.Clear();

            // 4. Repopulate the lists
            foreach (var track in tracks)
            {
                TopTracks.Add(track);
            }

            foreach (var artist in artists)
            {
                TopArtists.Add(artist);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error refreshing data: {ex.Message}");
        }
        finally 
        {
            // 5. Re-enable the button
            if (button != null) button.IsEnabled = true;
        }
    }
}