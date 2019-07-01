using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Exception;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Context;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Logging.Interfaces;
using ExecutionContext = ESFA.DC.Logging.ExecutionContext;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless
{
    public class MessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly IIndex<TaskKeys, ITask> _taskIndex;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger _logger;

        public MessageHandler(IIndex<TaskKeys, ITask> taskIndex, ILifetimeScope lifetimeScope, ILogger logger)
        {
            _taskIndex = taskIndex;
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

                try
                {
                    var task = GetTask(message);

                    await _taskIndex[task].ExecuteAsync(referenceDataContext, cancellationToken);
                }
                catch (ReferenceDataServiceFailureException exception)
                {
                    _logger.LogError("Reference Data Service Message Handler Exception", exception);
                }

                return true;
            }
        }

        private TaskKeys GetTask(JobContextMessage message)
        {
            return (TaskKeys)Enum.Parse(typeof(TaskKeys),
                message.Topics[message.TopicPointer].Tasks.SelectMany(x => x.Tasks).First());
        }
    }
}
