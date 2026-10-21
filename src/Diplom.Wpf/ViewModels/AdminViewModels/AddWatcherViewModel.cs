using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class AddWatcherViewModel : ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string mass = "";

        public AddWatcherViewModel() { }

        private bool CanOk()
        {
            return !string.IsNullOrEmpty(Name);
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            WeakReferenceMessenger.Default.Send(new NewWatcherMessage(new NewWatcher() { Name = name, Mass = mass }));
            window.Close();
        }

    }
}
