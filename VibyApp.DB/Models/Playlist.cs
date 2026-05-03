using System.ComponentModel.DataAnnotations;


namespace VibyApp.DB.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }
        public string PlaylistPicture { get; set; } = string.Empty;

        public List<Track> Tracks { get; set; } = new(); // Une playlist peut avoir plusieurs musiques
    }
}