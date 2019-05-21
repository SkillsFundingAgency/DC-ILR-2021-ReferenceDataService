using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers
{
    public class GZipFileProvider : IGzipFileProvider
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileService _fileService;

        public GZipFileProvider(IJsonSerializationService jsonSerializationService, IFileService fileService)
        {
            _jsonSerializationService = jsonSerializationService;
            _fileService = fileService;
        }

        public async Task CompressAndStoreAsync<T>(IReferenceDataContext referenceDataContext, T referenceDataRoot, CancellationToken cancellationToken)
        {
            using (var fileStream = await _fileService.OpenWriteStreamAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, cancellationToken))
            {
                using (var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal))
                {
                    _jsonSerializationService.Serialize(referenceDataRoot, gzipStream);
                }
            }
        }

        public async Task RetrieveAndDecompressAsync<T>(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            using (var fileStream = await _fileService.OpenReadStreamAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, cancellationToken))
            {
                using (var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal))
                {
                    _jsonSerializationService.Deserialize<T>(gzipStream);
                }
            }
        }
    }
}
