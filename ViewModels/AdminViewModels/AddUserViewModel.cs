﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data.DB;
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
    public partial class AddUserViewModel : ObservableObject
    {
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string name = "";
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        [ObservableProperty]
        private string login = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private string password = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private Role role;

        public List<Role> Roles { get; set; }

        public AddUserViewModel()
        {
            Role = new Role();

            Roles = new List<Role>() { new Role() { Name = "Пользователь" }, new Role() { Name = "Администратор" } };
        }
        private bool CanOk()
        {
            return !string.IsNullOrEmpty(Role.Name) && !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }
        [RelayCommand(CanExecute = nameof(CanOk))]
        private void Ok(Window window)
        {
            WeakReferenceMessenger.Default.Send(new NewUserMessage(new NewUser() { Name = Name, Login=Login, Password = Password, Role = Role.Name }));
            window.Close();
        }
    }
}
