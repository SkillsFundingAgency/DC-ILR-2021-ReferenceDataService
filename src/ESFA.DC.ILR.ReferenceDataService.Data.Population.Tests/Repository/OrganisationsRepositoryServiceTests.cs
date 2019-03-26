using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class OrganisationsRepositoryServiceTests
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
                   },
                   OrgFundings = new List<OrgFunding>
                   {
                       new OrgFunding
                       {
                            FundingFactor = "FACTOR1",
                            FundingFactorType = "FACTOR TYPE1",
                            FundingFactorValue = "1.5",
                            EffectiveFrom = new DateTime(2018, 8, 1)
                       },
                       new OrgFunding
                       {
                            FundingFactor = "FACTOR2",
                            FundingFactorType = "FACTOR TYPE1",
                            FundingFactorValue = "1.5",
                            EffectiveFrom = new DateTime(2018, 8, 1)
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
            organisations[1].OrganisationFundings.Should().HaveCount(2);

            organisations[2].UKPRN.Should().Be(2);
            organisations[2].LegalOrgType.Should().Be("LegalType2");
            organisations[2].CampusIdentifers.Should().BeNullOrEmpty();
            organisations[2].PartnerUKPRN.Should().BeFalse();
            organisations[2].OrganisationFundings.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetCampusIdentifiers()
        {
            var campusListOne = new List<string> { "CampisId1", "CampisId2" };
            var campusListTwo = new List<string> { "CampisId3", "CampisId4" };
            var campusListThree = new List<string> { "CampisId5", "CampisId6" };

            var dictionary = new Dictionary<long, List<string>>
            {
                { 1, campusListOne },
                { 2, campusListTwo },
                { 3, campusListThree }
            };

            NewService().GetCampusIdentifiers(1, dictionary).Should().BeEquivalentTo(campusListOne);
        }

        [Fact]
        public void GetCampusIdentifiers_MisMatch()
        {
            var campusListOne = new List<string> { "CampisId1", "CampisId2" };
            var campusListTwo = new List<string> { "CampisId3", "CampisId4" };
            var campusListThree = new List<string> { "CampisId5", "CampisId6" };

            var dictionary = new Dictionary<long, List<string>>
            {
                { 1, campusListOne },
                { 2, campusListTwo },
                { 3, campusListThree }
            };

            NewService().GetCampusIdentifiers(4, dictionary).Should().BeNull();
        }

        [Fact]
        public void GetCampusIdentifiers_Null()
        {
            var campusListOne = new List<string> { "CampisId1", "CampisId2" };
            var campusListTwo = new List<string> { "CampisId3", "CampisId4" };
            var campusListThree = new List<string> { "CampisId5", "CampisId6" };

            var dictionary = new Dictionary<long, List<string>>
            {
                { 1, campusListOne },
                { 2, campusListTwo },
                { 3, campusListThree }
            };

            NewService().GetCampusIdentifiers(1, new Dictionary<long, List<string>>()).Should().BeNull();
        }

        [Fact]
        public void OrgFundingFromEntity()
        {
            var orgFunding = new OrgFunding
            {
                FundingFactor = "FACTOR1",
                FundingFactorType = "FACTOR TYPE1",
                FundingFactorValue = "1.5",
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            var expectedOrgFunding = new OrganisationFunding()
            {
                OrgFundFactor = "FACTOR1",
                OrgFundFactType = "FACTOR TYPE1",
                OrgFundFactValue = "1.5",
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null
            };

            NewService().OrgFundingFromEntity(orgFunding).Should().BeEquivalentTo(expectedOrgFunding);
        }

        [Fact]
        public void OrgFundingFromEntity_Null()
        {
            NewService().OrgFundingFromEntity(null).Should().BeEquivalentTo(new OrganisationFunding());
        }

        private OrganisationsRepositoryService NewService(IOrganisationsContext organisations = null)
        {
            return new OrganisationsRepositoryService(organisations);
        }
    }
}
