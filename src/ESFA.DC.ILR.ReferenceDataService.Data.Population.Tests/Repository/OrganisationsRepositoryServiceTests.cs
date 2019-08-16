using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
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

            IEnumerable<MasterOrganisation> masterOrgList = new List<MasterOrganisation>
            {
                new MasterOrganisation
                {
                    Ukprn = 1,
                    OrgDetail = new OrgDetail
                    {
                        Ukprn = 1,
                        LegalOrgType = "LegalType1",
                        Name = "Name1"
                    },
                    OrgPartnerUkprns = new List<OrgPartnerUkprn>
                    {
                        new OrgPartnerUkprn
                        {
                            Ukprn = 1,
                            NameLegal = "NameLegal1",
                        },
                    },
                    OrgFundings = new List<OrgFunding>
                    {
                        new OrgFunding
                        {
                            FundingFactor = "FACTOR1",
                            FundingFactorType = "FACTOR TYPE1",
                            FundingFactorValue = "1.5",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                        new OrgFunding
                        {
                            FundingFactor = "FACTOR2",
                            FundingFactorType = "FACTOR TYPE1",
                            FundingFactorValue = "1.5",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                    },
                },
                new MasterOrganisation
                {
                    Ukprn = 2,
                    OrgDetail = new OrgDetail
                    {
                        Ukprn = 2,
                        LegalOrgType = "LegalType2",
                        Name = "Name2"
                    },
                },
            };

            IEnumerable<CampusIdentifier> campusIdentifiersList = new List<CampusIdentifier>
            {
                new CampusIdentifier
                {
                    MasterUkprn = 1,
                    CampusIdentifier1 = "CampId_01",
                    CampusIdentifierUkprn = new CampusIdentifierUkprn
                    {
                        CampusIdentifierSpecResources = new List<CampusIdentifierSpecResource>
                        {
                            new CampusIdentifierSpecResource
                            {
                                SpecialistResources = true
                            }
                        }
                    }
                },
            };

            IEnumerable<CampusIdentifierSpecResource> campusIdentifiersSpecResources = new List<CampusIdentifierSpecResource>
            {
                new CampusIdentifierSpecResource
                {
                    MasterUkprn = 1,
                    CampusIdentifier = "CampId_01",
                    SpecialistResources = true,
                    EffectiveFrom = new DateTime(2019, 8, 1)
                },
                new CampusIdentifierSpecResource
                {
                    MasterUkprn = 1,
                    CampusIdentifier = "CampId_02",
                    SpecialistResources = true,
                    EffectiveFrom = new DateTime(2019, 8, 1)
                },
            };

            var masterOrgMock = masterOrgList.AsQueryable().BuildMockDbSet();
            var campusIdentifiersMock = campusIdentifiersList.AsQueryable().BuildMockDbSet();
            var campusIdentifiersSpecResourcesMock = campusIdentifiersSpecResources.AsQueryable().BuildMockDbSet();

            var organisationsMock = new Mock<IOrganisationsContext>();

            organisationsMock.Setup(o => o.MasterOrganisations).Returns(masterOrgMock.Object);
            organisationsMock.Setup(o => o.CampusIdentifiers).Returns(campusIdentifiersMock.Object);
            organisationsMock.Setup(o => o.CampusIdentifierSpecResources).Returns(campusIdentifiersSpecResourcesMock.Object);

            var organisationsContextFactoryMock = new Mock<IDbContextFactory<IOrganisationsContext>>();
            organisationsContextFactoryMock.Setup(c => c.Create()).Returns(organisationsMock.Object);

            var organisations = await NewService(organisationsContextFactoryMock.Object).RetrieveAsync(ukprns, CancellationToken.None);

            organisations.Should().HaveCount(2);
            organisations.Select(o => o.UKPRN).Should().Contain(1);
            organisations.Select(o => o.UKPRN).Should().Contain(2);
            organisations.Select(o => o.UKPRN).Should().NotContain(3);

            organisations.Where(o => o.UKPRN == 1).Select(o => o.UKPRN).Should().BeEquivalentTo(1);
            organisations.Where(o => o.UKPRN == 1).SelectMany(o => o.LegalOrgType).Should().BeEquivalentTo("LegalType1");
            organisations.Where(o => o.UKPRN == 1).SelectMany(o => o.Name).Should().BeEquivalentTo("Name1");
            organisations.Where(o => o.UKPRN == 1).SelectMany(o => o.CampusIdentifers).Single().CampusIdentifier.Should().BeEquivalentTo("CampId_01");
            organisations.Where(o => o.UKPRN == 1).SelectMany(o => o.CampusIdentifers).Single().SpecialistResources.FirstOrDefault().IsSpecialistResource.Should().Be(true);
            organisations.Where(o => o.UKPRN == 1).Select(o => o.PartnerUKPRN).Should().BeEquivalentTo(true);
            organisations.Where(o => o.UKPRN == 1).SelectMany(o => o.OrganisationFundings).Should().HaveCount(2);

            organisations.Where(o => o.UKPRN == 2).Select(o => o.UKPRN).Should().BeEquivalentTo(2);
            organisations.Where(o => o.UKPRN == 2).SelectMany(o => o.LegalOrgType).Should().BeEquivalentTo("LegalType2");
            organisations.Where(o => o.UKPRN == 2).SelectMany(o => o.Name).Should().BeEquivalentTo("Name2");
            organisations.Where(o => o.UKPRN == 2).SelectMany(o => o.CampusIdentifers).Should().BeNullOrEmpty();
            organisations.Where(o => o.UKPRN == 2).Select(o => o.PartnerUKPRN).Should().BeEquivalentTo(false);
            organisations.Where(o => o.UKPRN == 2).SelectMany(o => o.OrganisationFundings).Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetCampusIdentifiers()
        {
            var campusListOne = new List<OrganisationCampusIdentifier>
            {
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId1"
                },
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId2"
                },
            };

            var campusListTwo = new List<OrganisationCampusIdentifier>
            {
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId3"
                },
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId4"
                },
            };

            var dictionary = new Dictionary<long, List<OrganisationCampusIdentifier>>
            {
                { 1, campusListOne },
                { 2, campusListTwo },
            };

            NewService().GetCampusIdentifiers(1, dictionary).Should().BeEquivalentTo(campusListOne);
        }

        [Fact]
        public void GetCampusIdentifiers_MisMatch()
        {
            var campusListOne = new List<OrganisationCampusIdentifier>
            {
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId1"
                },
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId2"
                },
            };

            var campusListTwo = new List<OrganisationCampusIdentifier>
            {
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId3"
                },
                new OrganisationCampusIdentifier
                {
                    CampusIdentifier = "CampisId4"
                },
            };

            var dictionary = new Dictionary<long, List<OrganisationCampusIdentifier>>
            {
                { 1, campusListOne },
                { 2, campusListTwo },
            };

            NewService().GetCampusIdentifiers(4, dictionary).Should().BeNullOrEmpty();
        }

        [Fact]
        public void GetCampusIdentifiers_Null()
        {
            NewService().GetCampusIdentifiers(1, new Dictionary<long, List<OrganisationCampusIdentifier>>()).Should().BeNullOrEmpty();
        }

        private OrganisationsRepositoryService NewService(IDbContextFactory<IOrganisationsContext> organisationsContextFactory = null)
        {
            return new OrganisationsRepositoryService(organisationsContextFactory);
        }
    }
}
