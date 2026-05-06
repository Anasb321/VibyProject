using System.Net.Http;
using System.Text.Json;
using VibyApp.UI.Models;

namespace VibyApp.UI.Services
{
    public class DeezerService
    {

        public string Preview { get; set; } = string.Empty;

        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<(List<Track> TopTracks, List<Artist> TopArtists)> GetTop50Async()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.deezer.com/chart/0/?limit=50");
                var chartData = JsonSerializer.Deserialize<DeezerChartResponse>(response);

                var topTracks = chartData?.Tracks?.Data ?? new List<Track>();
                var topArtists = chartData?.Artists?.Data ?? new List<Artist>();

                return (topTracks, topArtists);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur API Deezer : {ex.Message}");
                return (new List<Track>(), new List<Artist>());
            }
        }

        public async Task<List<VibyApp.DB.Models.Track>> SearchTracksAsync(string query)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(query);
                var response = await _httpClient.GetStringAsync($"https://api.deezer.com/search?q={encodedQuery}&limit=15");

                var searchResult = JsonSerializer.Deserialize<ChartSection<VibyApp.UI.Models.Track>>(response);

                if (searchResult?.Data == null) return new List<VibyApp.DB.Models.Track>();

                // On convertit les modèles "UI" (API) en modèles "DB" (SQLite)
                return searchResult.Data.Select(d => new VibyApp.DB.Models.Track
                {
                    DeezerId = d.Id,
                    Title = d.Title,
                    Artist = d.Artist?.Name ?? "Inconnu",
                    Duration = d.Duration,
                    TrackPicture = d.Album?.CoverUrl ?? "",
                    ArtistPicture = d.Artist?.PictureUrl ?? "",
                    PreviewUrl = d.Preview ?? ""
                }).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur Recherche : {ex.Message}");
                return new List<VibyApp.DB.Models.Track>();
            }
        }
    }
}
