using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class AddAirViewModel: ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string humidity = "";

        public AddAirViewModel() { }
        private bool CanOk()
        {

            return !string.IsNullOrEmpty(Name);
            
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {



            if (Int32.TryParse(Humidity, out int humidityAir))
            {
                if(humidityAir <= 100 && humidityAir >= 0)
                {
                    WeakReferenceMessenger.Default.Send(new NewAirMessage(new NewAir() { Name = Name, Humidity = Humidity }));
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Диапазон влажности от 0% до 100%. Попробуйте снова.");
                    Humidity = null;
                }
            }
            else 
            {
                MessageBox.Show("Введено неверное значение влажности.");
                Humidity = null;
            }

                
                

            
        }


    }
}
