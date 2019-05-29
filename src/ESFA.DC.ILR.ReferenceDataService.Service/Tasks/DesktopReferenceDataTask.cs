using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tasks
{
    public class DesktopReferenceDataTask : ITask
    {
        private readonly bool compressOutput = true;
        private readonly IDesktopReferenceDataPopulationService _referenceDataPopulationService;
        private readonly IFileProvider _gZipFileProvider;
        private readonly ILogger _logger;

        public DesktopReferenceDataTask(
            IDesktopReferenceDataPopulationService referenceDataPopulationService,
            IFileProvider gZipFileProvider,
            ILogger logger)
        {
            _referenceDataPopulationService = referenceDataPopulationService;
            _gZipFileProvider = gZipFileProvider;
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
                await _gZipFileProvider.StoreAsync(referenceDataContext, referenceData, compressOutput, cancellationToken);
                _logger.LogInfo("Finished Reference Data Output");
            }
            catch (Exception exception)
            {
                _logger.LogError("Reference Data Service Output Exception", exception);
            }
        }
    }
}
