using System.Net.Http;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Top10Trend.Models;

namespace Top10Trend.Services
{
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
                // L'ID '0' correspond au chart global
                var response = await _httpClient.GetStringAsync("https://api.deezer.com/chart/0/");
                
                var chartData = JsonSerializer.Deserialize<DeezerChartResponse>(response);

                // On s'assure de ne prendre que les 10 premiers
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
}