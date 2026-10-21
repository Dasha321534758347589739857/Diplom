using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data.DB;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;

using System.Windows;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class AddSubstanceViewModel : ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string stateOfAggregation = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string density = "";
        //[ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(OkCommand))]
        //private string degreeOfSensitivity = "";
        //[ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(OkCommand))]
        //private string ignitionSpeed = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string specificHeatOfCombustion = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string hazardClass = "";
       

       

       

        public AddSubstanceViewModel()
        {
            
        }
        private bool CanOk()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(StateOfAggregation) && !string.IsNullOrEmpty(Density) && !string.IsNullOrEmpty(SpecificHeatOfCombustion);
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            WeakReferenceMessenger.Default.Send(new NewSubstanceMessage(new NewSubstance() { Name = name, StateOfAggregation = stateOfAggregation, Density = density, SpecificHeatOfCombustion= specificHeatOfCombustion, HazardClass = hazardClass }));
            window.Close();
        }
    }
}
