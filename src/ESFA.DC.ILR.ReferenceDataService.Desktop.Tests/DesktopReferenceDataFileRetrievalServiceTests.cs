using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.Serialization.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class DesktopReferenceDataFileRetrievalServiceTests
    {
        [Fact]
        public async Task Retrieve()
        {
            var cancellationToken = CancellationToken.None;
            var expectedReferenceData = TestReferenceData();
            var fileServiceMock = new Mock<IFileService>();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();
            var referenceDataContext = new Mock<IReferenceDataContext>();

            var mapperData = new MapperData
            {
                Postcodes = new List<string> { "Postcode1" },
                EmployerIds = new List<int> { 1 },
                EpaOrgIds = new List<string> { "1", "2", "3" },
                UKPRNs = new List<int> { 1, 2, 3 },
                StandardCodes = new List<int> { 1, 2, 3 },
                LARSLearningDeliveryKeys = new List<LARSLearningDeliveryKey>
                {
                    new LARSLearningDeliveryKey("1", 1, 1, 1)
                }
            };

            var currentPath = Directory.GetCurrentDirectory();
            referenceDataContext.Setup(r => r.InputReferenceDataFileKey).Returns("ReferenceData.zip");
            referenceDataContext.Setup(r => r.Container).Returns(currentPath);

            jsonSerializationServiceMock.Setup(js => js.Deserialize<MetaData>(It.IsAny<Stream>())).Returns(TestNetaData());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<DevolvedPostcode>(It.IsAny<Stream>())).Returns(TestDevolvedPostcodes());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<McaGlaSofLookup>(It.IsAny<Stream>())).Returns(TestMcaSofLookups());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<Employer>(It.IsAny<Stream>())).Returns(TestEmployers());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<EPAOrganisation>(It.IsAny<Stream>())).Returns(TestEpaOrgs());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<LARSLearningDelivery>(It.IsAny<Stream>())).Returns(TestLarsLearningDeliveries());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<LARSFrameworkDesktop>(It.IsAny<Stream>())).Returns(TestLarsFrameworks());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<LARSStandard>(It.IsAny<Stream>())).Returns(TestLarsStandards());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<Organisation>(It.IsAny<Stream>())).Returns(TestOrganisations());
            jsonSerializationServiceMock.Setup(js => js.DeserializeCollection<Postcode>(It.IsAny<Stream>())).Returns(TestPostcodes());

            using (Stream stream = new FileStream(currentPath + "\\ReferenceData.zip", FileMode.Open))
            {
                fileServiceMock.Setup(fs => fs.OpenReadStreamAsync(
                   referenceDataContext.Object.InputReferenceDataFileKey,
                   referenceDataContext.Object.Container,
                   cancellationToken)).ReturnsAsync(stream);

                var result = await NewService(fileServiceMock.Object, jsonSerializationServiceMock.Object).Retrieve(referenceDataContext.Object, mapperData, cancellationToken);
                result.Should().BeEquivalentTo(expectedReferenceData);
            }
        }

        private DesktopReferenceDataRoot TestReferenceData()
        {
            return new DesktopReferenceDataRoot
            {
                MetaDatas = TestNetaData(),
                DevolvedPostocdes = new DevolvedPostcodes
                {
                    McaGlaSofLookups = TestMcaSofLookups(),
                    Postcodes = new List<DevolvedPostcode>
                    {
                        new DevolvedPostcode
                        {
                            Postcode = "Postcode1",
                            Area = "Area1",
                            SourceOfFunding = "105",
                            EffectiveFrom = new DateTime(2019, 9, 1)
                        }
                    },
                },
                Employers = new List<Employer>
                {
                    new Employer
                    {
                        ERN = 1,
                        LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                        {
                            new LargeEmployerEffectiveDates
                            {
                                EffectiveFrom = new DateTime(2018, 8, 1),
                            },
                        },
                    },
                },
                EPAOrganisations = new List<EPAOrganisation>
                {
                    new EPAOrganisation
                    {
                        ID = "1",
                        Standard = "1",
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new EPAOrganisation
                    {
                        ID = "2",
                        Standard = "1",
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new EPAOrganisation
                    {
                        ID = "3",
                        Standard = "1",
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                },
                LARSLearningDeliveries = new List<LARSLearningDelivery>
                {
                    new LARSLearningDelivery
                    {
                        LearnAimRef = "1",
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                },
                LARSFrameworks = TestLarsFrameworks(),
                LARSFrameworkAims = new List<LARSFrameworkAimDesktop>(),
                LARSStandards = new List<LARSStandard>
                {
                    new LARSStandard
                    {
                        StandardCode = 1,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSStandard
                    {
                        StandardCode = 2,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                    new LARSStandard
                    {
                        StandardCode = 3,
                        EffectiveFrom = new DateTime(2018, 8, 1),
                    },
                },
                Organisations = new List<Organisation>
                {
                    new Organisation
                    {
                        UKPRN = 1,
                        PartnerUKPRN = true,
                    },
                    new Organisation
                    {
                        UKPRN = 2,
                        PartnerUKPRN = true,
                    },
                    new Organisation
                    {
                        UKPRN = 3,
                        PartnerUKPRN = false,
                    },
                },
                Postcodes = new List<Postcode>
                {
                    new Postcode
                    {
                        PostCode = "Postcode1",
                    },
                },
                AppsEarningsHistories = new List<ApprenticeshipEarningsHistory>(),
                FCSContractAllocations = new List<FcsContractAllocation>(),
                ULNs = new List<long>(),
            };
        }

        private MetaData TestNetaData()
        {
            return new MetaData
            {
                ReferenceDataVersions = new ReferenceDataVersion()
                {
                    LarsVersion = new LarsVersion { Version = "LARS" },
                },
                Lookups = new List<Lookup>
                    {
                        new Lookup
                        {
                            Code = "1",
                            Name = "Name1",
                            SubCategories = new List<LookupSubCategory>
                            {
                                new LookupSubCategory
                                {
                                    Code = "1",
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                }
                            }
                        },
                        new Lookup
                        {
                            Code = "2",
                            Name = "Name2",
                        }
                    },
                ValidationErrors = new List<ValidationError>
                    {
                        new ValidationError
                        {
                            RuleName = "Rule1",
                            Severity = SeverityLevel.Error,
                            Message = "Message",
                        }
                    },
            };
        }

        private List<DevolvedPostcode> TestDevolvedPostcodes()
        {
            return new List<DevolvedPostcode>
            {
                new DevolvedPostcode
                {
                    Postcode = "Postcode1",
                    Area = "Area1",
                    SourceOfFunding = "105",
                    EffectiveFrom = new DateTime(2019, 9, 1)
                },
                new DevolvedPostcode
                {
                    Postcode = "Postcode2",
                    Area = "Area2",
                    SourceOfFunding = "105",
                    EffectiveFrom = new DateTime(2019, 9, 1)
                }
            };
        }

        private List<McaGlaSofLookup> TestMcaSofLookups()
        {
            return new List<McaGlaSofLookup>
            {
                new McaGlaSofLookup
                {
                    SofCode = "105",
                    McaGlaFullName = "Full Name",
                    McaGlaShortCode = "ShortCode",
                    EffectiveFrom = new DateTime(2019, 8, 1)
                }
            };
        }

        private List<Employer> TestEmployers()
        {
            return new List<Employer>
            {
                new Employer
                {
                    ERN = 1,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                    },
                },
                new Employer
                {
                    ERN = 10,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                    },
                },
                new Employer
                {
                    ERN = 2,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                    },
                },
                new Employer
                {
                    ERN = 3,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                    },
                },
            };
        }

        private List<EPAOrganisation> TestEpaOrgs()
        {
            return new List<EPAOrganisation>
            {
                new EPAOrganisation
                {
                    ID = "1",
                    Standard = "1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new EPAOrganisation
                {
                    ID = "2",
                    Standard = "1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new EPAOrganisation
                {
                    ID = "3",
                    Standard = "1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new EPAOrganisation
                {
                    ID = "33",
                    Standard = "1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new EPAOrganisation
                {
                    ID = "334",
                    Standard = "1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
            };
        }

        private List<LARSLearningDelivery> TestLarsLearningDeliveries()
        {
            return new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery
                {
                    LearnAimRef = "1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "3",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
            };
        }

        private List<LARSStandard> TestLarsStandards()
        {
            return new List<LARSStandard>
            {
                new LARSStandard
                {
                    StandardCode = 1,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSStandard
                {
                    StandardCode = 2,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSStandard
                {
                    StandardCode = 3,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSStandard
                {
                    StandardCode = 33,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSStandard
                {
                    StandardCode = 34,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
            };
        }


        private List<LARSFrameworkDesktop> TestLarsFrameworks()
        {
            return new List<LARSFrameworkDesktop>
            {
                new LARSFrameworkDesktop
                {
                    FworkCode = 1,
                    ProgType = 2,
                    PwayCode = 3,
                },
                new LARSFrameworkDesktop
                {
                    FworkCode = 2,
                    ProgType = 2,
                    PwayCode = 3,
                },
                new LARSFrameworkDesktop
                {
                    FworkCode = 2,
                    ProgType = 2,
                    PwayCode = 2,
                }
            };
        }

        private List<Organisation> TestOrganisations()
        {
            return new List<Organisation>
            {
                new Organisation
                {
                    UKPRN = 1,
                    PartnerUKPRN = true,
                },
                new Organisation
                {
                    UKPRN = 2,
                    PartnerUKPRN = true,
                },
                new Organisation
                {
                    UKPRN = 3,
                    PartnerUKPRN = false,
                },
                new Organisation
                {
                    UKPRN = 30,
                    PartnerUKPRN = false,
                },
                new Organisation
                {
                    UKPRN = 300,
                    PartnerUKPRN = false,
                },
            };
        }

        private List<Postcode> TestPostcodes()
        {
            return new List<Postcode>
            {
                new Postcode
                {
                    PostCode = "Postcode1",
                },
                new Postcode
                {
                    PostCode = "Postcode2",
                },
                new Postcode
                {
                    PostCode = "Postcode3",
                },
                new Postcode
                {
                    PostCode = "Postcode10",
                }
            };
        }

        private DesktopReferenceDataFileRetrievalService NewService(
            IFileService fileService = null,
            IJsonSerializationService jsonSerializationService = null)
        {
            return new DesktopReferenceDataFileRetrievalService(fileService, jsonSerializationService);
        }
    }
}
