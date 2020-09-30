using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class McaDevolvedContractsRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var ukprn = 1;

            var devolvedContracts = new List<DevolvedContract>
            {
                new DevolvedContract
                {
                    Ukprn = 1,
                    McaglashortCode = "Code1",
                    EffectiveFrom = new DateTime(2019, 8, 1)
                },
                new DevolvedContract
                {
                    Ukprn = 1,
                    McaglashortCode = "Code2",
                    EffectiveFrom = new DateTime(2019, 8, 1)
                },
                new DevolvedContract
                {
                    Ukprn = 1,
                    McaglashortCode = "Code3",
                    EffectiveFrom = new DateTime(2019, 8, 1)
                },
                new DevolvedContract
                {
                    Ukprn = 2,
                    McaglashortCode = "Code1",
                    EffectiveFrom = new DateTime(2019, 8, 1)
                },
                new DevolvedContract
                {
                    Ukprn = 3,
                    McaglashortCode = "Code1",
                    EffectiveFrom = new DateTime(2019, 8, 1)
                }
            };

            var mcaDevolvedContractsMock = devolvedContracts.AsQueryable().BuildMockDbSet();

            var fcsMock = new Mock<IFcsContext>();

            fcsMock.Setup(f => f.DevolvedContracts).Returns(mcaDevolvedContractsMock.Object);

            var fcsContextFactoryMock = new Mock<IDbContextFactory<IFcsContext>>();
            fcsContextFactoryMock.Setup(c => c.Create()).Returns(fcsMock.Object);

            var service = await NewService(fcsContextFactoryMock.Object).RetrieveAsync(ukprn, CancellationToken.None);

            service.Should().HaveCount(3);
            service.Select(f => f.Ukprn).FirstOrDefault().Should().Be(1);
            service.Select(f => f.McaGlaShortCode).Should().BeEquivalentTo(new List<string> { "Code1", "Code2", "Code3" });
        }

        private McaDevolvedContractsRepositoryService NewService(IDbContextFactory<IFcsContext> fcsContextFactory = null)
        {
            return new McaDevolvedContractsRepositoryService(fcsContextFactory);
        }
    }
}