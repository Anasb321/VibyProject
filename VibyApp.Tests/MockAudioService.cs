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
        public double Volume { get; set; } = 70;

        public bool PlayCalled { get; private set; }
        public bool PauseCalled { get; private set; }
        public bool NextCalled { get; private set; }
        public bool PreviousCalled { get; private set; }

#pragma warning disable CS0067
        public event EventHandler? TrackFinished;
        public event EventHandler? PositionChanged;
#pragma warning restore CS0067

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

        public void Stop() => IsPlaying = false;

        public void Next() => NextCalled = true;

        public void Previous() => PreviousCalled = true;
    }
}
