using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ZipFileService : IZipFileService
    {
        private readonly IFileService _fileService;
        private readonly ILogger _logger;

        public ZipFileService(IFileService fileService, ILogger logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        public async Task SaveCollectionZipAsync(string zipFileName, string container, IReadOnlyDictionary<string, string> zipContents, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Zip File Creation");
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update, true))
                {
                    foreach (var file in zipContents)
                    {
                        AddFileToZip(zipArchive, file.Key, file.Value);
                    }
                }

                using (var fileStream = await _fileService.OpenWriteStreamAsync(zipFileName, container, cancellationToken))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(fileStream);
                }

                _logger.LogInfo("Finished Zip File Creation");
            }
        }

        private void AddFileToZip(ZipArchive zipArchive, string fileName, string fileContent)
        {
            _logger.LogInfo("Writing " + fileName + " to zip file.");

            var file = zipArchive.CreateEntry(fileName);

            using (var entryStream = file.Open())
            {
                using (var streamWriter = new StreamWriter(entryStream))
                {
                    streamWriter.Write(fileContent);
                }
            }
        }
    }
}
