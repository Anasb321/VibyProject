using System;
using System.Windows.Input;
using System.Windows.Threading;
using VibyApp.UI.Services;
using VibyApp.UI.Commands;
using VibyApp.ViewModels;

namespace VibyApp.UI.ViewModels
{
    public class PlayerBarViewModel : BaseViewModel
    {
        private readonly IAudioService _audioService;
        private readonly DispatcherTimer _progressTimer;
        private bool _isPlaying;
        private double _timecurrentposition;
        private double _totalDuration;
        private bool _isShuffleActive;
        private bool _isRepeatActive;

        public bool IsPlaying
        {
            get => _isPlaying;
            set { _isPlaying = value; OnPropertyChanged(); }
        }

        public double TimeCurrentPosition
        {
            get => _timecurrentposition;
            set { _timecurrentposition = value; OnPropertyChanged(); }
        }

        public double TotalDuration
        {
            get => _totalDuration;
            set { _totalDuration = value; OnPropertyChanged(); }
        }

        public bool IsShuffleActive
        {
            get => _isShuffleActive;
            set { _isShuffleActive = value; OnPropertyChanged(); }
        }

        public bool IsRepeatActive
        {
            get => _isRepeatActive;
            set { _isRepeatActive = value; OnPropertyChanged(); }
        }

        public ICommand TogglePlayPauseCommand { get; }
        public ICommand NextTrackCommand { get; }
        public ICommand PreviousTrackCommand { get; }
        public ICommand ToggleShuffleCommand { get; }
        public ICommand ToggleRepeatCommand { get; }

        public PlayerBarViewModel(IAudioService audioService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));

            TogglePlayPauseCommand = new RelayCommand(_ => ExecuteTogglePlayPause());
            NextTrackCommand = new RelayCommand(_ => ExecuteNextTrack());
            PreviousTrackCommand = new RelayCommand(_ => ExecutePreviousTrack());
            ToggleShuffleCommand = new RelayCommand(_ => ExecuteToggleShuffle());
            ToggleRepeatCommand = new RelayCommand(_ => ExecuteToggleRepeat());

            _progressTimer = new DispatcherTimer();
            _progressTimer.Interval = TimeSpan.FromSeconds(1);
            _progressTimer.Tick += ProgressTimer_Tick;

            if (_audioService.IsPlaying)
            {
                _progressTimer.Start();
            }
        }

        private void ProgressTimer_Tick(object? sender, EventArgs e)
        {
            TimeCurrentPosition = _audioService.CurrentPosition.TotalSeconds;
            TotalDuration = _audioService.TotalDuration.TotalSeconds;
        }

        private void ExecuteTogglePlayPause()
        {
            if (_audioService.IsPlaying)
            {
                _audioService.Pause();
                _progressTimer.Stop();
            }
            else
            {
                _audioService.Play();
                _progressTimer.Start();
            }

            IsPlaying = _audioService.IsPlaying;
        }

        private void ExecuteNextTrack()
        {
            _audioService.Next();
            IsPlaying = _audioService.IsPlaying;
        }

        private void ExecutePreviousTrack()
        {
            _audioService.Previous();
            IsPlaying = _audioService.IsPlaying;
        }

        private void ExecuteToggleShuffle()
        {
            _audioService.IsShuffleActive = !_audioService.IsShuffleActive;
            IsShuffleActive = _audioService.IsShuffleActive;
        }

        private void ExecuteToggleRepeat()
        {
            _audioService.IsRepeatActive = !_audioService.IsRepeatActive;
            IsRepeatActive = _audioService.IsRepeatActive;
        }
    }
}