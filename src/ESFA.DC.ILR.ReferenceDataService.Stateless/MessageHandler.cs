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
        private readonly IDesktopReferenceDataTask _desktopReferenceDataTask;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger _logger;

        public MessageHandler(IIndex<TaskKeys, ITask> taskIndex, IDesktopReferenceDataTask desktopReferenceDataTask, ILifetimeScope lifetimeScope, ILogger logger)
        {
            _taskIndex = taskIndex;
            _desktopReferenceDataTask = desktopReferenceDataTask;
            _lifetimeScope = lifetimeScope;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(JobContextMessage message, CancellationToken cancellationToken)
        {
            using (var childLifetimeScope = _lifetimeScope.BeginLifetimeScope())
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = message.JobId.ToString();

                try
                {
                    if (GetDesktopRefDataTasks(message).Contains(TaskKeys.DesktopReferenceData))
                    {
                        var context = new DesktopReferenceDataJobContext(message);

                        await _desktopReferenceDataTask.ExecuteAsync(context, cancellationToken);
                    }

                    foreach (var task in GetIlrTasks(message))
                    {
                        var referenceDataContext = new IlrMessageJobContext(message);

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

        public IEnumerable<TaskKeys> GetDesktopRefDataTasks(JobContextMessage message)
        {
            var tasks = message.Topics[message.TopicPointer].Tasks.SelectMany(x => x.Tasks);

            foreach (var task in tasks.Where(x => x == TaskKeys.DesktopReferenceData.ToString()))
            {
                yield return (TaskKeys)Enum.Parse(typeof(TaskKeys), task);
            }
        }

        public IEnumerable<TaskKeys> GetIlrTasks(JobContextMessage message)
        {
            var tasks = message.Topics[message.TopicPointer].Tasks.SelectMany(x => x.Tasks);

            foreach (var task in tasks.Where(x => x != TaskKeys.DesktopReferenceData.ToString()))
            {
                yield return (TaskKeys)Enum.Parse(typeof(TaskKeys), task);
            }
        }
    }
}