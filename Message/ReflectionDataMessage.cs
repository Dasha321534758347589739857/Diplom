using CommunityToolkit.Mvvm.Messaging.Messages;
using Diplom.ViewModels.ViewModelsData;

namespace Diplom.Message
{
    public class ReflectionDataMessage : ValueChangedMessage<TableData[]>
    {
        public ReflectionDataMessage(TableData[] message) : base(message) { }
    }
   
}