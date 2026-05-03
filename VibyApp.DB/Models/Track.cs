using System.ComponentModel.DataAnnotations; 

namespace VibyApp.DB.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Artist { get; set; } = string.Empty;

        [Required]
        [Range(1, 1200, ErrorMessage = "La durée doit être comprise entre 1 seconde et 20 minutes.")]
        public int Duration { get; set; }

        public string TrackPicture { get; set; } = string.Empty;
        public string ArtistPicture { get; set; } = string.Empty;

        public List<Playlist> Playlists { get; set; } = new(); // Un track peut etre dans plusieurs playlist a la fois
        public List<User> FavoritedByUsers { get; set; } = new();
    }
}
