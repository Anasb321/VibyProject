namespace VibyApp.API
{
    using System.Net.Http;
    using System.Text.Json;

    public class DeezerRechercher
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<List<string>> GetGenresAsync()
        {
            try
            {
                string json = await _httpClient.GetStringAsync("https://api.deezer.com/genre");

                using JsonDocument doc = JsonDocument.Parse(json);
                var genres = new List<string>();

                foreach (var element in doc.RootElement.GetProperty("data").EnumerateArray())
                {
                    genres.Add(element.GetProperty("name").GetString() ?? string.Empty);
                }

                return genres;
            }
            catch (Exception)
            {
                return new List<string> { "Erreur de chargement" };
            }
        }
    }
}
