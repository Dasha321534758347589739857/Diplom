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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;


namespace Diplom.ViewModels.AdminViewModels
{
    public partial class GlossaryTabViewModel : ObservableObject, IRecipient<NewGlossaryMessage>
    {
        [ObservableProperty]
        private ObservableCollection<Glossary> glossaries;
        [ObservableProperty]
        private ObservableCollection<Substance> substances;


        public GlossaryTabViewModel()
        {
            using Context ctx = new Context();
            glossaries = new ObservableCollection<Glossary>(ctx.Glossaries.Include(a=>a.Substance));
            substances = new ObservableCollection<Substance>(ctx.Substances.ToList());
            WeakReferenceMessenger.Default.Register<NewGlossaryMessage>(this);
        }

        [RelayCommand]
        private void AddGlossary()
        {
           AddGlossaryWindow add = new AddGlossaryWindow();
            add.ShowDialog();
        }

        public void Receive(NewGlossaryMessage message)
        {
            NewGlossary glossary = message.Value;
            using Context ctx = new Context();
            Substance substance = new Substance();
            substance = ctx.Substances.FirstOrDefault(x => x.Id == glossary.IdSubstance);
            Glossary addGlossary = new Glossary() { NameSubstance=glossary.NameSubstance, beta=glossary.beta, minConcentration=glossary.minConcentration, maxConcentration=glossary.maxConcentration, IdSubstance=substance.Id, Substance=substance };
            ctx.Glossaries.Add(addGlossary);
            ctx.SaveChanges();
            Glossaries = new ObservableCollection<Glossary>(ctx.Glossaries.ToList());

        }

        [RelayCommand]
        private void DeleteGlossary(Glossary gloss)
        {

            using Context ctx = new Context();
            if (gloss is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить справочную информацию", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            ctx.Glossaries.Remove(gloss);
            ctx.SaveChanges();

            Glossaries = new ObservableCollection<Glossary>(ctx.Glossaries.ToList());
        }


        [RelayCommand]
        private void EditGlossary(Glossary gloss)
        {
            using Context ctx = new Context();

            ctx.Glossaries.Update(gloss);
            ctx.SaveChanges();

        }
    }
}
