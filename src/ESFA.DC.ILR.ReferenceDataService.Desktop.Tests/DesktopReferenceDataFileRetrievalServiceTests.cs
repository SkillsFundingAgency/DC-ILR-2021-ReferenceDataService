using System;
using System.Collections.Generic;
using System.IO.Compression;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
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

            jsonSerializationServiceMock.Setup(js => js.Deserialize<DesktopReferenceDataRoot>(It.IsAny<GZipStream>())).Returns(expectedReferenceData);

            NewService(jsonSerializationServiceMock.Object).Retrieve().Should().BeEquivalentTo(expectedReferenceData);
        }


        private DesktopReferenceDataRoot TestReferenceData()
        {
            return new DesktopReferenceDataRoot
            {
                MetaDatas = new MetaData
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
                },
                LARSLearningDeliveries = new List<LARSLearningDelivery>
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
                },
                LARSFrameworks = new List<LARSFramework>
                {
                    new LARSFramework
                    {
                        FworkCode = 1,
                        ProgType = 2,
                        PwayCode = 3,
                    },
                    new LARSFramework
                    {
                        FworkCode = 2,
                        ProgType = 2,
                        PwayCode = 3,
                    },
                    new LARSFramework
                    {
                        FworkCode = 2,
                        ProgType = 2,
                        PwayCode = 2,
                    },
                },
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
                },
                Postcodes = new List<Postcode>
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
                }
            };
        }


        private DesktopReferenceDataFileRetrievalService NewService(IJsonSerializationService jsonSerializationService = null)
        {
            return new DesktopReferenceDataFileRetrievalService(jsonSerializationService);
        }
    }
}
