using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibyApp.DB.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string TrackPicture { get; set; } = string.Empty;
        public string ArtistPicture { get; set; } = string.Empty;

        public List<Playlist> Playlists { get; set; } = new(); // Un track peut etre dans plusieurs playlist a la fois
    }
}
