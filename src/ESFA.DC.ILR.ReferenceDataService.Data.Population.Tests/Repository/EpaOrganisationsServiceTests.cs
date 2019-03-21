using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class EpaOrganisationsServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var epaOrgIds = new List<string> { "EpaOrg1", "EpaOrg2", "EpaOrg3" };

            var epaMock = new Mock<IEpaContext>();

            IEnumerable<Period> periodsList = new List<Period>
            {
                new Period
                {
                    OrganisationId = "EpaOrg1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1"
                },
                new Period
                {
                    OrganisationId = "EpaOrg1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "2"
                },
                new Period
                {
                    OrganisationId = "EpaOrg2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1"
                },
                new Period
                {
                    OrganisationId = "EpaOrg3",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1"
                },
                new Period
                {
                    OrganisationId = "EpaOrg3",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "2"
                },
                new Period
                {
                    OrganisationId = "EpaOrg4",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    StandardCode = "1"
                }
            };

            var periodsMock = periodsList.AsQueryable().BuildMockDbSet();

            epaMock.Setup(o => o.Periods).Returns(periodsMock.Object);

            var epaOganisations = await NewService(epaMock.Object).RetrieveAsync(epaOrgIds, CancellationToken.None);

            epaOganisations.Select(k => k.Key).Should().HaveCount(3);
            epaOganisations.Select(k => k.Key).Should().Contain("EpaOrg1");
            epaOganisations.Select(k => k.Key).Should().Contain("EpaOrg2");
            epaOganisations.Select(k => k.Key).Should().Contain("EpaOrg3");
            epaOganisations.Select(k => k.Key).Should().NotContain("EpaOrg4");

            epaOganisations["EpaOrg1"].Should().HaveCount(2);
            epaOganisations["EpaOrg2"].Should().HaveCount(1);
            epaOganisations["EpaOrg3"].Should().HaveCount(2);

            epaOganisations["EpaOrg1"].Select(e => e.Standard).Should().Contain("1", "2");
            epaOganisations["EpaOrg2"].Select(e => e.Standard).Should().Contain("1");
            epaOganisations["EpaOrg3"].Select(e => e.Standard).Should().Contain("1", "2");
        }

        private EpaOrganisationsRepositoryService NewService(IEpaContext epaContext = null)
        {
            return new EpaOrganisationsRepositoryService(epaContext);
        }
    }
}
