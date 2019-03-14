using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.ULNs;
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

            var referenceDataVersions = TestReferenceDataVersions();
            var employers = TestEmployerDictionary();
            var epaOrgs = TestEpaOrgDictionary();
            var fcsContractAllocations = TestFcsDictionary();
            var larsLearningDeliveries = TestLarsLearningDeliveryDictionary();
            var larsStandards = TestLarsStandardDictionary();
            var organisations = TestOrganisationDictionary();
            var postcodes = TestPostcodeDictionary();
            var ulns = TestUlnCollection();

            var metaDataServiceMock = new Mock<IReferenceMetaDataService>();
            var employersRDSMock = new Mock<IReferenceDataService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>>>();
            var epaOrgRDSMock = new Mock<IReferenceDataService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>>>();
            var fcsRDSMock = new Mock<IReferenceDataService<IReadOnlyDictionary<string, FcsContractAllocation>, int>>();
            var larsLearningDeliveryRDSMock = new Mock<IReferenceDataService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>>>();
            var larsStandardRDSMock = new Mock<IReferenceDataService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>>>();
            var orgRDSMock = new Mock<IReferenceDataService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>>>();
            var postcodesRDSMock = new Mock<IReferenceDataService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>>>();
            var ulnRDSMock = new Mock<IReferenceDataService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>>();

            metaDataServiceMock.Setup(s => s.Retrieve(cancellationToken)).Returns(Task.FromResult(new MetaData { ReferenceDataVersions = referenceDataVersions }));
            employersRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(employers));
            epaOrgRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(epaOrgs));
            fcsRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(fcsContractAllocations));
            larsLearningDeliveryRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(larsLearningDeliveries));
            larsStandardRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(larsStandards));
            orgRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(organisations));
            postcodesRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(postcodes));
            ulnRDSMock.Setup(s => s.Retrieve(message, cancellationToken)).Returns(Task.FromResult(ulns));

            var root = await NewService(
                metaDataServiceMock.Object,
                employersRDSMock.Object,
                epaOrgRDSMock.Object,
                fcsRDSMock.Object,
                larsLearningDeliveryRDSMock.Object,
                larsStandardRDSMock.Object,
                orgRDSMock.Object,
                postcodesRDSMock.Object,
                ulnRDSMock.Object).PopulateAsync(message, CancellationToken.None);

            root.MetaDatas.ReferenceDataVersions.Should().BeEquivalentTo(referenceDataVersions);
            root.MetaDatas.ReferenceDataVersions.Should().BeEquivalentTo(referenceDataVersions);

            root.Employers.Keys.Should().HaveCount(3);
            root.Employers.Keys.Should().Contain(employers.Keys);
            root.Employers.Keys.Should().NotContain(4);
            root.Employers[1].LargeEmployerEffectiveDates.Should().HaveCount(2);
            root.Employers[2].LargeEmployerEffectiveDates.Should().HaveCount(1);
            root.Employers[3].LargeEmployerEffectiveDates.Should().HaveCount(0);

            root.EPAOrganisations.Keys.Should().HaveCount(2);
            root.EPAOrganisations.Keys.Should().Contain(epaOrgs.Keys);
            root.EPAOrganisations.Keys.Should().NotContain("Org3");
            root.EPAOrganisations["Org1"].Should().HaveCount(2);
            root.EPAOrganisations["Org2"].Should().HaveCount(1);

            root.FCSContractAllocations.Keys.Should().HaveCount(2);
            root.FCSContractAllocations.Keys.Should().Contain(fcsContractAllocations.Keys);
            root.FCSContractAllocations.Keys.Should().NotContain("Org3");
            root.FCSContractAllocations["ConRef1"].Should().BeEquivalentTo(fcsContractAllocations["ConRef1"]);
            root.FCSContractAllocations["ConRef2"].Should().BeEquivalentTo(fcsContractAllocations["ConRef2"]);

            root.LARSLearningDeliveries.Keys.Should().HaveCount(2);
            root.LARSLearningDeliveries.Keys.Should().Contain(larsLearningDeliveries.Keys);
            root.LARSLearningDeliveries.Keys.Should().NotContain("LearnAimRef3");
            root.LARSLearningDeliveries["LearnAimRef1"].Should().BeEquivalentTo(larsLearningDeliveries["LearnAimRef1"]);
            root.LARSLearningDeliveries["LearnAimRef2"].Should().BeEquivalentTo(larsLearningDeliveries["LearnAimRef2"]);

            root.LARSStandards.Keys.Should().HaveCount(2);
            root.LARSStandards.Keys.Should().Contain(larsStandards.Keys);
            root.LARSStandards.Keys.Should().NotContain(3);
            root.LARSStandards[1].Should().BeEquivalentTo(larsStandards[1]);
            root.LARSStandards[2].Should().BeEquivalentTo(larsStandards[2]);

            root.Organisations.Keys.Should().HaveCount(2);
            root.Organisations.Keys.Should().Contain(organisations.Keys);
            root.Organisations.Keys.Should().NotContain(3);
            root.Organisations[1].Should().BeEquivalentTo(organisations[1]);
            root.Organisations[2].Should().BeEquivalentTo(organisations[2]);

            root.Postcodes.Keys.Should().HaveCount(2);
            root.Postcodes.Keys.Should().Contain(postcodes.Keys);
            root.Postcodes.Keys.Should().NotContain("Postcode3");
            root.Postcodes["Postcode1"].Should().BeEquivalentTo(postcodes["Postcode1"]);
            root.Postcodes["Postcode2"].Should().BeEquivalentTo(postcodes["Postcode2"]);

            root.ULNs.Should().HaveCount(5);
            root.ULNs.Should().BeEquivalentTo(ulns);
        }

        private List<ReferenceDataVersion> TestReferenceDataVersions()
        {
            return new List<ReferenceDataVersion>
            {
                new ReferenceDataVersion("Employers", "Version 1"),
                new ReferenceDataVersion("LARS", "Version 2"),
                new ReferenceDataVersion("Organisations", "Version 3"),
                new ReferenceDataVersion("Postcodes", "Version 4")
            };
        }

        private IReadOnlyDictionary<int, Employer> TestEmployerDictionary()
        {
            return new Dictionary<int, Employer>
            {
                {
                    1,
                    new Employer
                    {
                        ERN = 1,
                        LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                        {
                            new LargeEmployerEffectiveDates(),
                            new LargeEmployerEffectiveDates()
                        }
                    }
                },
                {
                    2,
                    new Employer
                    {
                        ERN = 2,
                        LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                        {
                            new LargeEmployerEffectiveDates()
                        }
                    }
                },
                 {
                    3,
                    new Employer
                    {
                        ERN = 3,
                        LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>()
                    }
                }
            };
        }

        private IReadOnlyDictionary<string, List<EPAOrganisation>> TestEpaOrgDictionary()
        {
            return new Dictionary<string, List<EPAOrganisation>>
            {
                {
                    "Org1",
                    new List<EPAOrganisation>
                    {
                        new EPAOrganisation(),
                        new EPAOrganisation()
                    }
                },
                {
                    "Org2",
                    new List<EPAOrganisation>
                    {
                        new EPAOrganisation()
                    }
                }
            };
        }

        private IReadOnlyDictionary<string, FcsContractAllocation> TestFcsDictionary()
        {
            return new Dictionary<string, FcsContractAllocation>
            {
                {
                    "ConRef1",
                    new FcsContractAllocation()
                },
                {
                    "ConRef2",
                    new FcsContractAllocation()
                }
            };
        }

        private IReadOnlyDictionary<string, LARSLearningDelivery> TestLarsLearningDeliveryDictionary()
        {
            return new Dictionary<string, LARSLearningDelivery>
            {
                {
                    "LearnAimRef1",
                    new LARSLearningDelivery()
                },
                {
                    "LearnAimRef2",
                    new LARSLearningDelivery()
                }
            };
        }

        private IReadOnlyDictionary<int, LARSStandard> TestLarsStandardDictionary()
        {
            return new Dictionary<int, LARSStandard>
            {
                {
                    1,
                    new LARSStandard()
                },
                {
                    2,
                    new LARSStandard()
                }
            };
        }

        private IReadOnlyDictionary<int, Organisation> TestOrganisationDictionary()
        {
            return new Dictionary<int, Organisation>
            {
                {
                    1,
                    new Organisation()
                },
                {
                    2,
                    new Organisation()
                }
            };
        }

        private IReadOnlyDictionary<string, Postcode> TestPostcodeDictionary()
        {
            return new Dictionary<string, Postcode>
            {
                {
                    "Postcode1",
                    new Postcode()
                },
                {
                    "Postcode2",
                    new Postcode()
                }
            };
        }

        private IReadOnlyCollection<ULN> TestUlnCollection()
        {
            return new List<ULN>
            {
                new ULN { UniqueLearnerNumber = 1 },
                new ULN { UniqueLearnerNumber = 2 },
                new ULN { UniqueLearnerNumber = 3 },
                new ULN { UniqueLearnerNumber = 4 },
                new ULN { UniqueLearnerNumber = 5 }
            };
        }

        private ReferenceDataPopulationService NewService(
            IReferenceMetaDataService metaDataReferenceService = null,
            IReferenceDataService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>> employersReferenceDataService = null,
            IReferenceDataService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>> epaOrgReferenceDataService = null,
            IReferenceDataService<IReadOnlyDictionary<string, FcsContractAllocation>, int> fcsReferenceDataService = null,
            IReferenceDataService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>> larsLearningDeliveryReferenceDataService = null,
            IReferenceDataService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>> larsStandardReferenceDataService = null,
            IReferenceDataService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>> orgReferenceDataService = null,
            IReferenceDataService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>> postcodeReferenceDataService = null,
            IReferenceDataService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>> ulnReferenceDataService = null)
        {
            return new ReferenceDataPopulationService(
                metaDataReferenceService,
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
