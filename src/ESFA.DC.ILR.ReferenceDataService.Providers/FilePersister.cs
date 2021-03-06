﻿using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers
{
    public class FilePersister : IFilePersister
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileService _fileService;

        public FilePersister(IJsonSerializationService jsonSerializationService, IFileService fileService)
        {
            _jsonSerializationService = jsonSerializationService;
            _fileService = fileService;
        }

        public async Task StoreAsync<T>(string outputKey, string container, T referenceDataRoot, bool compress, CancellationToken cancellationToken)
        {
            using (var fileStream = await _fileService.OpenWriteStreamAsync(outputKey, container, cancellationToken))
            {
                if (compress)
                {
                    using (var gzipStream = new GZipStream(fileStream, CompressionLevel.Optimal))
                    {
                        _jsonSerializationService.Serialize(referenceDataRoot, gzipStream);
                    }
                }
                else
                {
                    _jsonSerializationService.Serialize(referenceDataRoot, fileStream);
                }
            }
        }
    }
}
