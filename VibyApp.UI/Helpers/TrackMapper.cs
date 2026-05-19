using UiTrack = VibyApp.UI.Models.Track;
using DbTrack = VibyApp.DB.Models.Track;

namespace VibyApp.UI.Helpers
{
    public static class TrackMapper
    {
        public static DbTrack ToDbTrack(UiTrack track) => new()
        {
            DeezerId = track.Id,
            Title = track.Title,
            Artist = track.Artist?.Name ?? "Inconnu",
            Duration = track.Duration,
            TrackPicture = track.Album?.CoverUrl ?? string.Empty,
            ArtistPicture = track.Artist?.PictureUrl ?? string.Empty,
            PreviewUrl = track.Preview ?? string.Empty
        };

        public static UiTrack ToUiTrack(DbTrack track) => new()
        {
            Id = track.DeezerId,
            Title = track.Title,
            Duration = track.Duration,
            Preview = track.PreviewUrl,
            Artist = new Models.Artist
            {
                Name = track.Artist,
                PictureUrl = track.ArtistPicture
            },
            Album = new Models.Album
            {
                CoverUrl = track.TrackPicture
            }
        };
    }
}
