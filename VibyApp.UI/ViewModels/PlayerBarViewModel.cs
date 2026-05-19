using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VibyApp.UI.Commands;
using VibyApp.UI.Models;
using VibyApp.UI.Services;

namespace VibyApp.UI.ViewModels
{
    public class PlayerBarViewModel : BaseViewModel
    {
        private readonly IAudioService _audioService;
        private readonly IFavoriteCoordinator _favoriteCoordinator;
        private readonly ICurrentUserService _currentUserService;
        private List<Track> _queue = new();
        private int _currentIndex = -1;
        private Track? _currentTrack;

        private bool _isPlaying;
        private double _timeCurrentPosition;
        private double _totalDuration;
        private bool _isShuffleActive;
        private bool _isRepeatActive;
        private string _currentTrackName = string.Empty;
        private string _currentArtistName = string.Empty;
        private double _volume = 70;
        private ImageSource? _coverImage;
        private bool _isCurrentTrackFavorite;

        public event Action? PlaybackStarted;

        public bool IsPlaying
        {
            get => _isPlaying;
            set { _isPlaying = value; OnPropertyChanged(); }
        }

        public double TimeCurrentPosition
        {
            get => _timeCurrentPosition;
            set
            {
                if (Math.Abs(_timeCurrentPosition - value) < 0.5)
                    return;

                _timeCurrentPosition = value;
                OnPropertyChanged();
                _audioService.CurrentPosition = TimeSpan.FromSeconds(value);
            }
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

        public string CurrentTrackName
        {
            get => _currentTrackName;
            set { _currentTrackName = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasActiveTrack)); }
        }

        public string CurrentArtistName
        {
            get => _currentArtistName;
            set { _currentArtistName = value; OnPropertyChanged(); }
        }

        public ImageSource? CoverImage
        {
            get => _coverImage;
            set { _coverImage = value; OnPropertyChanged(); }
        }

        public double Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _audioService.Volume = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentTrackFavorite
        {
            get => _isCurrentTrackFavorite;
            private set { _isCurrentTrackFavorite = value; OnPropertyChanged(); }
        }

        public bool HasActiveTrack => !string.IsNullOrEmpty(CurrentTrackName);

        public ICommand TogglePlayPauseCommand { get; }
        public ICommand NextTrackCommand { get; }
        public ICommand PreviousTrackCommand { get; }
        public ICommand ToggleShuffleCommand { get; }
        public ICommand ToggleRepeatCommand { get; }
        public ICommand ToggleFavoriteCommand { get; }

