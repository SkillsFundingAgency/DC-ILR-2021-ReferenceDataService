using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ZipFileService : IZipFileService
    {
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IFileService _fileService;
        private readonly ILogger _logger;

        public ZipFileService(IJsonSerializationService jsonSerializationService, IFileService fileService, ILogger logger)
        {
            _jsonSerializationService = jsonSerializationService;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task SaveCollectionZipAsync(string zipFileName, string container, IReadOnlyDictionary<string, object> zipContents, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Zip File Creation");
            using (var stream = await _fileService.OpenWriteStreamAsync(zipFileName, container, cancellationToken))
            {
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in zipContents)
                    {
                        AddFileToZip(zipArchive, file.Key, file.Value);
                    }
                }

                _logger.LogInfo("Finished Zip File Creation");
            }
        }

        private void AddFileToZip(ZipArchive zipArchive, string fileName, object fileContent)
        {
            _logger.LogInfo("Writing " + fileName + " to zip file.");

            var file = zipArchive.CreateEntry(fileName);

            using (var entryStream = file.Open())
            {
                _jsonSerializationService.Serialize(fileContent, entryStream);
            }
        }
    }
}
