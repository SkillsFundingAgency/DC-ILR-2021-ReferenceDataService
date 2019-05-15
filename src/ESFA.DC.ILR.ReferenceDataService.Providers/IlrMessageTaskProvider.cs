using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Providers
{
    public class IlrMessageTaskProvider : IIlrMessageTaskProvider
    {
        private const string IlrMessageTask = "IlrMessage";
        private readonly IIlrMessageTask _ilrMessageTask;
        private readonly ILogger _logger;

        public IlrMessageTaskProvider(IIlrMessageTask ilrMessageTask, ILogger logger)
        {
            _ilrMessageTask = ilrMessageTask;
            _logger = logger;
        }

        public Task Provide(IReferenceDataContext referenceDataContext, CancellationToken cancellationToken)
        {
            if (referenceDataContext.Tasks.Contains(IlrMessageTask))
            {
                _logger.LogInfo("Adding ILR Message Task");
                return _ilrMessageTask.Execute(referenceDataContext, cancellationToken);
            }
            else
            {
                throw new ArgumentException("Task missing or incorrect from Message Topic.");
            }
        }
    }
}
