using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Diplom.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data.DB;
using Diplom.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;


namespace Diplom.ViewModels.AdminViewModels
{
    public partial class AdminViewModel : ObservableObject, IRecipient<UserMessage>
    {
        private User? userInput;
        [ObservableProperty]
        private string? nameUser;
        public AdminViewModel() 
        {
            WeakReferenceMessenger.Default.Register<UserMessage>(this);
        }
        public void Receive(UserMessage message)
        {
            userInput = message.Value;
            NameUser = userInput.Name;
        }
    }
   
    
}
