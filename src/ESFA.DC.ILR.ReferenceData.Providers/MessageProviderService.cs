using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Message;


namespace ESFA.DC.ILR.ValidationService.Providers
{
    public class MessageProviderService : IValidationItemProviderService<IEnumerable<IMessage>>
    {
        private readonly MessageItem _messageItem;

        public MessageProviderService(MessageItem messageItem)
        {
            _messageItem = messageItem;
        }

        public async Task<IMessage>> ProvideAsync(CancellationToken cancellationToken)
        {
            return _messageItem.Message
        }
    }
}
