using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tasks
{
    public class IlrMessageTask : ITask
    {
        private const string TempFileKey = "{0}/{1}/webservice-output.json";
        private readonly bool compressOutput = false;
        private readonly IMessageProvider _messageProvider;
        private readonly IReferenceDataPopulationService _referenceDataPopulationService;
        private readonly IEdrsApiService _edrsApiService;
        private readonly IFilePersister _filePersister;
        private readonly FeatureConfiguration _featureConfiguration;
        private readonly ILogger _logger;

        public IlrMessageTask(
            IMessageProvider messageProvider,
            IReferenceDataPopulationService referenceDataPopulationService,
            IEdrsApiService edrsApiService,
            IFilePersister filePersister,
            FeatureConfiguration featureConfiguration,
            ILogger logger)
        {
            _messageProvider = messageProvider;
            _referenceDataPopulationService = referenceDataPopulationService;
            _edrsApiService = edrsApiService;
            _filePersister = filePersister;
            _featureConfiguration = featureConfiguration;
            _logger = logger;
        }

        public async Task ExecuteAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieving ILR File
                _logger.LogInfo("Starting ILR File Retrieval");
                var message = await _messageProvider.ProvideAsync(referenceDataContext, cancellationToken);
                _logger.LogInfo("Finished retirieving ILR File");

                // get reference data and build model.
                _logger.LogInfo("Starting Reference Data Population");
                var referenceData = await _referenceDataPopulationService.PopulateAsync(referenceDataContext, message, cancellationToken);
                _logger.LogInfo("Finished Reference Data Population");

                if (Convert.ToBoolean(_featureConfiguration.EDRSAPIEnabled))
                {
                    _logger.LogInfo("Starting EDRS API validation");
                    var apiData = await _edrsApiService.ValidateErnsAsync(message, cancellationToken);
                    _logger.LogInfo("Finished EDRS API validation");

                    _logger.LogInfo("Starting EDRS API Output");
                    var key = string.Format(TempFileKey, referenceDataContext.Ukprn, referenceDataContext.JobId);
                    await _filePersister.StoreAsync(key, referenceDataContext.Container, apiData, compressOutput, cancellationToken);
                    _logger.LogInfo("Finished EDRS API Output");
                }

                // output model.
                _logger.LogInfo("Starting Reference Data Output");
                await _filePersister.StoreAsync(referenceDataContext.OutputIlrReferenceDataFileKey, referenceDataContext.Container, referenceData, compressOutput, cancellationToken);
                _logger.LogInfo("Finished Reference Data Output");
            }
            catch (Exception exception)
            {
                _logger.LogError("Reference Data Service Output Exception", exception);
                throw;
            }
        }
    }
}
