using System.Text.Json.Serialization;

namespace VibyApp.UI.Models
{
    public class DeezerChartResponse
    {
        [JsonPropertyName("tracks")]
        public ChartSection<Track> Tracks { get; set; } = new();

        [JsonPropertyName("artists")]
        public ChartSection<Artist> Artists { get; set; } = new();
    }

    public class ChartSection<T>
    {
        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new();
    }

    public class DeezerSearchResponse
    {
        [JsonPropertyName("data")]
        public List<Track> Data { get; set; } = new();
    }

    public class Track
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("preview")]
        public string Preview { get; set; } = string.Empty;

        [JsonPropertyName("artist")]
        public Artist Artist { get; set; } = new();

        [JsonPropertyName("album")]
        public Album Album { get; set; } = new();
    }

    public class Artist
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("picture_medium")]
        public string PictureUrl { get; set; } = string.Empty;
    }

    public class Album
    {
        [JsonPropertyName("cover_medium")]
        public string CoverUrl { get; set; } = string.Empty;
    }
}
