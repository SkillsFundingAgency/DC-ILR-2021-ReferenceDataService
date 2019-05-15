using System;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ReferenceDataOrchestrationService : IReferenceDataOrchestrationService
    {
        private readonly IIlrMessageTaskProvider _taskProvider;
        private readonly ILogger _logger;

        public ReferenceDataOrchestrationService(
            IIlrMessageTaskProvider taskProvider,
            ILogger logger)
        {
            _taskProvider = taskProvider;
            _logger = logger;
        }

        public async Task Process(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            try
            {
                var tasks = _taskProvider.Provide(referenceDataContext, cancellationToken);

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError("Reference Data Service Output Exception", exception);
            }
        }
    }
}
