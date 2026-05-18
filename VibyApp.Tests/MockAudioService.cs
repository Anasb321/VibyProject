using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibyApp.UI.Services;

namespace VibyApp.Tests
{
    public class MockAudioService : IAudioService
    {
        public bool IsPlaying { get; set; }
        public bool IsShuffleActive { get; set; }
        public bool IsRepeatActive { get; set; }
        public TimeSpan CurrentPosition { get; set; }
        public TimeSpan TotalDuration { get; set; }

        // Espions pour savoir si les méthodes ont été appelées
        public bool PlayCalled { get; private set; }
        public bool PauseCalled { get; private set; }
        public bool NextCalled { get; private set; }
        public bool PreviousCalled { get; private set; }

        public event EventHandler TrackFinished;

        public void LoadTrack(string trackUrl) { }

        public void Play()
        {
            PlayCalled = true;
            IsPlaying = true;
        }

        public void Pause()
        {
            PauseCalled = true;
            IsPlaying = false;
        }

        public void Next() => NextCalled = true;
        public void Previous() => PreviousCalled = true;
    }
}