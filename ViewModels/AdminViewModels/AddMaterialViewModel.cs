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
    public partial class AddMaterialViewModel : ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string density = "";

        public AddMaterialViewModel() { }
        private bool CanOk()
        {
            return !string.IsNullOrEmpty(Name);
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            if (Double.TryParse(Density, out double density))
            {
                if (density <= 10000 && density >= 0)
                {
                    WeakReferenceMessenger.Default.Send(new NewMaterialMessage(new NewMaterial() { Name = name, Density = Density }));
                    window.Close();
                }
                else
                {

                }

            }
            else 
            {
            
            }
        }
    }
}
