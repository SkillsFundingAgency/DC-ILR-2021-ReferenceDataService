using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests
{
    public class DesktopReferenceDataPopulationServiceTests
    {
        [Fact]
        public async void PopulateAsync()
        {
            var cancellationToken = CancellationToken.None;

            var referenceDataVersions = TestReferenceDataVersions();
            var appsEarningHistories = TestAppsEarningHistories();
            var devolvedPostcodes = TestDevolvedPostcodes();
            var employers = TestEmployers();
            var epaOrgs = TestEpaOrgs();
            var larsLearningDeliveries = TestLarsLearningDeliveries();
            var larsStandards = TestLarsStandards();
            var larsFrameworks = TestLarsFrameworks();
            var larsFrameworkAims = TestLarsFrameworkAims();
            var organisations = TestOrganisations();
            var postcodes = TestPostcodes();

            var metaDataServiceMock = new Mock<IDesktopMetaDataRetrievalService>();
            var devolvedPostcodesRSMock = new Mock<IDesktopReferenceDataRepositoryService<DevolvedPostcodes>>();
            var employersRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>>>();
            var epaOrgRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>>>();
            var larsLearningDeliveryRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>>>();
            var larsStandardRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>>>();
            var larsFrameworkRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkDesktop>>>();
            var larsFrameworkAimsRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkAimDesktop>>>();
            var orgRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>>>();
            var postcodesRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>>>();

            metaDataServiceMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(new MetaData { ReferenceDataVersions = referenceDataVersions }));
            devolvedPostcodesRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(devolvedPostcodes));
            employersRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(employers));
            epaOrgRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(epaOrgs));
            larsLearningDeliveryRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(larsLearningDeliveries));
            larsStandardRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(larsStandards));
            larsFrameworkRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(larsFrameworks));
            larsFrameworkAimsRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(larsFrameworkAims));
            orgRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(organisations));
            postcodesRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(postcodes));

            var root = await NewService(
                metaDataServiceMock.Object,
                devolvedPostcodesRSMock.Object,
                employersRSMock.Object,
                epaOrgRSMock.Object,
                larsLearningDeliveryRSMock.Object,
                larsStandardRSMock.Object,
                larsFrameworkRSMock.Object,
                larsFrameworkAimsRSMock.Object,
                orgRSMock.Object,
                postcodesRSMock.Object).PopulateAsync(CancellationToken.None);

            root.MetaDatas.ReferenceDataVersions.Should().BeEquivalentTo(referenceDataVersions);
            root.AppsEarningsHistories.Should().HaveCount(0);
            root.DevolvedPostocdes.Postcodes.Should().HaveCount(2);
            root.Employers.Should().HaveCount(3);
            root.EPAOrganisations.Should().HaveCount(3);
            root.FCSContractAllocations.Should().HaveCount(0);
            root.LARSLearningDeliveries.Should().HaveCount(2);
            root.LARSFrameworks.Should().HaveCount(2);
            root.LARSFrameworkAims.Should().HaveCount(2);
            root.LARSStandards.Should().HaveCount(2);
            root.Organisations.Should().HaveCount(2);
            root.Postcodes.Should().HaveCount(2);
            root.ULNs.Should().HaveCount(0);
        }

        private ReferenceDataVersion TestReferenceDataVersions()
        {
            return new ReferenceDataVersion
            {
                Employers = new EmployersVersion { Version = "Version 1" },
                LarsVersion = new LarsVersion { Version = "Version 2" },
                OrganisationsVersion = new OrganisationsVersion { Version = "Version 3" },
                PostcodesVersion = new PostcodesVersion { Version = "Version 4" }
            };
        }

        private IReadOnlyCollection<ApprenticeshipEarningsHistory> TestAppsEarningHistories()
        {
            return new List<ApprenticeshipEarningsHistory>
            {
                new ApprenticeshipEarningsHistory(),
                new ApprenticeshipEarningsHistory(),
            };
        }

        private DevolvedPostcodes TestDevolvedPostcodes()
        {
            return new DevolvedPostcodes
            {
                McaGlaSofLookups = new List<McaGlaSofLookup>
                {
                    new McaGlaSofLookup
                    {
                        SofCode = "105",
                        McaGlaFullName = "Full Name",
                        McaGlaShortCode = "ShortCode",
                        EffectiveFrom = new DateTime(2019, 8, 1)
                    }
                },
                Postcodes = new List<DevolvedPostcode>
                {
                    new DevolvedPostcode(),
                    new DevolvedPostcode()
                }
            };
        }

        private IReadOnlyCollection<Employer> TestEmployers()
        {
            return new List<Employer>
            {
                new Employer
                {
                    ERN = 1,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates(),
                        new LargeEmployerEffectiveDates(),
                    },
                },
                new Employer
                {
                    ERN = 2,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates(),
                    },
                },
                new Employer
                {
                    ERN = 3,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>(),
                },
            };
        }

        private IReadOnlyCollection<EPAOrganisation> TestEpaOrgs()
        {
            return new List<EPAOrganisation>
            {
                new EPAOrganisation(),
                new EPAOrganisation(),
                new EPAOrganisation(),
            };
        }

        private IReadOnlyCollection<LARSLearningDelivery> TestLarsLearningDeliveries()
        {
            return new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery(),
                new LARSLearningDelivery(),
            };
        }

        private IReadOnlyCollection<LARSStandard> TestLarsStandards()
        {
            return new List<LARSStandard>
            {
                new LARSStandard(),
                new LARSStandard(),
            };
        }

        private IReadOnlyCollection<LARSFrameworkDesktop> TestLarsFrameworks()
        {
            return new List<LARSFrameworkDesktop>
            {
                new LARSFrameworkDesktop(),
                new LARSFrameworkDesktop(),
            };
        }

        private IReadOnlyCollection<LARSFrameworkAimDesktop> TestLarsFrameworkAims()
        {
            return new List<LARSFrameworkAimDesktop>
            {
                new LARSFrameworkAimDesktop(),
                new LARSFrameworkAimDesktop(),
            };
        }

        private IReadOnlyCollection<Organisation> TestOrganisations()
        {
            return new List<Organisation>
            {
                new Organisation(),
                new Organisation(),
            };
        }

        private IReadOnlyCollection<Postcode> TestPostcodes()
        {
            return new List<Postcode>
            {
                new Postcode(),
                new Postcode(),
            };
        }

        private DesktopReferenceDataPopulationService NewService(
            IDesktopMetaDataRetrievalService metaDataRetrievalService = null,
            IDesktopReferenceDataRepositoryService<DevolvedPostcodes> devolvedPostcodesRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>> employersRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>> epaOrganisationsRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>> larsStandardRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkDesktop>> larsFrameworkRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkAimDesktop>> larsFrameworkAimsRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>> organisationsRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>> postcodesRepositoryService = null)
        {
            return new DesktopReferenceDataPopulationService(
                metaDataRetrievalService,
                devolvedPostcodesRepositoryService,
                employersRepositoryService,
                epaOrganisationsRepositoryService,
                larsLearningDeliveryRepositoryService,
                larsStandardRepositoryService,
                larsFrameworkRepositoryService,
                larsFrameworkAimsRepositoryService,
                organisationsRepositoryService,
                postcodesRepositoryService);
        }
    }
}
