using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop
{
    public class ReferenceDataServiceDesktopTask : IDesktopTask
    {
        private readonly bool compressOutput = false;
        private readonly IMessageProvider _messageProvider;
        private readonly IReferenceDataPopulationService _referenceDataPopulationService;
        private readonly IFilePersister _filePersister;
        private readonly IDesktopContextReturnPeriodUpdateService _desktopContextReturnPeriodUpdateService;
        private readonly ILogger _logger;

        public ReferenceDataServiceDesktopTask(
            IMessageProvider messageProvider,
            IReferenceDataPopulationService referenceDataPopulationService,
            IFilePersister filePersister,
            IDesktopContextReturnPeriodUpdateService desktopContextReturnPeriodUpdateService,
            ILogger logger)
        {
            _messageProvider = messageProvider;
            _referenceDataPopulationService = referenceDataPopulationService;
            _filePersister = filePersister;
            _desktopContextReturnPeriodUpdateService = desktopContextReturnPeriodUpdateService;
            _logger = logger;
        }

        public async Task<IDesktopContext> ExecuteAsync(IDesktopContext desktopContext, CancellationToken cancellationToken)
        {
            // Create context
            IReferenceDataContext referenceDataContext = new ReferenceDataJobContextMessageContext(desktopContext);

            // Retrieving ILR File
            _logger.LogInfo("Starting ILR File Retrieval");
            var message = await _messageProvider.ProvideAsync(referenceDataContext, cancellationToken);
            _logger.LogInfo("Finished retirieving ILR File");

            // get reference data and build model.
            _logger.LogInfo("Starting Reference Data Population");
            var referenceData = await _referenceDataPopulationService.PopulateAsync(referenceDataContext, message, cancellationToken);
            _logger.LogInfo("Finished Reference Data Population");

            // output model.
            _logger.LogInfo("Starting Reference Data Output");
            await _filePersister.StoreAsync(referenceDataContext.OutputReferenceDataFileKey, referenceDataContext.Container, referenceData, compressOutput, cancellationToken);
            _logger.LogInfo("Finished Reference Data Output");

            // set return period
            _logger.LogInfo("Adding Return Period and Ukprn to Context");

            _desktopContextReturnPeriodUpdateService.UpdateCollectionPeriod(
                referenceDataContext,
                message.HeaderEntity.CollectionDetailsEntity.FilePreparationDate,
                referenceData.MetaDatas.CollectionDates.ReturnPeriods);

            var ukprn = message?.HeaderEntity?.SourceEntity?.UKPRN;

            if (ukprn != null)
            {
                referenceDataContext.Ukprn = ukprn.Value;
            }

            _logger.LogInfo($"Finished adding Return Period : {referenceDataContext.ReturnPeriod} and Ukprn : {referenceDataContext.Ukprn} to Context");

            return desktopContext;
        }
    }
}
