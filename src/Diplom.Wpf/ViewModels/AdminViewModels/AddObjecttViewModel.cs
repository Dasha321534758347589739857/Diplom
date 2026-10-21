using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data;
using Diplom.Data.DB;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class AddObjecttViewModel: ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string temperature = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string radius = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string wallThickness = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string height = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string distanceFromSurface = "";
       
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string clutter = "";
        [ObservableProperty]
        private ObservableCollection<Fabric> materials;

        [ObservableProperty]
        private Fabric? selectedMaterial;



        public AddObjecttViewModel()
        {
            Context ctx = new Context();
            Materials = new ObservableCollection<Fabric>(ctx.Materials.ToList());


        }
        private bool CanOk()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Temperature) && !string.IsNullOrEmpty(Radius) && !string.IsNullOrEmpty(Height) && !string.IsNullOrEmpty(DistanceFromSurface);
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {

            if (Int32.TryParse(Temperature, out int temperature)&& Double.TryParse(Radius, out double radius)&& Double.TryParse(Height, out double height)&& Double.TryParse(DistanceFromSurface, out double distanceFromSurface)&& Int32.TryParse(Clutter, out int clutter))
            {
                using Context ctx = new Context();
                Fabric fabricc = new Fabric();
                fabricc = ctx.Materials.FirstOrDefault(x => x.Id == SelectedMaterial.Id);
                WeakReferenceMessenger.Default.Send(new NewObjecttMessage(new NewObjectt() { Name = Name, IdMaterial=fabricc.Id, Temperature = Temperature, Radius = Radius, Height = Height, DistanceFromSurface = DistanceFromSurface, WallThickness= WallThickness, Clutter = Clutter, fabric=fabricc }));
                //window.Close();
            }
            else
            {
                MessageBox.Show("Введены неверные значения.");
                Name = "";
                Temperature = "";
                Radius = "";
                Height = "";
                DistanceFromSurface = "";
                Clutter = "";
                WallThickness = "";

            }

        }
    }
}
