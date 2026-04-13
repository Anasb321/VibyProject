using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Top10Trend.Models
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
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist")]
        public Artist Artist { get; set; }

        [JsonPropertyName("album")]
        public Album Album { get; set; }
        
        [JsonPropertyName("link")]
        public string linkTrack { get; set; }
    }

    public class Artist
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("picture_medium")]
        public string PictureUrl { get; set; }
        
        [JsonPropertyName("link")]
        public string linkArtist { get; set; }
    }

    public class Album
    {
        [JsonPropertyName("cover_small")]
        public string CoverUrl { get; set; }
    }
}