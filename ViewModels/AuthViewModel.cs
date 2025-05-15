using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data;
using Diplom.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Diplom.ViewModels
{
    public partial class AuthViewModel : ObservableObject
    {
        private PasswordBox pwb;
        [ObservableProperty]
        private string error = "";
        [ObservableProperty]
        private string login = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EnterCommand))]
        private int tryEnterCount = 0;

        public AuthViewModel(PasswordBox pwb)
        {
            this.pwb = pwb;
        }
        [RelayCommand(CanExecute = nameof(CanEnter))]
        private void Enter(Window window)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(pwb.Password))
            {
                Error = "Введите данные";
                return;
            }
            Context ctx = new Context();

            var user = ctx.Users.Include(x => x.Role).FirstOrDefault(x => x.Login == Login && x.Password == pwb.Password);
            if (user is null)
            {
                TryEnterCount++;
                Error = "Неверный логин и/или пароль";
                return;
            }
            if (user.Role!.Name == "admin")
            {
                window.Hide();
                var main = new AdminWindow() { Owner = window };
                
                WeakReferenceMessenger.Default.Send(new Message.UserMessage(user));
               
                main.ShowDialog();
                window.Close();
            }
            else if (user.Role.Name == "user")
            {
                window.Hide();
                var main = new MainWindow(user) { Owner = window };
                WeakReferenceMessenger.Default.Send(new Message.UserMessage(user));
                
                main.ShowDialog();
                window.Close();
            }
        }
        private bool CanEnter() =>
          TryEnterCount <= 10;
    }

}
