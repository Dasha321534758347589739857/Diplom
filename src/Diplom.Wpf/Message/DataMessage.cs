using Diplom.ViewModels.ViewModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Diplom.Message
{
    public class DataMessage: ValueChangedMessage<TableData[]>
    {
        public DataMessage(TableData[] message) : base(message) { }

    }
}
