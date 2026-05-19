using VibyApp.UI.ViewModels;

namespace VibyApp.UI.Models
{
    public class TrackDisplayItem : BaseViewModel
    {
        public Track Track { get; }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                OnPropertyChanged();
            }
        }


        public TrackDisplayItem(Track track, bool isFavorite = false)
        {
            Track = track;
            _isFavorite = isFavorite;
        }
    }
}
