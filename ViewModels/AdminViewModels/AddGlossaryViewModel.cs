using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data;
using Diplom.Data.DB;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using Microsoft.Office.Interop.Excel;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class AddGlossaryViewModel: ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string beta = "";
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string minConcentration = "";
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string maxConcentration = "";
        [ObservableProperty]
        private ObservableCollection<Substance> substances;

        [ObservableProperty]
        private Substance? selectedSubstance;

        public AddGlossaryViewModel()
        {
            Context ctx = new Context();
            Substances = new ObservableCollection<Substance>(ctx.Substances.ToList());
        }


        private bool CanOk()
        {
            return !string.IsNullOrEmpty(Beta) && !string.IsNullOrEmpty(MaxConcentration) && !string.IsNullOrEmpty(MinConcentration);
        }


        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok()
        {

            if ( Double.TryParse(Beta, out double betta) && Int32.TryParse(MinConcentration, out int minC) && Int32.TryParse(MaxConcentration, out int maxC))
            {
                using Context ctx = new Context();
                Substance substance = new Substance();
                substance = ctx.Substances.FirstOrDefault(x => x.Id == SelectedSubstance.Id);
                WeakReferenceMessenger.Default.Send(new NewGlossaryMessage(new NewGlossary() { NameSubstance = substance.Name, IdSubstance = substance.Id, minConcentration = MinConcentration, maxConcentration = MaxConcentration, Substance=substance }));
                //window.Close();
            }
            else
            {
                MessageBox.Show("Введены неверные значения.");

                MinConcentration = "";
                MaxConcentration = "";
                

            }

        }




    }

   
}
