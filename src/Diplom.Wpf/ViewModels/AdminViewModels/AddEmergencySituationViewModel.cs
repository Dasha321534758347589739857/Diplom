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
using System.Xml.Linq;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class AddEmergencySituationViewModel: ObservableObject
    {
       
            
            [ObservableProperty]
            private string name = "";
            

            public AddEmergencySituationViewModel() { }
            private bool CanOk()
            {
                return !string.IsNullOrEmpty(Name);
            }
            [RelayCommand]
            private void Ok(Window window)
            {
                WeakReferenceMessenger.Default.Send(new NewEmergencySituationMessage(new NewEmergencySituation() { Name = name}));
                window.Close();
            }


        
    }
}
