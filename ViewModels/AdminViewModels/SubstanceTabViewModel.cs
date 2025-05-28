using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data;
using Diplom.Data.DB;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using Diplom.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;



namespace Diplom.ViewModels.AdminViewModels
{
    public partial class SubstanceTabViewModel : ObservableObject, IRecipient<NewSubstanceMessage>, IRecipient<ChangeDbMessage>
    {
        [ObservableProperty]
        private ObservableCollection<Substance> substancess;
        [ObservableProperty]
        private ObservableCollection<SubstanceInAnObject> substanceInAnObjectss;
        [ObservableProperty]
        private Substance sub;
        [ObservableProperty]
        private Objectt obj;
        public SubstanceTabViewModel()
        {
            using Context ctx = new Context();
            Substancess = new ObservableCollection<Substance>(ctx.Substances.ToList());
            SubstanceInAnObjectss = new ObservableCollection<SubstanceInAnObject>(ctx.SubstanceInAnObjects.ToList());

            WeakReferenceMessenger.Default.Register<NewSubstanceMessage>(this);
            WeakReferenceMessenger.Default.Register<ChangeDbMessage>(this);

        }

        [RelayCommand]
        private void AddSubstance()
        {
            AddSubtanceWindow add = new AddSubtanceWindow();
            add.ShowDialog();
        }


        [RelayCommand]
        private void DeleteSubstanceInAnObject(SubstanceInAnObject substanceInAnObject)
        {



            using Context ctx = new Context();
            if (substanceInAnObject is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ctx.SubstanceInAnObjects.Remove(substanceInAnObject);
            ctx.SaveChanges();

            SubstanceInAnObjectss = new ObservableCollection<SubstanceInAnObject>(ctx.SubstanceInAnObjects.ToList());


        }



        [RelayCommand]
        private void DeleteSubstance(Substance substance)
        {



            using Context ctx = new Context();
            if (substance is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var res = MessageBox.Show("Вы уверены, что хотите удалить данное вещество", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;

            ctx.Substances.Remove(substance);
            ctx.SaveChanges();

            Substancess = new ObservableCollection<Substance>(ctx.Substances.ToList());


        }
        public void Receive(NewSubstanceMessage message)
        {
            NewSubstance substance = message.Value;
           
            
            using Context ctx = new Context();
           
            Substance addSubstance = new Substance() { Name = substance.Name,  StateOfAggregation = substance.StateOfAggregation, Density = substance.Density, SpecificHeatOfCombustion=substance.SpecificHeatOfCombustion, HazardClass = substance.HazardClass };
       
            ctx.Substances.Add(addSubstance);
            ctx.SaveChanges();
            Substancess = new ObservableCollection<Substance>(ctx.Substances.ToList());

        }

        public void Receive(ChangeDbMessage message)
        {
            using Context ctx = new Context();
            Substancess = new ObservableCollection<Substance>(ctx.Substances.ToList());
        }
        [RelayCommand]
        private void EditSubstance(Substance substance)
        {
            using Context ctx = new Context();

            ctx.Substances.Update(substance);
            ctx.SaveChanges();
        }

    }
}
