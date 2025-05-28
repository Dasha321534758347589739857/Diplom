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

    }
}
