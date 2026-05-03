namespace VibyApp.API
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DeezerRechercher
    {
        private readonly HttpClient _httpClient = new HttpClient();

        
        public async Task<List<string>> SearchTracksAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<string>();

            try
            {
                string url = $"https://api.deezer.com/search?q={query}";
                string json = await _httpClient.GetStringAsync(url);

                using JsonDocument doc = JsonDocument.Parse(json);
                var resultats = new List<string>();

                foreach (var element in doc.RootElement.GetProperty("data").EnumerateArray())
                {  
                    string title = element.GetProperty("title").GetString();
                    string artist = element.GetProperty("artist").GetProperty("name").GetString();

                    resultats.Add($"{title} - {artist}");
                }

                return resultats;
            }
            catch (Exception)
            {
                return new List<string> { "Aucun résultat trouvé" };
            }
        }
    }
}