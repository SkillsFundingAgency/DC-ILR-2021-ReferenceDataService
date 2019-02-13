using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers
{
    public class AzureStorageFileContentStringProviderService : IMessageStreamProviderService
    {
        private readonly ILogger _logger;
        private readonly IStreamableKeyValuePersistenceService _keyValuePersistenceService;

        public AzureStorageFileContentStringProviderService(
            IStreamableKeyValuePersistenceService keyValuePersistenceService,
            ILogger logger)
        {
            _logger = logger;
            _keyValuePersistenceService = keyValuePersistenceService;
        }

        public async Task<Stream> Provide(string fileLocation, CancellationToken cancellationToken)
        {
            var startDateTime = DateTime.UtcNow;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            MemoryStream memoryStream = new MemoryStream();
            await _keyValuePersistenceService.GetAsync(fileLocation, memoryStream, cancellationToken);

            var processTimes = new StringBuilder();

            processTimes.AppendLine($"Start Time : {startDateTime}");
            processTimes.AppendLine($"Total Time : {(DateTime.UtcNow - startDateTime).TotalMilliseconds}");

            _logger.LogDebug($"Blob download :{processTimes} ");

            return memoryStream;
        }
    }
}
