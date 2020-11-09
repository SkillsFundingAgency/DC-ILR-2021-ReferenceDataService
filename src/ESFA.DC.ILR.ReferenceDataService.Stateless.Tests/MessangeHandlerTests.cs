using System.Collections.Generic;
using Autofac;
using Autofac.Features.Indexed;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobContextManager.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Tests
{
    public class MessangeHandlerTests
    {
        [Fact]
        public void GetDesktopRefDataTasks()
        {
            var jobContextMessage = new JobContextMessage
            {
                TopicPointer = 0,
                Topics = new ITopicItem[]
                {
                    new TopicItem
                    {
                        SubscriptionName = "ReferenceDataRetrieval",
                        Tasks = new ITaskItem[]
                        {
                            new TaskItem
                            {
                                Tasks = new List<string>
                                {
                                    "DesktopReferenceData",
                                    "IlrMessage",
                                    "LearnerReferenceData",
                                    "FrmReferenceData"
                                },
                                SupportsParallelExecution = false
                            },
                        }
                    }
                }
            };

            var expectedTasks = new List<TaskKeys>
            {
                TaskKeys.DesktopReferenceData
            };

            var tasks = NewHandler().GetDesktopRefDataTasks(jobContextMessage);

            tasks.Should().BeEquivalentTo(expectedTasks);
        }


        [Fact]
        public void GetIlrTasks()
        {
            var jobContextMessage = new JobContextMessage
            {
                TopicPointer = 0,
                Topics = new ITopicItem[]
                {
                    new TopicItem
                    {
                        SubscriptionName = "ReferenceDataRetrieval",
                        Tasks = new ITaskItem[]
                        {
                            new TaskItem
                            {
                                Tasks = new List<string>
                                {
                                    "DesktopReferenceData",
                                    "IlrMessage",
                                    "LearnerReferenceData",
                                    "FrmReferenceData"
                                },
                                SupportsParallelExecution = false
                            },
                        }
                    }
                }
            };

            var expectedTasks = new List<TaskKeys>
            {
                TaskKeys.IlrMessage,
                TaskKeys.LearnerReferenceData,
                TaskKeys.FrmReferenceData
            };

            var tasks = NewHandler().GetIlrTasks(jobContextMessage);

            tasks.Should().BeEquivalentTo(expectedTasks);
        }

        private MessageHandler NewHandler(ILifetimeScope lifetimeScope = null)
        {
            return new MessageHandler(Mock.Of<IIndex<TaskKeys, ITask>>(), Mock.Of<IDesktopReferenceDataTask>(), lifetimeScope ?? Mock.Of<ILifetimeScope>(), Mock.Of<ILogger>());
        }
    }
}
