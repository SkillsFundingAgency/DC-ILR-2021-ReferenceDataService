using System;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using PubSub;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping
{
    public class CommandLineMessengerService : IMessengerService
    {
        private readonly Hub _hub = new Hub();

        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            _hub.Subscribe(recipient, action);
        }

        public void Send<TMessage>(TMessage message)
        {
            _hub.Publish(message);
        }
    }
}
