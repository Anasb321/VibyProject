using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibyApp.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainVM;

        public RegisterViewModel(MainViewModel mainVM) 
        {
            _mainVM = mainVM;
        }

        [RelayCommand]
        public void GoToLogin()
        {
            _mainVM.CurrentView = new LoginViewModel(_mainVM);
        }
    }
}
