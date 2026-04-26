using System.Text.Json.Serialization;
using System.Windows.Controls;

namespace VibyApp.UI.Views;

public partial class MediaBarView : UserControl
{
    public class DeezerChartResponse
    {
        [JsonPropertyName("tracks")] public ChartSection<Track> Tracks { get; set; }
    }
}