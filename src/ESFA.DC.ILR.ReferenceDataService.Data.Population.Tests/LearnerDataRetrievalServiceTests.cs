using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Learner;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests
{
    public class LearnerDataRetrievalServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var cancellationToken = CancellationToken.None;
            var message = TestMessage();
            var ukprns = new List<int> { 1, 2, 3, 4 };
            var learnRefNumbers = new List<string> { "Learner1", "Learner2", "Learner3", "PrevLearner1", "PrevLearner2" };

            var learnerReferenceData = new List<Learner>
            {
                new Learner
                {
                    UKPRN = 1,
                    LearnRefNumber = "Learner1"
                },
                new Learner
                {
                    UKPRN = 1,
                    LearnRefNumber = "Learner2"
                },
                new Learner
                {
                    UKPRN = 1,
                    LearnRefNumber = "Learner3"
                },
                new Learner
                {
                    UKPRN = 2,
                    LearnRefNumber = "PrevLearner1"
                },
                new Learner
                {
                    UKPRN = 3,
                    LearnRefNumber = "PrevLearner2"
                }
            };

            var ukprnsMapper = new Mock<IUkprnsMapper>();
            var learnRefNumberMapper = new Mock<ILearnRefNumberMapper>();
            var learnerReferenceDataRepositoryService = new Mock<ILearnerReferenceDataRepositoryService>();

            ukprnsMapper.Setup(x => x.MapUKPRNsFromMessage(message)).Returns(ukprns);
            learnRefNumberMapper.Setup(x => x.MapLearnRefNumbersFromMessage(message)).Returns(learnRefNumbers);
            learnerReferenceDataRepositoryService.Setup(x => x.RetrieveLearnerReferenceDataAsync(ukprns, learnRefNumbers, cancellationToken)).ReturnsAsync(learnerReferenceData);

            var referenceData = await NewService(ukprnsMapper.Object, learnRefNumberMapper.Object, learnerReferenceDataRepositoryService.Object).RetrieveAsync(message, cancellationToken);

            referenceData.Should().BeEquivalentTo(new LearnerReferenceData { Learners = learnerReferenceData });
        }

        private TestMessage TestMessage()
        {
            return new TestMessage
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = 1,
                },
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        PMUKPRNNullable = 2,
                        LearnRefNumber = "Learner1",
                        PrevLearnRefNumber = "PrevLearner1",
                    },
                    new TestLearner
                    {
                        PrevUKPRNNullable = 3,
                        LearnRefNumber = "Learner2",
                        PrevLearnRefNumber = "PrevLearner2",
                    },
                    new TestLearner
                    {
                        PMUKPRNNullable = 4,
                        LearnRefNumber = "Learner3",
                        ULN = 3,
                    },
                }
            };
        }

        private LearnerDataRetrievalService NewService(
            IUkprnsMapper ukprnsMapper = null,
            ILearnRefNumberMapper learnRefNumberMapper = null,
            ILearnerReferenceDataRepositoryService learnerReferenceDataRepositoryService = null)
        {
            return new LearnerDataRetrievalService(ukprnsMapper, learnRefNumberMapper, learnerReferenceDataRepositoryService);
        }
    }
}
