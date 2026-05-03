using Microsoft.EntityFrameworkCore;
using VibyApp.DB.Models;
using VibyApp.DB.Services;

namespace VibyApp.DB.Data
{
    public class VibyDbContext : DbContext
    {

        public VibyDbContext(DbContextOptions<VibyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Artist> Artists { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=viby.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Tracks)
                .WithMany(t => t.Playlists);

            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.User)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteTracks)
                .WithMany(t => t.FavoritedByUsers)
                .UsingEntity(j => j.ToTable("UserFavoriteTracks"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteArtists)
                .WithMany(a => a.FollowedByUsers)
                .UsingEntity(j => j.ToTable("UserFavoriteArtists"));

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "Administrateur",
                LastName = "Vibe",
                UserName = "admin",
                Email = "test@gmail.com",
                //MotDePasse = "1234",
                MotDePasse = HashageService.HacherMDP("1234"),
                IsAdmin = true
            });
        }
    }
}