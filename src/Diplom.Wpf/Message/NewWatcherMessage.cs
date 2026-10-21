using CommunityToolkit.Mvvm.Messaging.Messages;
using Diplom.ViewModels.ViewModelsData;

namespace Diplom.Message
{
    public class NewWatcherMessage : ValueChangedMessage<NewWatcher>
    {
        public NewWatcherMessage(NewWatcher message) : base(message) { }
    }
}
