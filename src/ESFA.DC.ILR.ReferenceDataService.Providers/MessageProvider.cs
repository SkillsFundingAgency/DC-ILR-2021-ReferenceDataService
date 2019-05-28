using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers
{
    public class MessageProvider : IMessageProvider
    {
        private readonly IFileService _fileService;
        private readonly IXmlSerializationService _xmlSerializationService;

        public MessageProvider(IFileService fileService, IXmlSerializationService xmlSerializationService)
        {
            _fileService = fileService;
            _xmlSerializationService = xmlSerializationService;
        }

        public async Task<IMessage> ProvideAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            using (var stream = await _fileService.OpenReadStreamAsync(referenceDataContext.FileReference, referenceDataContext.Container, cancellationToken))
            {
                stream.Position = 0;

                return _xmlSerializationService.Deserialize<IMessage>(stream);
            }
        }
    }
}
