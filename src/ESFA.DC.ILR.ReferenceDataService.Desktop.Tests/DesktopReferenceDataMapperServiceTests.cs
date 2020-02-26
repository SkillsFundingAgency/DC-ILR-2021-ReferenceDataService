using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.Logging.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class DesktopReferenceDataMapperServiceTests
    {
        [Fact]
        public async Task Retrieve()
        {
            var cancellationToken = CancellationToken.None;
            var fileServiceMock = new Mock<IFileService>();
            var zipArchiveFileServiceMock = new Mock<IZipArchiveFileService>();
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

            var larsLearningDeliveries = new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery
                {
                    LearnAimRef = "1",
                    LARSFrameworks = new List<LARSFramework>
                    {
                        new LARSFramework
                        {
                            FworkCode = 1,
                            ProgType = 2,
                            PwayCode = 3,
                        }
                    },
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LARSFrameworks = new List<LARSFramework>
                    {
                        new LARSFramework
                        {
                            FworkCode = 2,
                            ProgType = 2,
                            PwayCode = 3,
                        }
                    }
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "3",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                },
            };

            var expectedReferenceData = new ReferenceDataRoot
            {
                MetaDatas = TestNetaData(),
                DevolvedPostocdes = new DevolvedPostcodes
                {
                    McaGlaSofLookups = TestMcaSofLookups(),
                    Postcodes = TestDevolvedPostcodes(),
                },
                Employers = TestEmployers(),
                EPAOrganisations = TestEpaOrgs(),
                LARSLearningDeliveries = larsLearningDeliveries,
                LARSStandards = TestLarsStandards(),
                Organisations = TestOrganisations(),
                Postcodes = TestPostcodes()
            };

            var currentPath = Directory.GetCurrentDirectory();
            referenceDataContext.Setup(r => r.InputReferenceDataFileKey).Returns("ReferenceData.zip");
            referenceDataContext.Setup(r => r.Container).Returns(currentPath);

            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModel<MetaData>(It.IsAny<ZipArchive>(), It.IsAny<string>())).Returns(TestNetaData());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<DevolvedPostcode>(It.IsAny<ZipArchive>(), It.IsAny<string>(), It.IsAny<Func<DevolvedPostcode, bool>>())).Returns(TestDevolvedPostcodes());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<McaGlaSofLookup>(It.IsAny<ZipArchive>(), It.IsAny<string>(), null)).Returns(TestMcaSofLookups());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<McaGlaSofLookup>(It.IsAny<ZipArchive>(), It.IsAny<string>(), null)).Returns(TestMcaSofLookups());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<Employer>(It.IsAny<ZipArchive>(), It.IsAny<string>(), It.IsAny<Func<Employer, bool>>())).Returns(TestEmployers());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<EPAOrganisation>(It.IsAny<ZipArchive>(), It.IsAny<string>(), It.IsAny<Func<EPAOrganisation, bool>>())).Returns(TestEpaOrgs());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<LARSLearningDelivery>(It.IsAny<ZipArchive>(), It.IsAny<string>(), It.IsAny<Func<LARSLearningDelivery, bool>>())).Returns(TestLarsLearningDeliveries());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<LARSFrameworkDesktop>(It.IsAny<ZipArchive>(), It.IsAny<string>(), null)).Returns(TestLarsFrameworks());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<LARSFrameworkAimDesktop>(It.IsAny<ZipArchive>(), It.IsAny<string>(), null)).Returns(new List<LARSFrameworkAimDesktop>());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<LARSStandard>(It.IsAny<ZipArchive>(), It.IsAny<string>(), It.IsAny<Func<LARSStandard, bool>>())).Returns(TestLarsStandards());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<Organisation>(It.IsAny<ZipArchive>(), It.IsAny<string>(), It.IsAny<Func<Organisation, bool>>())).Returns(TestOrganisations());
            zipArchiveFileServiceMock.Setup(zs => zs.RetrieveModels<Postcode>(It.IsAny<ZipArchive>(), It.IsAny<string>(), It.IsAny<Func<Postcode, bool>>())).Returns(TestPostcodes());

            var larsLearningDeliveryMapperServiceMock = new Mock<IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>>();

            larsLearningDeliveryMapperServiceMock.Setup(sm => sm.Map(It.IsAny<List<LARSLearningDeliveryKey>>(), It.IsAny<DesktopReferenceDataRoot>())).Returns(larsLearningDeliveries);

            using (Stream stream = new FileStream(currentPath + "\\TestFiles\\ReferenceData.zip", FileMode.Open))
            {
                fileServiceMock.Setup(fs => fs.OpenReadStreamAsync(
                   referenceDataContext.Object.InputReferenceDataFileKey,
                   referenceDataContext.Object.Container,
                   cancellationToken)).ReturnsAsync(stream);

                var result = await NewService(
                    zipArchiveFileServiceMock.Object,
                    larsLearningDeliveryMapperServiceMock.Object,
                    fileServiceMock.Object)
                    .MapReferenceData(referenceDataContext.Object, mapperData, cancellationToken);

                result.Should().BeEquivalentTo(expectedReferenceData);
            }
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

        private DesktopReferenceDataMapperService NewService(
            IZipArchiveFileService zipArchiveFileService = null,
            IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryMapperService = null,
            IFileService fileService = null)
        {
            return new DesktopReferenceDataMapperService(
                zipArchiveFileService,
                larsLearningDeliveryMapperService,
                fileService,
                Mock.Of<ILogger>());
        }
    }
}