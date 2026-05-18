using System.Net.Http;
using System.Text.Json;
using VibyApp.UI.Models;

namespace VibyApp.UI.Services
{
    public class DeezerService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<(List<Track> TopTracks, List<Artist> TopArtists)> GetTop50Async()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.deezer.com/chart/0?limit=50");
                var chartData = JsonSerializer.Deserialize<DeezerChartResponse>(response, JsonOptions);

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

                var searchResult = JsonSerializer.Deserialize<ChartSection<VibyApp.UI.Models.Track>>(response, JsonOptions);

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

        public async Task<List<string>> GetGenreNamesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.deezer.com/genre");
                using var doc = JsonDocument.Parse(response);

                var genres = new List<string>();
                foreach (var element in doc.RootElement.GetProperty("data").EnumerateArray())
                {
                    var name = element.GetProperty("name").GetString();
                    if (!string.IsNullOrWhiteSpace(name))
                        genres.Add(name);
                }

                return genres;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur Genres Deezer : {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<Track>> SearchTracksUiAsync(string query, int limit = 25)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return new List<Track>();

                var encodedQuery = Uri.EscapeDataString(query.Trim());
                var response = await _httpClient.GetStringAsync($"https://api.deezer.com/search?q={encodedQuery}&limit={limit}");
                var searchResult = JsonSerializer.Deserialize<DeezerSearchResponse>(response, JsonOptions);
                return searchResult?.Data ?? new List<Track>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur Recherche : {ex.Message}");
                return new List<Track>();
            }
        }
    }
}
