using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop
{
    public class DesktopMessageTask : IDesktopTask
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IReferenceDataPopulationService _referenceDataPopulationService;
        private readonly IGzipFileProvider _gZipFileProvider;
        private readonly ILogger _logger;

        public DesktopMessageTask(
            IMessageProvider messageProvider,
            IReferenceDataPopulationService referenceDataPopulationService,
            IGzipFileProvider gZipFileProvider,
            ILogger logger)
        {
            _messageProvider = messageProvider;
            _referenceDataPopulationService = referenceDataPopulationService;
            _gZipFileProvider = gZipFileProvider;
            _logger = logger;
        }

        public async Task<IDesktopContext> ExecuteAsync(IDesktopContext desktopContext, CancellationToken cancellationToken)
        {

            // Create context
            var referenceDataContext = new ReferenceDataJobContextMessageContext(desktopContext);


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
            await _gZipFileProvider.CompressAndStoreAsync(referenceDataContext, referenceData, cancellationToken);
            _logger.LogInfo("Finished Reference Data Output");

            return desktopContext;


        }
    }
}
