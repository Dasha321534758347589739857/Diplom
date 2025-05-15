using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom.ViewModels.ViewModelsData;

namespace Diplom.Message
{
    public class NewUserMessage : ValueChangedMessage<NewUser>
    {
        public NewUserMessage(NewUser message) : base(message) { }
    }
}
