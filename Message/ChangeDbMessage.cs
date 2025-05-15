using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diplom.ViewModels;

namespace Diplom.Message
{
    public class ChangeDbMessage : ValueChangedMessage<NewMat>
    {
        public ChangeDbMessage(NewMat message) : base(message)
        {
        }

    }
    public class NewMat
    {
        public string? Name { get; set; }
    }
}
