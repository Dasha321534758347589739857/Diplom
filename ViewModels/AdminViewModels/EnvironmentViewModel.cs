using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data.DB;
using Diplom.Data;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using Diplom.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class EnvironmentViewModel: ObservableObject, IRecipient<NewAirMessage>, IRecipient<NewPrimingMessage>, IRecipient<NewEmergencySituationMessage>, IRecipient<ChangeDbMessage>
    {
    
        [ObservableProperty]
        private ObservableCollection<Air> airs;
        [ObservableProperty]
        private ObservableCollection<Priming> primings;
        [ObservableProperty]
        private ObservableCollection<EmergencySituation> emergencySituations;
        [ObservableProperty]
        private ObservableCollection<EmergencySituationAtTheFacility> emergencySituationAtTheFacilities;


        public EnvironmentViewModel()
        {
            using Context ctx = new Context(); 
            airs = new ObservableCollection<Air>(ctx.Airs.ToList());
            primings = new ObservableCollection<Priming>(ctx.Primings.ToList());
            emergencySituations = new ObservableCollection<EmergencySituation>(ctx.EmergencySituations.ToList());
            emergencySituationAtTheFacilities = new ObservableCollection<EmergencySituationAtTheFacility>(ctx.EmergencySituationAtTheFacilities.Include(a => a.SubstanceInAnObject).Include(b => b.Air).Include(c => c.Priming).Include(d => d.EmergencySituation).ToList());



            WeakReferenceMessenger.Default.Register<NewAirMessage>(this);
            WeakReferenceMessenger.Default.Register<NewPrimingMessage>(this);
            WeakReferenceMessenger.Default.Register <NewEmergencySituationMessage>(this);
            //WeakReferenceMessenger.Default.Register<ChangeDbMessage>(this);
        }

        [RelayCommand]
        private void DeletePriming(Priming priming)
        {



            using Context ctx = new Context();
            if (priming is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить данный грунт", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            ctx.Primings.Remove(priming);
            ctx.SaveChanges();

            Primings = new ObservableCollection<Priming>(ctx.Primings.ToList());


        }



        [RelayCommand]
        private void DeleteEF(EmergencySituationAtTheFacility emergencySituationAtTheFacility)
        {



            using Context ctx = new Context();
            if (emergencySituationAtTheFacility is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить данную ситуацию", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            ctx.EmergencySituationAtTheFacilities.Remove(emergencySituationAtTheFacility);
            ctx.SaveChanges();

            EmergencySituationAtTheFacilities = new ObservableCollection<EmergencySituationAtTheFacility>(ctx.EmergencySituationAtTheFacilities.ToList());


        }

        [RelayCommand]
        private void DeleteAir(Air air)
        {



            using Context ctx = new Context();
            if (air is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить данный элемент", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            ctx.Airs.Remove(air);
            ctx.SaveChanges();

            Airs = new ObservableCollection<Air>(ctx.Airs.ToList());


        }
        [RelayCommand]
        private void DeleteEmer(EmergencySituation emergency)
        {



            using Context ctx = new Context();
            if (emergency is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить данную ситуацию", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            ctx.EmergencySituations.Remove(emergency);
            ctx.SaveChanges();

            EmergencySituations = new ObservableCollection<EmergencySituation>(ctx.EmergencySituations.ToList());


        }

        public void Receive(NewAirMessage message)
        {
            NewAir substance = message.Value;


            using Context ctx = new Context();

            Air addSubstance = new Air() { Name = substance.Name, Humidity=substance.Humidity };
            ctx.Airs.Add(addSubstance);
            ctx.SaveChanges();
            Airs = new ObservableCollection<Air>(ctx.Airs.ToList());


        }
        public void Receive(NewEmergencySituationMessage message)
        {
            NewEmergencySituation substance = message.Value;


            using Context ctx = new Context();

            EmergencySituation addSubstance = new EmergencySituation() { Name = substance.Name };
            ctx.EmergencySituations.Add(addSubstance);
            ctx.SaveChanges();
            EmergencySituations = new ObservableCollection<EmergencySituation>(ctx.EmergencySituations.ToList());


        }
        public void Receive(NewPrimingMessage message)
        {
            NewPriming substance = message.Value;


            using Context ctx = new Context();

            Priming addSubstance = new Priming() { Name = substance.Name, Density = substance.Density, SoilType=substance.SoilType };
            ctx.Primings.Add(addSubstance);
            ctx.SaveChanges();

            Primings = new ObservableCollection<Priming>(ctx.Primings.ToList());
        }

        public void Receive(ChangeDbMessage message)
        {
            using Context ctx = new Context();
            Airs = new ObservableCollection<Air>(ctx.Airs.ToList());
            Primings = new ObservableCollection<Priming>(ctx.Primings.ToList());
            EmergencySituations = new ObservableCollection<EmergencySituation>(ctx.EmergencySituations.ToList());
            emergencySituationAtTheFacilities = new ObservableCollection<EmergencySituationAtTheFacility>(ctx.EmergencySituationAtTheFacilities.ToList());
        }

        [RelayCommand]
        private void AddPriming()
        {
            AddPrimingWindow add = new AddPrimingWindow();
            add.ShowDialog();
        }

        [RelayCommand]
        private void AddAir()
        {
            AddAirWindow add = new AddAirWindow();
            add.ShowDialog();
        }

        [RelayCommand]
        private void AddEmergencySituation()
        {
            AddEmergencySituationWindow add = new AddEmergencySituationWindow();
            add.ShowDialog();
        }

        
    }
}
