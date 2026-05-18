using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VibyApp.UI.ViewModels;

namespace VibyApp.Tests
{
    public class PlayerBarViewModelTests
    {
        [Fact]
        public void ExecuteTogglePlayPause_WhenNotPlaying_ShouldCallPlayAndSetIsPlayingToTrue()
        {
            // Arrange : On prépare le terrain
            var mockAudio = new MockAudioService { IsPlaying = false };
            var viewModel = new PlayerBarViewModel(mockAudio);

            // Act : On exécute l'action de l'utilisateur (le clic sur le bouton)
            viewModel.TogglePlayPauseCommand.Execute(null);

            // Assert : On vérifie que le comportement est le bon
            Assert.True(mockAudio.PlayCalled, "La méthode Play() du service audio aurait dû être appelée.");
            Assert.True(viewModel.IsPlaying, "La propriété IsPlaying du ViewModel devrait être à true.");
        }

        [Fact]
        public void ExecuteTogglePlayPause_WhenPlaying_ShouldCallPauseAndSetIsPlayingToFalse()
        {
            // Arrange
            var mockAudio = new MockAudioService { IsPlaying = true };
            var viewModel = new PlayerBarViewModel(mockAudio);
            viewModel.IsPlaying = true;

            // Act
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