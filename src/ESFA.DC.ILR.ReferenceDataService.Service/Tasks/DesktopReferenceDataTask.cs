using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tasks
{
    public class DesktopReferenceDataTask : ITask
    {
        private readonly IDesktopReferenceDataPopulationService _referenceDataPopulationService;
        private readonly IDesktopReferenceDataFileService _desktopReferenceDataFileService;
        private readonly IDesktopReferenceDataSummaryFileService _desktopReferenceDataSummaryFileService;
        private readonly ILogger _logger;

        public DesktopReferenceDataTask(
            IDesktopReferenceDataPopulationService referenceDataPopulationService,
            IDesktopReferenceDataFileService desktopReferenceDataFileService,
            IDesktopReferenceDataSummaryFileService desktopReferenceDataSummaryFileService,
            ILogger logger)
        {
            _referenceDataPopulationService = referenceDataPopulationService;
            _desktopReferenceDataFileService = desktopReferenceDataFileService;
            _desktopReferenceDataSummaryFileService = desktopReferenceDataSummaryFileService;
            _logger = logger;
        }

        public async Task ExecuteAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            try
            {
                // get reference data and build model.
                _logger.LogInfo("Starting Reference Data Population");
                var referenceData = await _referenceDataPopulationService.PopulateAsync(cancellationToken);
                _logger.LogInfo("Finished Reference Data Population");

                // output model.
                _logger.LogInfo("Starting Reference Data Output");
                //await _desktopReferenceDataFileService.ProcessAsync(referenceDataContext, referenceData, cancellationToken);
                _logger.LogInfo("Finished Reference Data Output");

                // output summary.
                _logger.LogInfo("Starting Reference Summary Output");
                await _desktopReferenceDataSummaryFileService.ProcessAync(referenceDataContext, cancellationToken);
                _logger.LogInfo("Finished Reference Summary Output");
            }
            catch (Exception exception)
            {
                _logger.LogError("Reference Data Service Output Exception", exception);
                throw;
            }
        }
    }
}
