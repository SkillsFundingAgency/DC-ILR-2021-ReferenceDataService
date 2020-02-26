using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class ReferenceInputDataPopulationService : IReferenceInputDataPopulationService
    {
        private readonly IReferenceInputDataMapperService _desktopReferenceDataRootMapperService;
        private readonly IReferenceInputEFMapper _referenceInputEFMapper;
        private readonly IReferenceInputPersistanceService _referenceInputPersistanceService;
        private readonly ILogger _logger;

        public ReferenceInputDataPopulationService(
            IReferenceInputDataMapperService desktopReferenceDataRootMapperService,
            IReferenceInputEFMapper referenceInputEFMapper,
            IReferenceInputPersistanceService referenceInputPersistanceService,
            ILogger logger)
        {
            _desktopReferenceDataRootMapperService = desktopReferenceDataRootMapperService;
            _referenceInputEFMapper = referenceInputEFMapper;
            _referenceInputPersistanceService = referenceInputPersistanceService;
            _logger = logger;
        }

        public async Task<DesktopReferenceDataRoot> PopulateAsync(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Reference Data Retrieval from sources");
            var desktopReferenceData = await _desktopReferenceDataRootMapperService.MapReferenceData(referenceDataContext, cancellationToken);
            _logger.LogInfo("Finished Reference Data Retrieval from sources");

            var efmodels = _referenceInputEFMapper.Map(referenceDataContext, desktopReferenceData, cancellationToken);

            _referenceInputPersistanceService.PersistModels(referenceDataContext, efmodels, cancellationToken);

            return desktopReferenceData;
        }
    }
}
