using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class DesktopReferenceDataPopulationService : IReferenceDataPopulationService
    {
        private readonly IMessageMapperService _messageMapperService;
        private readonly IDesktopReferenceDataMapperService _desktopReferenceDataRootMapperService;
        private readonly ILogger _logger;

        public DesktopReferenceDataPopulationService(
            IMessageMapperService messageMapperService,
            IDesktopReferenceDataMapperService desktopReferenceDataRootMapperService,
            ILogger logger)
        {
            _messageMapperService = messageMapperService;
            _desktopReferenceDataRootMapperService = desktopReferenceDataRootMapperService;
            _logger = logger;
        }

        public async Task<ReferenceDataRoot> PopulateAsync(IReferenceDataContext referenceDataContext, IMessage message, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Reference Data Parameter Mapping from Message");
            var mapperData = _messageMapperService.MapFromMessage(message);
            _logger.LogInfo("Finished Reference Data Parameter Mapping from Message");

            _logger.LogInfo("Starting Reference Data Retrieval from sources");
            var desktopReferenceData = await _desktopReferenceDataRootMapperService.MapReferenceData(referenceDataContext, mapperData, cancellationToken);
            _logger.LogInfo("Finished Reference Data Retrieval from sources");

            return desktopReferenceData;
        }
    }
}
