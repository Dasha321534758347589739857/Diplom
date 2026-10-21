using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data.DB;
using Diplom.Data;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using Diplom.Windows;
using System.Collections.ObjectModel;

namespace Diplom.ViewModels.AdminViewModels
{
    public partial class WatcherViewModel:ObservableObject, IRecipient<NewWatcherMessage>
    {
        [ObservableProperty]
        private ObservableCollection<Watcher> watchers;

        public WatcherViewModel()
        {
            using Context ctx = new Context();
            watchers = new ObservableCollection<Watcher>(ctx.Watchers.ToList());

            WeakReferenceMessenger.Default.Register<NewWatcherMessage>(this);
        }

        [RelayCommand]
        private void AddWatcher()
        {
            AddWatcherWindow add = new AddWatcherWindow();
            add.ShowDialog();
        }

        [RelayCommand]
        private void DeleteWatcher(Watcher watcher)
        {



            using Context ctx = new Context();
            if (watcher is null)
            {
                MessageBox.Show("Внутернняя ошибка, повтроите попытку позже", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show("Вы уверены, что хотите удалить данный элемент", "Подтверждение", MessageBoxButton.YesNo);
            if (res != MessageBoxResult.Yes)
                return;
            ctx.Watchers.Remove(watcher);
            ctx.SaveChanges();

            Watchers = new ObservableCollection<Watcher>(ctx.Watchers.ToList());


        }

        public void Receive(NewWatcherMessage message)
        {
            NewWatcher watch = message.Value;


            using Context ctx = new Context();

            Watcher addWatcher = new Watcher() { Name = watch.Name, Mass = watch.Mass };
            ctx.Watchers.Add(addWatcher);
            ctx.SaveChanges();
            Watchers = new ObservableCollection<Watcher>(ctx.Watchers.ToList());


        }
    }
}
