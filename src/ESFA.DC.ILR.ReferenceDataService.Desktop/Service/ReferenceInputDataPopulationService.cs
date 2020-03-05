using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class ReferenceInputDataPopulationService : IReferenceInputDataPopulationService
    {
        private readonly IReferenceInputDataMapperService _desktopReferenceDataRootMapperService;
        private readonly IReferenceInputEFMapper _referenceInputEFMapper;
        private readonly IEFModelIdentityAssigner _efModelIdentityAssigner;
        private readonly IReferenceInputTruncator _referenceInputTruncator;
        private readonly IReferenceInputPersistence _referenceInputPersistenceService;
        private readonly ILogger _logger;

        public ReferenceInputDataPopulationService(
            IReferenceInputDataMapperService desktopReferenceDataRootMapperService,
            IReferenceInputEFMapper referenceInputEFMapper,
            IEFModelIdentityAssigner efModelIdentityAssigner,
            IReferenceInputTruncator referenceInputTruncator,
            IReferenceInputPersistence referenceInputPersistenceService,
            ILogger logger)
        {
            _desktopReferenceDataRootMapperService = desktopReferenceDataRootMapperService;
            _referenceInputEFMapper = referenceInputEFMapper;
            _efModelIdentityAssigner = efModelIdentityAssigner;
            _referenceInputTruncator = referenceInputTruncator;
            _referenceInputPersistenceService = referenceInputPersistenceService;
            _logger = logger;
        }

        public async Task<bool> PopulateAsync(IInputReferenceDataContext inputReferenceDataContext, CancellationToken cancellationToken)
        {
            _logger.LogInfo("Starting Reference Data Retrieval from sources");
            var desktopReferenceData = await _desktopReferenceDataRootMapperService.MapReferenceData(inputReferenceDataContext, cancellationToken);
            _logger.LogInfo("Finished Reference Data Retrieval from sources");

            _logger.LogInfo("Starting Reference Data mapping from models to EF models");
            var efReferenceInputDataRoot = _referenceInputEFMapper.Map(desktopReferenceData);
            _logger.LogInfo("Finished Reference Data mapping from models to EF models");

            _logger.LogInfo("Starting assigning ID's to EF models");
            _efModelIdentityAssigner.AssignIds(efReferenceInputDataRoot);
            _logger.LogInfo("Finished assigning ID's to EF models");

            _logger.LogInfo("Starting Truncate existing data");
            await _referenceInputTruncator.TruncateReferenceDataAsync(inputReferenceDataContext, cancellationToken);
            _logger.LogInfo("Finished Truncate existing data");

            _logger.LogInfo("Starting persisting EF Models to Db");
            await _referenceInputPersistenceService.PersistEFModelsAsync(inputReferenceDataContext, efReferenceInputDataRoot, cancellationToken);
            _logger.LogInfo("Finished persisting EF Models to Db");

            return true;
        }
    }
}
