using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Exception;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Context;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Logging.Interfaces;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless
{
    public class MessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger _logger;

        public MessageHandler(ILifetimeScope lifetimeScope, ILogger logger)
        {
            _lifetimeScope = lifetimeScope;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(JobContextMessage message, CancellationToken cancellationToken)
        {
            var referenceDataContext = new ReferenceDataJobContextMessageContext(message);

            using (var childLifetimeScope = _lifetimeScope.BeginLifetimeScope())
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = message.JobId.ToString();

                var referenceDataOrchestrationService = childLifetimeScope.Resolve<IReferenceDataOrchestrationService>();

                try
                {
                    await referenceDataOrchestrationService.Process(referenceDataContext, cancellationToken);
                }
                catch (ReferenceDataServiceFailureException exception)
                {
                    _logger.LogError("Reference Data Service Message Handler Exception", exception);
                }

                return true;
            }
        }
    }
}
