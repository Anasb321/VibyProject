using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VibyApp.DB.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibyApp.ViewModels
{
    public partial class SongsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _pageTitle = "Ma Musique";

        // Liste à remplir pour chanson
        //public ObservableCollection<Song> Songs { get; set; } = new ObservableCollection<Song>();

        public SongsViewModel()
        {
            //À remplir
        }
    }
}