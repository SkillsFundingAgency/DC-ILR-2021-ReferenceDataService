using System;
using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers;
using ESFA.DC.ILR.ReferenceDataService.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using FluentAssertions;
using Xunit;
using static ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ValidationError;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Tests
{
    public class MapperTests
    {
        [Fact]
        public void MetaDataReferenceDataMapper_Retrieve()
        {
            var expectedResult = new MetaData
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

            var referenceData = TestReferenceData();

            new MetaDataReferenceDataMapper().Retrieve(referenceData).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void EmployersReferenceDataMapper_Retrieve()
        {
            var empIds = new List<int> { 1, 2, 3, 4, 5 };
            var expectedResult = new List<Employer>
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

            var referenceData = TestReferenceData();

            new EmployersReferenceDataMapper().Retrieve(empIds, referenceData).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void EpaOrganisationsReferenceDataMapper_Retrieve()
        {
            var epaIds = new List<string> { "1", "2", "3", "4", "5" };
            var expectedResult = new List<EPAOrganisation>
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
            };

            var referenceData = TestReferenceData();

            new EpaOrganisationsReferenceDataMapper().Retrieve(epaIds, referenceData).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void LarsLearningDeliveryReferenceDataMapper_Retrieve()
        {
            var learnAimRefs = new List<LARSLearningDeliveryKey>
            {
                new LARSLearningDeliveryKey("1", 1, 2, 3),
                new LARSLearningDeliveryKey("2", 2, 2, 3),
                new LARSLearningDeliveryKey("3", 1, 2, 4),
                new LARSLearningDeliveryKey("4", 1, 2, 3),
            };

            var expectedResult = new List<LARSLearningDelivery>
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

            var referenceData = TestReferenceData();

            new LarsLearningDeliveryReferenceDataMapper().Retrieve(learnAimRefs, referenceData).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void LarsStandardReferenceDataMapper_Retrieve()
        {
            var standardCodes = new List<int> { 1, 2, 3, 4, 5 };

            var expectedResult = new List<LARSStandard>
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
            };

            var referenceData = TestReferenceData();

            new LarsStandardReferenceDataMapper().Retrieve(standardCodes, referenceData).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void OrganisationsReferenceDataMapper_Retrieve()
        {
            var ukprns = new List<int> { 1, 2, 3, 4, 5 };

            var expectedResult = new List<Organisation>
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
            };

            var referenceData = TestReferenceData();

            new OrganisationsReferenceDataMapper().Retrieve(ukprns, referenceData).Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void PostcodesReferenceDataMapper_Retrieve()
        {
            var postcodes = new List<string> { "Postcode1", "Postcode2", "Postcode3", "Postcode4", "Postcode5" };

            var expectedResult = new List<Postcode>
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
            };

            var referenceData = TestReferenceData();

            new PostcodesReferenceDataMapper().Retrieve(postcodes, referenceData).Should().BeEquivalentTo(expectedResult);
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
    }
}
