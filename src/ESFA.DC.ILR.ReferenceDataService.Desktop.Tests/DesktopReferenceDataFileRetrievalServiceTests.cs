using System;
using System.Collections.Generic;
using System.IO;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
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
        public void Retrieve()
        {
            var expectedReferenceData = TestReferenceData();
            var jsonSerializationServiceMock = new Mock<IJsonSerializationService>();

            jsonSerializationServiceMock.Setup(js => js.Deserialize<MetaData>(It.IsAny<Stream>())).Returns(TestNetaData());
            jsonSerializationServiceMock.Setup(js => js.Deserialize<List<Employer>>(It.IsAny<Stream>())).Returns(TestEmployers());
            jsonSerializationServiceMock.Setup(js => js.Deserialize<List<EPAOrganisation>>(It.IsAny<Stream>())).Returns(TestEpaOrgs());
            jsonSerializationServiceMock.Setup(js => js.Deserialize<List<LARSLearningDelivery>>(It.IsAny<Stream>())).Returns(TestLarsLearningDeliveries());
            jsonSerializationServiceMock.Setup(js => js.Deserialize<List<LARSFrameworkDesktop>>(It.IsAny<Stream>())).Returns(TestLarsFrameworks());
            jsonSerializationServiceMock.Setup(js => js.Deserialize<List<LARSStandard>>(It.IsAny<Stream>())).Returns(TestLarsStandards());
            jsonSerializationServiceMock.Setup(js => js.Deserialize<List<Organisation>>(It.IsAny<Stream>())).Returns(TestOrganisations());
            jsonSerializationServiceMock.Setup(js => js.Deserialize<List<Postcode>>(It.IsAny<Stream>())).Returns(TestPostcodes());

            NewService(jsonSerializationServiceMock.Object).Retrieve().Should().BeEquivalentTo(expectedReferenceData);
        }

        private DesktopReferenceDataRoot TestReferenceData()
        {
            return new DesktopReferenceDataRoot
            {
                MetaDatas = TestNetaData(),
                Employers = TestEmployers(),
                EPAOrganisations = TestEpaOrgs(),
                LARSLearningDeliveries = TestLarsLearningDeliveries(),
                LARSFrameworks = TestLarsFrameworks(),
                LARSStandards = TestLarsStandards(),
                Organisations = TestOrganisations(),
                Postcodes = TestPostcodes(),
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
                    LarsVersion = new LarsVersion("LARS"),
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

        private DesktopReferenceDataFileRetrievalService NewService(IJsonSerializationService jsonSerializationService = null)
        {
            return new DesktopReferenceDataFileRetrievalService(jsonSerializationService);
        }
    }
}
