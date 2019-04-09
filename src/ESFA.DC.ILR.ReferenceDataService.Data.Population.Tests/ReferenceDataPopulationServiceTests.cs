using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests
{
    public class ReferenceDataPopulationServiceTests
    {
        [Fact]
        public async void PopulateAsync()
        {
            var message = new TestMessage();
            var cancellationToken = CancellationToken.None;

            var mapperData = new MapperData();

            var referenceDataVersions = TestReferenceDataVersions();
            var appsEarningHistories = TestAppsEarningHistories();
            var employers = TestEmployers();
            var epaOrgs = TestEpaOrgs();
            var fcsContractAllocations = TestFcs();
            var larsLearningDeliveries = TestLarsLearningDeliveries();
            var larsStandards = TestLarsStandards();
            var organisations = TestOrganisations();
            var postcodes = TestPostcodes();
            var ulns = TestUlnCollection();

            var messageMapperServiceMock = new Mock<IMessageMapperService>();
            var metaDataServiceMock = new Mock<IMetaDataRetrievalService>();
            var appsHistoryRSMock = new Mock<IAppEarningsHistoryRepositoryService>();
            var employersRSMock = new Mock<IEmployersRepositoryService>();
            var epaOrgRSMock = new Mock<IEpaOrganisationsRepositoryService>();
            var fcsRSMock = new Mock<IFcsRepositoryService>();
            var larsLearningDeliveryRSMock = new Mock<ILarsLearningDeliveryRepositoryService>();
            var larsStandardRSMock = new Mock<ILarsStandardRepositoryService>();
            var orgRSMock = new Mock<IOrganisationsRepositoryService>();
            var postcodesRSMock = new Mock<IPostcodesRepositoryService>();
            var ulnRSMock = new Mock<IUlnRepositoryService>();

            messageMapperServiceMock.Setup(s => s.MapFromMessage(message)).Returns(mapperData);
            metaDataServiceMock.Setup(s => s.RetrieveAsync(cancellationToken)).Returns(Task.FromResult(new MetaData { ReferenceDataVersions = referenceDataVersions }));
            appsHistoryRSMock.Setup(s => s.RetrieveAsync(mapperData.FM36Ulns, cancellationToken)).Returns(Task.FromResult(appsEarningHistories));
            employersRSMock.Setup(s => s.RetrieveAsync(mapperData.EmployerIds, cancellationToken)).Returns(Task.FromResult(employers));
            epaOrgRSMock.Setup(s => s.RetrieveAsync(mapperData.EpaOrgIds, cancellationToken)).Returns(Task.FromResult(epaOrgs));
            fcsRSMock.Setup(s => s.RetrieveAsync(mapperData.LearningProviderUKPRN, cancellationToken)).Returns(Task.FromResult(fcsContractAllocations));
            larsLearningDeliveryRSMock.Setup(s => s.RetrieveAsync(mapperData.LARSLearningDeliveryKeys, cancellationToken)).Returns(Task.FromResult(larsLearningDeliveries));
            larsStandardRSMock.Setup(s => s.RetrieveAsync(mapperData.StandardCodes, cancellationToken)).Returns(Task.FromResult(larsStandards));
            orgRSMock.Setup(s => s.RetrieveAsync(mapperData.UKPRNs, cancellationToken)).Returns(Task.FromResult(organisations));
            postcodesRSMock.Setup(s => s.RetrieveAsync(mapperData.Postcodes, cancellationToken)).Returns(Task.FromResult(postcodes));
            ulnRSMock.Setup(s => s.RetrieveAsync(mapperData.ULNs, cancellationToken)).Returns(Task.FromResult(ulns));

            var root = await NewService(
                messageMapperServiceMock.Object,
                metaDataServiceMock.Object,
                appsHistoryRSMock.Object,
                employersRSMock.Object,
                epaOrgRSMock.Object,
                fcsRSMock.Object,
                larsLearningDeliveryRSMock.Object,
                larsStandardRSMock.Object,
                orgRSMock.Object,
                postcodesRSMock.Object,
                ulnRSMock.Object).PopulateAsync(message, CancellationToken.None);

            root.MetaDatas.ReferenceDataVersions.Should().BeEquivalentTo(referenceDataVersions);
            root.AppsEarningsHistories.Should().HaveCount(2);
            root.Employers.Should().HaveCount(3);
            root.EPAOrganisations.Should().HaveCount(3);
            root.FCSContractAllocations.Should().HaveCount(2);
            root.LARSLearningDeliveries.Should().HaveCount(2);
            root.LARSStandards.Should().HaveCount(2);
            root.Organisations.Should().HaveCount(2);
            root.Postcodes.Should().HaveCount(2);
            root.ULNs.Should().HaveCount(5);
        }

        private ReferenceDataVersion TestReferenceDataVersions()
        {
            return new ReferenceDataVersion
            {
                Employers = new EmployersVersion("Version 1"),
                LarsVersion = new LarsVersion("Version 2"),
                OrganisationsVersion = new OrganisationsVersion("Version 3"),
                PostcodesVersion = new PostcodesVersion("Version 4")
            };
        }

        private IReadOnlyCollection<ApprenticeshipEarningsHistory> TestAppsEarningHistories()
        {
            return new List<ApprenticeshipEarningsHistory>
            {
                new ApprenticeshipEarningsHistory(),
                new ApprenticeshipEarningsHistory()
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
                        new LargeEmployerEffectiveDates()
                    }
                },
                new Employer
                {
                    ERN = 2,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates()
                    }
                },
                new Employer
                {
                    ERN = 3,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>()
                }
            };
        }

        private IReadOnlyCollection<EPAOrganisation> TestEpaOrgs()
        {
            return new List<EPAOrganisation>
            {
                new EPAOrganisation(),
                new EPAOrganisation(),
                new EPAOrganisation()
            };
        }

        private IReadOnlyCollection<FcsContractAllocation> TestFcs()
        {
            return new List<FcsContractAllocation>
            {
                new FcsContractAllocation(),
                new FcsContractAllocation()
            };
        }

        private IReadOnlyCollection<LARSLearningDelivery> TestLarsLearningDeliveries()
        {
            return new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery(),
                new LARSLearningDelivery()
            };
        }

        private IReadOnlyCollection<LARSStandard> TestLarsStandards()
        {
            return new List<LARSStandard>
            {
                new LARSStandard(),
                new LARSStandard()
            };
        }

        private IReadOnlyCollection<Organisation> TestOrganisations()
        {
            return new List<Organisation>
            {
                new Organisation(),
                new Organisation()
            };
        }

        private IReadOnlyCollection<Postcode> TestPostcodes()
        {
            return new List<Postcode>
            {
                new Postcode(),
                new Postcode()
            };
        }

        private IReadOnlyCollection<long> TestUlnCollection()
        {
            return new List<long>
            {
                1, 2, 3, 4, 5
            };
        }

        private ReferenceDataPopulationService NewService(
            IMessageMapperService messageMapperService = null,
            IMetaDataRetrievalService metaDataReferenceService = null,
            IAppEarningsHistoryRepositoryService appEarningsHistoryRepositoryService = null,
            IEmployersRepositoryService employersReferenceDataService = null,
            IEpaOrganisationsRepositoryService epaOrgReferenceDataService = null,
            IFcsRepositoryService fcsReferenceDataService = null,
            ILarsLearningDeliveryRepositoryService larsLearningDeliveryReferenceDataService = null,
            ILarsStandardRepositoryService larsStandardReferenceDataService = null,
            IOrganisationsRepositoryService orgReferenceDataService = null,
            IPostcodesRepositoryService postcodeReferenceDataService = null,
            IUlnRepositoryService ulnReferenceDataService = null)
        {
            return new ReferenceDataPopulationService(
                messageMapperService,
                metaDataReferenceService,
                appEarningsHistoryRepositoryService,
                employersReferenceDataService,
                epaOrgReferenceDataService,
                fcsReferenceDataService,
                larsLearningDeliveryReferenceDataService,
                larsStandardReferenceDataService,
                orgReferenceDataService,
                postcodeReferenceDataService,
                ulnReferenceDataService);
        }
    }
}
