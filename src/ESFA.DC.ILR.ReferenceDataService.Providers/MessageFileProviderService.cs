using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers
{
    public class MessageFileProviderService : IMessageFileProviderService
    {
        private readonly IXmlSerializationService _xmlSerializationService;
        private readonly IMessageStreamProviderService _streamProvider;

        public MessageFileProviderService(
            IXmlSerializationService xmlSerializationService,
             IMessageStreamProviderService streamProvider)
        {
            _xmlSerializationService = xmlSerializationService;
            _streamProvider = streamProvider;
        }

        public async Task<IMessage> ProvideAsync(string fileLocation, CancellationToken cancellationToken)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (var stream = await _streamProvider.Provide(fileLocation, cancellationToken))
            {
                stream.Seek(0, SeekOrigin.Begin);

                return _xmlSerializationService.Deserialize<Message>(stream);
            }
        }
    }
}
