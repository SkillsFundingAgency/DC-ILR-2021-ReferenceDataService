using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class EpaOrganisationsRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var epaOrgIds = new List<string> { "EPAORG1", "EPAORG2", "EPAORG3" };

            IEnumerable<Period> periodsList = new List<Period>
            {
                new Period
                {
                    OrganisationId = "EpaOrg1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1",
                },
                new Period
                {
                    OrganisationId = "EpaOrg1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "2",
                },
                new Period
                {
                    OrganisationId = "EpaOrg2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1",
                },
                new Period
                {
                    OrganisationId = "EpaOrg3",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1",
                },
                new Period
                {
                    OrganisationId = "EpaOrg3",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "2",
                },
                new Period
                {
                    OrganisationId = "EpaOrg4",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1",
                },
            };

            var periodsMock = periodsList.AsQueryable().BuildMockDbSet();

            var epaMock = new Mock<IEpaContext>();
            var epaContextFactoryMock = new Mock<IDbContextFactory<IEpaContext>>();
            epaContextFactoryMock.Setup(c => c.Create()).Returns(epaMock.Object);

            epaMock.Setup(o => o.Periods).Returns(periodsMock.Object);

            var epaOganisations = await NewService(epaContextFactoryMock.Object).RetrieveAsync(epaOrgIds, CancellationToken.None);

            epaOganisations.Should().HaveCount(5);
            epaOganisations.Select(e => e.ID).Should().Contain("EPAORG1");
            epaOganisations.Select(e => e.ID).Should().Contain("EPAORG2");
            epaOganisations.Select(e => e.ID).Should().Contain("EPAORG3");
            epaOganisations.Select(e => e.ID).Should().NotContain("EPAORG4");

            epaOganisations.Where(e => e.ID == "EPAORG1").Should().HaveCount(2);
            epaOganisations.Where(e => e.ID == "EPAORG2").Should().HaveCount(1);
            epaOganisations.Where(e => e.ID == "EPAORG3").Should().HaveCount(2);

            epaOganisations.Where(e => e.ID == "EPAORG1").Select(e => e.Standard).Should().Contain("1", "2");
            epaOganisations.Where(e => e.ID == "EPAORG2").Select(e => e.Standard).Should().Contain("1");
            epaOganisations.Where(e => e.ID == "EPAORG3").Select(e => e.Standard).Should().Contain("1", "2");
        }

        private EpaOrganisationsRepositoryService NewService(IDbContextFactory<IEpaContext> epaContextFactory = null)
        {
            return new EpaOrganisationsRepositoryService(epaContextFactory);
        }
    }
}
