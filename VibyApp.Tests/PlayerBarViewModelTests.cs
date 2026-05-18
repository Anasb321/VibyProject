using VibyApp.UI.Models;
using VibyApp.UI.ViewModels;
using Xunit;

namespace VibyApp.Tests
{
    public class PlayerBarViewModelTests
    {
        private static Track SampleTrack => new()
        {
            Id = 1,
            Title = "Test",
            Preview = "https://example.com/preview.mp3",
            Duration = 30,
            Artist = new Artist { Name = "Artist" }
        };

        [Fact]
        public void ExecuteTogglePlayPause_WhenNotPlaying_ShouldCallPlayAndSetIsPlayingToTrue()
        {
            var mockAudio = new MockAudioService { IsPlaying = false };
            var viewModel = new PlayerBarViewModel(mockAudio);
            viewModel.PlayTrack(SampleTrack);
            mockAudio.Pause();
            viewModel.IsPlaying = false;

            viewModel.TogglePlayPauseCommand.Execute(null);

            // Assert : On vérifie que le comportement est le bon
            Assert.True(mockAudio.PlayCalled, "La méthode Play() du service audio aurait dû être appelée.");
            Assert.True(viewModel.IsPlaying, "La propriété IsPlaying du ViewModel devrait être à true.");
        }

        [Fact]
        public void ExecuteTogglePlayPause_WhenPlaying_ShouldCallPauseAndSetIsPlayingToFalse()
        {
            var mockAudio = new MockAudioService { IsPlaying = true };
            var viewModel = new PlayerBarViewModel(mockAudio);
            viewModel.PlayTrack(SampleTrack);

            viewModel.TogglePlayPauseCommand.Execute(null);

            // Assert
            Assert.True(mockAudio.PauseCalled, "La méthode Pause() du service audio aurait dû être appelée.");
            Assert.False(viewModel.IsPlaying, "La propriété IsPlaying du ViewModel devrait être à false.");
        }

        [Fact]
        public void ExecuteToggleShuffle_ShouldToggleActiveState()
        {
            // Arrange
            var mockAudio = new MockAudioService { IsShuffleActive = false };
            var viewModel = new PlayerBarViewModel(mockAudio);

            // Act
            viewModel.ToggleShuffleCommand.Execute(null);

            // Assert
            Assert.True(viewModel.IsShuffleActive, "Le mode Shuffle devrait être actif dans le ViewModel.");
            Assert.True(mockAudio.IsShuffleActive, "Le mode Shuffle devrait être actif dans le service audio.");
        }
    }
}