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
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddDbContext<VibyDbContext>(options =>
                options.UseSqlite("Data Source=viby.db"));

            services.AddSingleton<DeezerService>();
            services.AddSingleton<IAudioService, AudioService>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IFavoriteCoordinator, FavoriteCoordinator>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<PlayerBarViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<LikedViewModel>();
            services.AddTransient<LibraryViewModel>();

            services.AddTransient<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            _ = InitializeAppAsync(mainWindow);
        }

        private static async Task InitializeAppAsync(MainWindow mainWindow)
        {
            try
            {
                await Task.Run(() =>
                {
                    using var scope = ServiceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<VibyDbContext>();
                    dbContext.Database.Migrate();
                });

                if (mainWindow.DataContext is MainViewModel mainVm)
                    mainVm.ShowInitialView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur d'initialisation : {ex.Message}",
                    "Viby",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();

            base.OnExit(e);
        }
    }
}
