using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ReferenceDataOutputService : IReferenceDataOutputService
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileService _fileService;

        public ReferenceDataOutputService(IJsonSerializationService jsonSerializationService, IFileService fileService)
        {
            _jsonSerializationService = jsonSerializationService;
            _fileService = fileService;
        }

        public async Task OutputAsync(IReferenceDataContext referenceDataContext, ReferenceDataRoot referenceDataRoot, CancellationToken cancellationToken)
        {
            using (var fileStream = await _fileService.OpenWriteStreamAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, cancellationToken))
            {
                _jsonSerializationService.Serialize(referenceDataRoot, fileStream);
            }
        }
    }
}
