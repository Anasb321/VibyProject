using System.Text.Json.Serialization;

namespace VibyApp.UI.Models
{
        public class DeezerChartResponse
        {
            [JsonPropertyName("tracks")]
            public ChartSection<Track> Tracks { get; set; }

            [JsonPropertyName("artists")]
            public ChartSection<Artist> Artists { get; set; }
        }

        public class ChartSection<T>
        {
            [JsonPropertyName("data")]
            public List<T> Data { get; set; }
        }

        public class Track
        {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("preview")]
        public string Preview { get; set; }

        [JsonPropertyName("artist")]
        public Artist Artist { get; set; }

        [JsonPropertyName("album")]
        public Album Album { get; set; }
    }

        public class Artist
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("picture_medium")]
            public string PictureUrl { get; set; }
    }

        public class Album
        {
            [JsonPropertyName("cover_medium")]
            public string CoverUrl { get; set; }
        }
}
