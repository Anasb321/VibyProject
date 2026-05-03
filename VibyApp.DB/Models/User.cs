using System.ComponentModel.DataAnnotations;

namespace VibyApp.DB.Models
{

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(75)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(75)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(75)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required] // Vu qu'on vas mettre des mot de passe Haché on donne pas de max length car ils peuvent etre tres long
        public string MotDePasse { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }

        public List<Playlist> Playlists { get; set; } = new(); // Un user peut avoir plusieurs playlist

        public List<Track> FavoriteTracks { get; set; } = new();

        public List<Artist> FavoriteArtists { get; set; } = new();
    }
}