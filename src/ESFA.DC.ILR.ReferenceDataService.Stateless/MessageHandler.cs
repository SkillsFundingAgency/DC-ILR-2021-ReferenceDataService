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

        public MessageHandler(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
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
                    await referenceDataOrchestrationService.Retrieve(referenceDataContext, cancellationToken);
                }
                catch (ReferenceDataServiceFailureException exception)
                {
                }

                return true;
            }
        }
    }
}
