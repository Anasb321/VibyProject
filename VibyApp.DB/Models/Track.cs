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

        public long DeezerId { get; set; } // Id de la musique dans Deezer pour pouvoir faire le lien entre notre base de données et l'api de Deezer

        public string PreviewUrl { get; set; } = string.Empty; // Url de la preview de la musique pour pouvoir l'écouter dans notre application
        public List<User> FavoritedByUsers { get; set; } = new();
    }
}