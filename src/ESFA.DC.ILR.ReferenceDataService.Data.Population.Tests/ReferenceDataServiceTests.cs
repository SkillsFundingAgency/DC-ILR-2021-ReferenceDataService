using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.ULNs;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests
{
    public class ReferenceDataServiceTests
    {
        [Fact]
        public async void Retrieve()
        {
            var message = new TestMessage();
            var ulns = new List<long> { 1, 2, 3, 4, 5 };
            IReadOnlyCollection<ULN> expectedUlns = new List<ULN>
            {
                new ULN { UniqueLearnerNumber = 1 },
                new ULN { UniqueLearnerNumber = 2 },
                new ULN { UniqueLearnerNumber = 3 },
                new ULN { UniqueLearnerNumber = 4 },
                new ULN { UniqueLearnerNumber = 5 },
            };

            var messageMapperMock = new Mock<IMessageMapper<IReadOnlyCollection<long>>>();
            var retrievalServiceMock = new Mock<IRetrievalService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>>();

            messageMapperMock.Setup(mm => mm.MapFromMessage(message)).Returns(ulns);
            retrievalServiceMock.Setup(rs => rs.RetrieveAsync(ulns, CancellationToken.None)).Returns(Task.FromResult(expectedUlns));

            var result = await new ReferenceDataService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>(
                messageMapperMock.Object, retrievalServiceMock.Object)
                .Retrieve(message, CancellationToken.None);

            result.Should().HaveCount(5);
            result.Should().BeEquivalentTo(expectedUlns);
        }
    }
}
