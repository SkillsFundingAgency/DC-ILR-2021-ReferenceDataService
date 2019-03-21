using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.ULN.Model;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class UlnServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var ulns = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 31, 32, 33, 34, 35 };

            var ulnMock = new Mock<IUlnContext>();

            IEnumerable<UniqueLearnerNumber> ulnList = new List<UniqueLearnerNumber>
            {
                new UniqueLearnerNumber { Uln = 1 },
                new UniqueLearnerNumber { Uln = 2 },
                new UniqueLearnerNumber { Uln = 3 },
                new UniqueLearnerNumber { Uln = 4 },
                new UniqueLearnerNumber { Uln = 5 },
                new UniqueLearnerNumber { Uln = 6 },
                new UniqueLearnerNumber { Uln = 7 },
                new UniqueLearnerNumber { Uln = 8 },
                new UniqueLearnerNumber { Uln = 9 },
                new UniqueLearnerNumber { Uln = 10 },
                new UniqueLearnerNumber { Uln = 11 },
                new UniqueLearnerNumber { Uln = 12 },
                new UniqueLearnerNumber { Uln = 13 },
                new UniqueLearnerNumber { Uln = 14 },
                new UniqueLearnerNumber { Uln = 15 },
                new UniqueLearnerNumber { Uln = 16 },
                new UniqueLearnerNumber { Uln = 17 },
                new UniqueLearnerNumber { Uln = 18 },
                new UniqueLearnerNumber { Uln = 19 },
                new UniqueLearnerNumber { Uln = 20 },
                new UniqueLearnerNumber { Uln = 35 }
            };

            var ulnDbMock = ulnList.AsQueryable().BuildMockDbSet();

            ulnMock.Setup(u => u.UniqueLearnerNumbers).Returns(ulnDbMock.Object);

            var serviceResult = await NewService(ulnMock.Object).RetrieveAsync(ulns, CancellationToken.None);

            serviceResult.Count().Should().Be(21);
            serviceResult.Select(u => u.UniqueLearnerNumber).ToList().Should().BeEquivalentTo(ulnList.Select(u => u.Uln).ToList());
        }

        private UlnRepositoryService NewService(IUlnContext uln = null)
        {
            return new UlnRepositoryService(uln);
        }
    }
}
