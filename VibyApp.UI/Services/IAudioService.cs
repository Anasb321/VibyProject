namespace VibyApp.UI.Services
{
    public interface IAudioService
    {
        bool IsPlaying { get; }
        bool IsShuffleActive { get; set; }
        bool IsRepeatActive { get; set; }
        double Volume { get; set; }

        void LoadTrack(string musicURL);
        void Play();
        void Pause();
        void Stop();
        void Next();
        void Previous();

        TimeSpan CurrentPosition { get; set; }
        TimeSpan TotalDuration { get; }

        event EventHandler? TrackFinished;
        event EventHandler? PositionChanged;
    }
}
