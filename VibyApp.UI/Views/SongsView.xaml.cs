using System;
using System.Windows.Controls;
using VibyApp.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibyApp.UI.Views
{
    public partial class SongsView : UserControl
    {
        public SongsView()
        {
            InitializeComponent();
            
            this.DataContext = new SongsViewModel();
        }
    }
}