using VibyApp.DB.Models;
using VibyApp.UI.Models;
using VibyApp.UI.Services;
using VibyApp.UI.ViewModels;
using Xunit;
using UiTrack = VibyApp.UI.Models.Track;

namespace VibyApp.Tests
{
    public class PlayerBarViewModelTests
    {
        private static UiTrack SampleTrack => new()
        {
            Id = 1,
            Title = "Test",
            Preview = "https://example.com/preview.mp3",
            Duration = 30,
            Artist = new VibyApp.UI.Models.Artist { Name = "Artist" }
        };

        private static PlayerBarViewModel CreateViewModel(MockAudioService mockAudio) =>
            new(mockAudio, new StubFavoriteCoordinator(), new StubCurrentUserService());

        [Fact]
        public void ExecuteTogglePlayPause_WhenNotPlaying_ShouldCallPlayAndSetIsPlayingToTrue()
        {
            var mockAudio = new MockAudioService { IsPlaying = false };
            var viewModel = CreateViewModel(mockAudio);
            viewModel.PlayTrack(SampleTrack);
            mockAudio.Pause();
            viewModel.IsPlaying = false;

            viewModel.TogglePlayPauseCommand.Execute(null);

            Assert.True(mockAudio.PlayCalled);
            Assert.True(viewModel.IsPlaying);
        }

        [Fact]
        public void ExecuteTogglePlayPause_WhenPlaying_ShouldCallPauseAndSetIsPlayingToFalse()
        {
            var mockAudio = new MockAudioService { IsPlaying = true };
            var viewModel = CreateViewModel(mockAudio);
            viewModel.PlayTrack(SampleTrack);

            viewModel.TogglePlayPauseCommand.Execute(null);

            Assert.True(mockAudio.PauseCalled);
            Assert.False(viewModel.IsPlaying);
        }

        [Fact]
        public void ExecuteToggleShuffle_ShouldToggleActiveState()
        {
            var mockAudio = new MockAudioService { IsShuffleActive = false };
            var viewModel = CreateViewModel(mockAudio);

            viewModel.ToggleShuffleCommand.Execute(null);

            Assert.True(viewModel.IsShuffleActive);
            Assert.True(mockAudio.IsShuffleActive);
        }

        private sealed class StubFavoriteCoordinator : IFavoriteCoordinator
        {
            public event Action<long, bool>? FavoriteChanged;
            public Task LoadFromDatabaseAsync() => Task.CompletedTask;
            public void Clear() { }
            public bool IsFavorite(long deezerId) => false;
            public Task<bool> ToggleAsync(UiTrack track) => Task.FromResult(true);
        }

        private sealed class StubCurrentUserService : ICurrentUserService
        {
            public User? CurrentUser { get; private set; } = new User { Id = 1 };
            public void SetUser(User user) => CurrentUser = user;
            public void ClearUser() => CurrentUser = null;
        }
    }
}
