using System.Windows.Media;
using System.Windows.Threading;

namespace VibyApp.UI.Services
{
    public class AudioService : IAudioService, IDisposable
    {
        private readonly MediaPlayer _mediaPlayer = new();
        private readonly DispatcherTimer _positionTimer;
        private bool _playWhenReady;

        public bool IsPlaying { get; private set; }
        public bool IsShuffleActive { get; set; }
        public bool IsRepeatActive { get; set; }
        public TimeSpan CurrentPosition
        {
            get => _mediaPlayer.Position;
            set
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                    _mediaPlayer.Position = value;
            }
        }

        public TimeSpan TotalDuration { get; private set; }

        public double Volume
        {
            get => _mediaPlayer.Volume * 100;
            set => _mediaPlayer.Volume = Math.Clamp(value, 0, 100) / 100.0;
        }

        public event EventHandler? TrackFinished;
        public event EventHandler? PositionChanged;

        public AudioService()
        {
            _mediaPlayer.MediaOpened += (_, _) =>
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                    TotalDuration = _mediaPlayer.NaturalDuration.TimeSpan;

                if (_playWhenReady)
                    StartPlayback();
            };

            _mediaPlayer.MediaEnded += (_, _) =>
            {
                IsPlaying = false;
                TrackFinished?.Invoke(this, EventArgs.Empty);
            };

            _positionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _positionTimer.Tick += (_, _) => PositionChanged?.Invoke(this, EventArgs.Empty);

            Volume = 70;
        }

        public void LoadTrack(string musicURL)
        {
            StopInternal();
            if (string.IsNullOrWhiteSpace(musicURL))
            {
                TotalDuration = TimeSpan.Zero;
                return;
            }

            _playWhenReady = false;
            _mediaPlayer.Open(new Uri(musicURL, UriKind.Absolute));
        }

        public void Play()
        {
            if (_mediaPlayer.Source == null)
                return;

            _playWhenReady = true;
            if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                StartPlayback();
        }

        private void StartPlayback()
        {
            _playWhenReady = false;
            _mediaPlayer.Play();
            IsPlaying = true;
            _positionTimer.Start();
        }

        public void Pause()
        {
            _mediaPlayer.Pause();
            IsPlaying = false;
            _positionTimer.Stop();
        }

        public void Next() { }

        public void Previous() { }

        public void Stop()
        {
            StopInternal();
        }

        private void StopInternal()
        {
            _positionTimer.Stop();
            _mediaPlayer.Stop();
            _mediaPlayer.Close();
            IsPlaying = false;
            TotalDuration = TimeSpan.Zero;
        }

        public void Dispose()
        {
            _positionTimer.Stop();
            _mediaPlayer.Close();
        }
    }
}