        public PlayerBarViewModel(
            IAudioService audioService,
            IFavoriteCoordinator favoriteCoordinator,
            ICurrentUserService currentUserService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _favoriteCoordinator = favoriteCoordinator;
            _currentUserService = currentUserService;

            TogglePlayPauseCommand = new RelayCommand(_ => ExecuteTogglePlayPause(), _ => HasActiveTrack);
            NextTrackCommand = new RelayCommand(_ => PlayNext(), _ => _queue.Count > 1);
            PreviousTrackCommand = new RelayCommand(_ => PlayPrevious(), _ => _queue.Count > 1);
            ToggleShuffleCommand = new RelayCommand(_ => ExecuteToggleShuffle());
            ToggleRepeatCommand = new RelayCommand(_ => ExecuteToggleRepeat());
            ToggleFavoriteCommand = new RelayCommand(_ => _ = ToggleFavoriteAsync(), _ => HasActiveTrack && _currentUserService.CurrentUser != null);

            _favoriteCoordinator.FavoriteChanged += OnFavoriteChanged;

            _audioService.PositionChanged += (_, _) => UpdatePositionFromPlayer();
            _audioService.TrackFinished += (_, _) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_audioService.IsRepeatActive && _currentIndex >= 0)
                        PlayTrackAtIndex(_currentIndex);
                    else
                        PlayNext();
                });
            };

            Volume = _audioService.Volume;
        }

        private void OnFavoriteChanged(long deezerId, bool isFavorite)
        {
            if (_currentTrack?.Id == deezerId)
                IsCurrentTrackFavorite = isFavorite;
        }

        private async Task ToggleFavoriteAsync()
        {
            if (_currentTrack == null || _currentUserService.CurrentUser == null)
                return;

            IsCurrentTrackFavorite = await _favoriteCoordinator.ToggleAsync(_currentTrack);
        }

        public void PlayTrack(Track track, IEnumerable<Track>? queue = null)
        {
            if (string.IsNullOrWhiteSpace(track.Preview))
                return;

            _queue = queue?.Where(t => !string.IsNullOrWhiteSpace(t.Preview)).ToList() ?? new List<Track> { track };
            _currentIndex = _queue.FindIndex(t => t.Id == track.Id);
            if (_currentIndex < 0)
            {
                _queue.Insert(0, track);
                _currentIndex = 0;
            }

            PlayTrackAtIndex(_currentIndex);
        }

        private void PlayTrackAtIndex(int index)
        {
            if (index < 0 || index >= _queue.Count)
                return;

            var track = _queue[index];
            _currentIndex = index;
            _currentTrack = track;

            CurrentTrackName = track.Title;
            CurrentArtistName = track.Artist?.Name ?? "Artiste inconnu";
            LoadCoverImage(track.Album?.CoverUrl ?? track.Artist?.PictureUrl);
            IsCurrentTrackFavorite = _favoriteCoordinator.IsFavorite(track.Id);

            _audioService.LoadTrack(track.Preview);
            TotalDuration = track.Duration > 0 ? track.Duration : _audioService.TotalDuration.TotalSeconds;
            _audioService.Play();

            IsPlaying = true;
            UpdatePositionFromPlayer();
            PlaybackStarted?.Invoke();
            OnPropertyChanged(nameof(HasActiveTrack));
        }

        private void PlayNext()
        {
            if (_queue.Count == 0)
                return;

            if (_audioService.IsShuffleActive)
            {
                var random = new Random();
                PlayTrackAtIndex(random.Next(_queue.Count));
                return;
            }

            var index = _currentIndex + 1;
            if (index >= _queue.Count)
            {
                if (_audioService.IsRepeatActive)
                    index = 0;
                else
                {
                    StopPlayback();
                    return;
                }
            }

            PlayTrackAtIndex(index);
        }

        private void PlayPrevious()
        {
            if (_queue.Count == 0)
                return;

            if (TimeCurrentPosition > 3)
            {
                _audioService.CurrentPosition = TimeSpan.Zero;
                UpdatePositionFromPlayer();
                return;
            }

            var index = _currentIndex - 1;
            if (index < 0)
                index = _audioService.IsRepeatActive ? _queue.Count - 1 : 0;

            PlayTrackAtIndex(index);
        }

        private void UpdatePositionFromPlayer()
        {
            TimeCurrentPosition = _audioService.CurrentPosition.TotalSeconds;
            if (_audioService.TotalDuration.TotalSeconds > 0)
                TotalDuration = _audioService.TotalDuration.TotalSeconds;
            IsPlaying = _audioService.IsPlaying;
        }

        private void LoadCoverImage(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                CoverImage = null;
                return;
            }

            try
            {
                CoverImage = new BitmapImage(new Uri(url, UriKind.Absolute));
            }
            catch
            {
                CoverImage = null;
            }
        }

        private void ExecuteTogglePlayPause()
        {
            if (!HasActiveTrack)
                return;

            if (_audioService.IsPlaying)
                _audioService.Pause();
            else
                _audioService.Play();

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

        public void StopPlayback()
        {
            _audioService.Stop();
            _queue.Clear();
            _currentIndex = -1;
            _currentTrack = null;
            CurrentTrackName = string.Empty;
            CurrentArtistName = string.Empty;
            CoverImage = null;
            TimeCurrentPosition = 0;
            TotalDuration = 0;
            IsPlaying = false;
            IsCurrentTrackFavorite = false;
            OnPropertyChanged(nameof(HasActiveTrack));
        }
    }
}
