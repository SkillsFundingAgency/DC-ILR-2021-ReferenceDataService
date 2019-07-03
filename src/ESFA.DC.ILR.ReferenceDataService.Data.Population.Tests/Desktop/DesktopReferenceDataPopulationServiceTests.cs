using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
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
            var employers = TestEmployers();
            var epaOrgs = TestEpaOrgs();
            var larsLearningDeliveries = TestLarsLearningDeliveries();
            var larsStandards = TestLarsStandards();
            var larsFrameworks = TestLarsFrameworks();
            var organisations = TestOrganisations();
            var postcodes = TestPostcodes();

            var metaDataServiceMock = new Mock<IMetaDataRetrievalService>();
            var employersRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>>>();
            var epaOrgRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>>>();
            var larsLearningDeliveryRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>>>();
            var larsStandardRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>>>();
            var larsFrameworkRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFramework>>>();
            var orgRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>>>();
            var postcodesRSMock = new Mock<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>>>();

            metaDataServiceMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(new MetaData { ReferenceDataVersions = referenceDataVersions }));
            employersRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(employers));
            epaOrgRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(epaOrgs));
            larsLearningDeliveryRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(larsLearningDeliveries));
            larsStandardRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(larsStandards));
            larsFrameworkRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(larsFrameworks));
            orgRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(organisations));
            postcodesRSMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(postcodes));

            var root = await NewService(
                metaDataServiceMock.Object,
                employersRSMock.Object,
                epaOrgRSMock.Object,
                larsLearningDeliveryRSMock.Object,
                larsStandardRSMock.Object,
                larsFrameworkRSMock.Object,
                orgRSMock.Object,
                postcodesRSMock.Object).PopulateAsync(CancellationToken.None);

            root.DateGenerated.Should().BeCloseTo(DateTime.UtcNow);
            root.MetaDatas.ReferenceDataVersions.Should().BeEquivalentTo(referenceDataVersions);
            root.AppsEarningsHistories.Should().HaveCount(0);
            root.Employers.Should().HaveCount(3);
            root.EPAOrganisations.Should().HaveCount(3);
            root.FCSContractAllocations.Should().HaveCount(0);
            root.LARSLearningDeliveries.Should().HaveCount(2);
            root.LARSStandards.Should().HaveCount(2);
            root.Organisations.Should().HaveCount(2);
            root.Postcodes.Should().HaveCount(2);
            root.ULNs.Should().HaveCount(0);
        }

        private ReferenceDataVersion TestReferenceDataVersions()
        {
            return new ReferenceDataVersion
            {
                Employers = new EmployersVersion("Version 1"),
                LarsVersion = new LarsVersion("Version 2"),
                OrganisationsVersion = new OrganisationsVersion("Version 3"),
                PostcodesVersion = new PostcodesVersion("Version 4"),
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

        private IReadOnlyCollection<LARSFramework> TestLarsFrameworks()
        {
            return new List<LARSFramework>
            {
                new LARSFramework(),
                new LARSFramework(),
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
            IMetaDataRetrievalService metaDataRetrievalService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>> employersRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>> epaOrganisationsRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>> larsStandardRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFramework>> larsFrameworkRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>> organisationsRepositoryService = null,
            IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>> postcodesRepositoryService = null)
        {
            return new DesktopReferenceDataPopulationService(
                metaDataRetrievalService,
                employersRepositoryService,
                epaOrganisationsRepositoryService,
                larsLearningDeliveryRepositoryService,
                larsStandardRepositoryService,
                larsFrameworkRepositoryService,
                organisationsRepositoryService,
                postcodesRepositoryService);
        }
    }
}
