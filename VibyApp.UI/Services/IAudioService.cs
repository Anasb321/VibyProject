using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibyApp.UI.Services
{
    public interface IAudioService
    {
        bool IsPlaying { get; }
        bool IsShuffleActive { get; set; }
        bool IsRepeatActive { get; set; }

        void LoadTrack(string musicURL);
        void Play();
        void Pause();
        void Next();
        void Previous();

        
        TimeSpan CurrentPosition { get; set; }
        TimeSpan TotalDuration { get; }

        event EventHandler TrackFinished;
    }
}
