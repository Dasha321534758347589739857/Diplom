﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using Diplom.ViewModels.ViewModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Message
{
    public class NewGrafDateMessage : ValueChangedMessage<NewKey>
    {
        public NewGrafDateMessage(NewKey message) : base(message) { }
    }

}
