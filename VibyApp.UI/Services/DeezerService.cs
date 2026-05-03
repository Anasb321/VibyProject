using System.Net.Http;
using System.Text.Json;
using VibyApp.UI.Models;

namespace VibyApp.UI.Services
{
    public class DeezerService
    {
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
    }
}
