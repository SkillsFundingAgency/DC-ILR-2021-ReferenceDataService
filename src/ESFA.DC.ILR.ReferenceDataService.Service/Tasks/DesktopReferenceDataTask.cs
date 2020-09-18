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
        private readonly ILogger _logger;

        public DesktopReferenceDataTask(
            IDesktopReferenceDataPopulationService referenceDataPopulationService,
            IDesktopReferenceDataFileService desktopReferenceDataFileService,
            ILogger logger)
        {
            _referenceDataPopulationService = referenceDataPopulationService;
            _desktopReferenceDataFileService = desktopReferenceDataFileService;
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
                await _desktopReferenceDataFileService.ProcessAync(referenceDataContext.Container, referenceData, cancellationToken);
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
