using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class IlrMessageTask : ITask
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IReferenceDataPopulationService _referenceDataPopulationService;
        private readonly IReferenceDataOutputService _referenceDataOutputService;
        private readonly ILogger _logger;

        public IlrMessageTask(
            IMessageProvider messageProvider,
            IReferenceDataPopulationService referenceDataPopulationService,
            IReferenceDataOutputService referenceDataOutputService,
            ILogger logger)
        {
            _messageProvider = messageProvider;
            _referenceDataPopulationService = referenceDataPopulationService;
            _referenceDataOutputService = referenceDataOutputService;
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
                var referenceData = await _referenceDataPopulationService.PopulateAsync(message, cancellationToken);
                _logger.LogInfo("Finished Reference Data Population");

                // output model.
                _logger.LogInfo("Starting Reference Data Output");
                await _referenceDataOutputService.OutputAsync(referenceDataContext, referenceData, cancellationToken);
                _logger.LogInfo("Finished Reference Data Output");
            }
            catch (Exception exception)
            {
                _logger.LogError("Reference Data Service Output Exception", exception);
            }
        }
    }
}
