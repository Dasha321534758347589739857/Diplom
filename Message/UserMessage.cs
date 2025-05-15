using CommunityToolkit.Mvvm.Messaging.Messages;
using Diplom.Data.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Message
{
    public class UserMessage : ValueChangedMessage<User>
    {
        public UserMessage(User message) : base(message) { }
    }
}
