using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibyApp.DB.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }
        public string PlaylistPicture { get; set; } = string.Empty;

        public List<Track> Tracks { get; set; } = new(); // Une playlist peut avoir plusieurs musiques
    }
}
