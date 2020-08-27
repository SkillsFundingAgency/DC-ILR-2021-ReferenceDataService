using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.Learner;
using ESFA.DC.ILR1920.DataStore.EF.Valid.Interface;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class LearnerReferenceDataRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var cancellationToken = CancellationToken.None;
            IReadOnlyCollection<string> learnRefNumbers = new List<string> { "LearnRefNumber1", "LearnRefNumber2", "LearnRefNumber3", "LearnRefNumber4" };
            var learnRefNumbersJson = @"[""LearnRefNumber1"",""LearnRefNumber2"",""LearnRefNumber3"",""LearnRefNumber4""]";

            IReadOnlyCollection<int> ukprns = new List<int> { 1, 2, 3, 4 };
            var ukprnsJson = @"[1,2,3,4]";

            var dbResult = new TaskCompletionSource<IEnumerable<Learner>>();

            var learners = new List<Learner>
            {
                new Learner
                {
                    LearnRefNumber = "LearnRefNumber1",
                },
                new Learner
                {
                    LearnRefNumber = "LearnRefNumber2",
                },
                new Learner
                {
                    LearnRefNumber = "LearnRefNumber3",
                }
            };

            dbResult.SetResult(learners);

            var jsonSerializationMock = new Mock<IJsonSerializationService>();

            jsonSerializationMock.Setup(sm => sm.Serialize(ukprns)).Returns(ukprnsJson);
            jsonSerializationMock.Setup(sm => sm.Serialize(learnRefNumbers)).Returns(learnRefNumbersJson);

            var contextMock = new Mock<IILR1920_DataStoreEntitiesValid>();
            var contextFactoryMock = new Mock<IDbContextFactory<IILR1920_DataStoreEntitiesValid>>();
            contextFactoryMock.Setup(c => c.Create()).Returns(contextMock.Object);

            var service = NewServiceMock(contextFactoryMock.Object, jsonSerializationService: jsonSerializationMock.Object);

            service.Setup(s => s.RetrieveAsync(ukprnsJson, learnRefNumbersJson, It.IsAny<string>(), cancellationToken)).Returns(dbResult.Task);

            var serviceResult = await service.Object.RetrieveLearnerReferenceDataAsync(ukprns, learnRefNumbers, cancellationToken);

            IReadOnlyCollection<Learner> expectedResult = learners;

            serviceResult.Should().BeEquivalentTo(expectedResult);
        }

        private Mock<LearnerReferenceDataRepositoryService> NewServiceMock(
             IDbContextFactory<IILR1920_DataStoreEntitiesValid> contextFactory = null,
             IReferenceDataOptions referenceDataOptions = null,
             IJsonSerializationService jsonSerializationService = null)
        {
            return new Mock<LearnerReferenceDataRepositoryService>(contextFactory, referenceDataOptions, jsonSerializationService);
        }
    }
}
