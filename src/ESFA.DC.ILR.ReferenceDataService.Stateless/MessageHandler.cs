using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Exception;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
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
                    foreach (var task in GetTasks(message))
                    {
                        await _taskIndex[task].ExecuteAsync(referenceDataContext, cancellationToken);
                    }
                }
                catch (ReferenceDataServiceFailureException exception)
                {
                    _logger.LogError("Reference Data Service Message Handler Exception", exception);
                }

                return true;
            }
        }

        private IEnumerable<TaskKeys> GetTasks(JobContextMessage message)
        {
            var tasks = message.Topics[message.TopicPointer].Tasks.SelectMany(x => x.Tasks);

            foreach (var task in tasks)
            {
                yield return (TaskKeys)Enum.Parse(typeof(TaskKeys), task);
            }
        }
    }
}