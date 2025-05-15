using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data.DB;
using Diplom.Data;
using Diplom.Message;
using Diplom.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Diplom.ViewModels.ViewModelsData;
using Microsoft.EntityFrameworkCore;


namespace Diplom.ViewModels.AdminViewModels
{
    public partial class ObjectsTabViewModel: ObservableObject, IRecipient<NewObjecttMessage>, IRecipient<ChangeDbMessage>, IRecipient<NewMaterialMessage>
    {
        [ObservableProperty]
        private ObservableCollection<Objectt> objecttss;
        [ObservableProperty]
        private ObservableCollection<EmergencySituationAtTheFacility> emergencySituationAtTheFacilities;
        [ObservableProperty]
        private ObservableCollection<Fabric> materials;

        public ObjectsTabViewModel()
        {
            using Context ctx = new Context();
            objecttss = new ObservableCollection<Objectt>(ctx.Objectts.Include(a=>a.Fabric).ToList());
            emergencySituationAtTheFacilities = new ObservableCollection<EmergencySituationAtTheFacility>(ctx.EmergencySituationAtTheFacilities.Include(a=>a.SubstanceInAnObject).Include(b=>b.Air).Include(c=>c.Priming).Include(d=>d.EmergencySituation).ToList());
            materials = new ObservableCollection<Fabric>(ctx.Materials.ToList());

            WeakReferenceMessenger.Default.Register<NewObjecttMessage>(this);
            WeakReferenceMessenger.Default.Register<NewMaterialMessage>(this);
            WeakReferenceMessenger.Default.Register<ChangeDbMessage>(this);
        }

        [RelayCommand]
        private void AddObject()
        {
            AddObjectWindow add = new AddObjectWindow();
            add.ShowDialog();
        }

        [RelayCommand]
        private void AddMaterial()
        {
            AddMaterialWindow add = new AddMaterialWindow();
            add.ShowDialog();
        }

        [RelayCommand]
        private void DeleteObject(Objectt objectt)
        {

            using Context ctx = new Context();
            if (objectt is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить данный объект", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            ctx.Objectts.Remove(objectt);
            ctx.SaveChanges();

            Objecttss = new ObservableCollection<Objectt>(ctx.Objectts.ToList());


        }
        [RelayCommand]
        private void DeleteMaterial(Fabric materialus)
        {
            List<Objectt> objectts = new List<Objectt>();
            using Context ctx = new Context();
            string objDel="";
            if (materialus is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить данный материал?", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;

            objectts = ctx.Objectts.Where(p=>p.IdMaterial== materialus.Id).ToList();
            if (objectts.Count != 0)
            {
                for (int i = 0; i < objectts.Count; i++)
                {
                    objDel = objDel + " " + objectts[i].Name;
                }
                res = MessageBox.Show("Будут также удалены объекты:" + objDel, "Подтверждение", MessageBoxButton.YesNo);
                if (res != MessageBoxResult.Yes)
                    return;
                ctx.Materials.Remove(materialus);
                for (int i = 0; i < objectts.Count; i++)
                {
                    ctx.Objectts.Remove(objectts[i]);
                }
                ctx.SaveChanges();

                Materials = new ObservableCollection<Fabric>(ctx.Materials.ToList());
                Objecttss = new ObservableCollection<Objectt>(ctx.Objectts.ToList());
            }
            else {
                ctx.Materials.Remove(materialus);
                ctx.SaveChanges();

                Materials = new ObservableCollection<Fabric>(ctx.Materials.ToList());
                
            }
            
            


        }

        [RelayCommand]
        private void DeleteEmergencySituationAtTheFacilities(EmergencySituationAtTheFacility emergencySituationAtTheFacility)
        {

            using Context ctx = new Context();
            if (emergencySituationAtTheFacility is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ctx.EmergencySituationAtTheFacilities.Remove(emergencySituationAtTheFacility);
            ctx.SaveChanges();

            emergencySituationAtTheFacilities = new ObservableCollection<EmergencySituationAtTheFacility>(ctx.EmergencySituationAtTheFacilities.ToList());


        }
        public void Receive(NewObjecttMessage message)
        {
            NewObjectt substance = message.Value;
            

            using Context ctx = new Context();
            Fabric fabricc = new Fabric();
            fabricc = ctx.Materials.FirstOrDefault(x => x.Id == substance.IdMaterial);
            Objectt addSubstance = new Objectt() { Name = substance.Name, IdMaterial=fabricc.Id, Temperature = substance.Temperature, Radius = substance.Radius, Height=substance.Height, DistanceFromSurface=substance.DistanceFromSurface, Clutter=substance.Clutter, Fabric=fabricc};
            ctx.Objectts.Add(addSubstance);
            ctx.SaveChanges();
            Objecttss = new ObservableCollection<Objectt>(ctx.Objectts.ToList());

        }

        public void Receive(ChangeDbMessage message)
        {
            using Context ctx = new Context();
            Objecttss = new ObservableCollection<Objectt>(ctx.Objectts.ToList());
        }

        public void Receive(NewMaterialMessage message)
        {
            
            NewMaterial mat = message.Value;
            using Context ctx = new Context();
            Fabric addFabric=new Fabric() { Name = mat.Name,Density = mat.Density };
            ctx.Materials.Add(addFabric);
            ctx.SaveChanges();
            Materials = new ObservableCollection<Fabric>(ctx.Materials.ToList());
        }

    }
}
