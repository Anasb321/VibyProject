using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using VibyApp.DB.Data;
using VibyApp.DB.Repository;
using VibyApp.UI.Services;
using VibyApp.UI.ViewModels;
using VibyApp.UI.Views;

namespace VibyProject
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // --- Base de données ---
            services.AddDbContext<VibyDbContext>(options =>
            {
                options.UseSqlite("Data Source=viby.db");
            });

            // --- SERVICES ---
            services.AddSingleton<DeezerService>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();

            // --- VIEWMODELS ---
            services.AddSingleton<MainViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<ProfileViewModel>();

            // --- VUES ---
            services.AddTransient<MainWindow>();

            // Construction du Provider
            ServiceProvider = services.BuildServiceProvider();

            // Migration de la DB
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<VibyDbContext>();
                dbContext.Database.Migrate();
            }

            // Lancement propre
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

            // On s'assure que le DataContext est bien le MainViewModel injecté
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();

            base.OnExit(e);
        }
    }
}