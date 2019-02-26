using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class OrganisationsServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var ukprns = new List<int> { 1, 2, 3 };

            var organisationsMock = new Mock<IOrganisationsContext>();

            IEnumerable<MasterOrganisation> masterOrgList = new List<MasterOrganisation>
            {
                new MasterOrganisation
                {
                    Ukprn = 1,
                    OrgDetail = new OrgDetail
                    {
                        Ukprn = 1,
                        LegalOrgType = "LegalType1"
                    },
                   OrgPartnerUkprns = new List<OrgPartnerUkprn>
                   {
                       new OrgPartnerUkprn
                       {
                           Ukprn = 1,
                           NameLegal = "NameLegal1"
                       }
                   }
                },
                new MasterOrganisation
                {
                    Ukprn = 2,
                    OrgDetail = new OrgDetail
                    {
                        Ukprn = 2,
                        LegalOrgType = "LegalType2"
                    }
                },
            };

            IEnumerable<CampusIdentifier> campusIdentifiersList = new List<CampusIdentifier>
            {
                new CampusIdentifier
                {
                    MasterUkprn = 1,
                    CampusIdentifier1 = "CampId_01"
                }
            };

            var masterOrgMock = masterOrgList.AsQueryable().BuildMockDbSet();
            var campusIdentifiersMock = campusIdentifiersList.AsQueryable().BuildMockDbSet();

            organisationsMock.Setup(o => o.MasterOrganisations).Returns(masterOrgMock.Object);
            organisationsMock.Setup(o => o.CampusIdentifiers).Returns(campusIdentifiersMock.Object);

            var organisations = await NewService(organisationsMock.Object).RetrieveAsync(ukprns, CancellationToken.None);

            organisations.Select(k => k.Key).Should().HaveCount(2);
            organisations.Select(k => k.Key).Should().Contain(1);
            organisations.Select(k => k.Key).Should().Contain(2);
            organisations.Select(k => k.Key).Should().NotContain(3);

            organisations[1].UKPRN.Should().Be(1);
            organisations[1].LegalOrgType.Should().Be("LegalType1");
            organisations[1].CampusIdentifers.Single().Should().Be("CampId_01");
            organisations[1].PartnerUKPRN.Should().BeTrue();

            organisations[2].UKPRN.Should().Be(2);
            organisations[2].LegalOrgType.Should().Be("LegalType2");
            organisations[2].CampusIdentifers.Should().BeNullOrEmpty();
            organisations[2].PartnerUKPRN.Should().BeFalse();
        }

        private OrganisationsService NewService(IOrganisationsContext organisations = null)
        {
            return new OrganisationsService(organisations);
        }
    }
}
