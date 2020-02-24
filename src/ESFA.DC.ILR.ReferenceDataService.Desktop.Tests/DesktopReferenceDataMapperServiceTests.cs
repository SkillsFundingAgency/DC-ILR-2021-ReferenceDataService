﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
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
using ESFA.DC.ILR.Tests.Model;
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
        public async void PopulateAsync()
        {
            IMessage message = new TestMessage();
            var mapperData = new MapperData();

            var desktopReferenceData = TestReferenceData();
            var metaData = new MetaData
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

            var devolvedPostcodes = new DevolvedPostcodes
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
                }
            };

            var employers = new List<Employer>
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

            var epaOrgs = new List<EPAOrganisation>
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

            var larsStandards = new List<LARSStandard>
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

            var organisations = new List<Organisation>
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

            var postcodes = new List<Postcode>
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

            var expectedReferenceDataRoot = new ReferenceDataRoot
            {
                MetaDatas = metaData,
                DevolvedPostocdes = devolvedPostcodes,
                Employers = employers,
                EPAOrganisations = epaOrgs,
                LARSLearningDeliveries = larsLearningDeliveries,
                LARSStandards = larsStandards,
                Organisations = organisations,
                Postcodes = postcodes,
            };

            var referenceDataContext = new Mock<IReferenceDataContext>();
            var messageMapperServiceMock = new Mock<IMessageMapperService>();
            var desktopReferenceDataFileRetrievalServiceMock = new Mock<IDesktopReferenceDataFileRetrievalService>();
     
            var larsLearningDeliveryMapperServiceMock = new Mock<IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>>();

            messageMapperServiceMock.Setup(sm => sm.MapFromMessage(message)).Returns(mapperData);
            desktopReferenceDataFileRetrievalServiceMock.Setup(sm => sm.Retrieve(referenceDataContext.Object, It.IsAny<MapperData>(), CancellationToken.None)).Returns(Task.FromResult(desktopReferenceData)); 
            larsLearningDeliveryMapperServiceMock.Setup(sm => sm.Retrieve(It.IsAny<List<LARSLearningDeliveryKey>>(), desktopReferenceData)).Returns(larsLearningDeliveries);
           

            NewService(
                messageMapperServiceMock.Object,
                desktopReferenceDataFileRetrievalServiceMock.Object,
                larsLearningDeliveryMapperServiceMock.Object).PopulateAsync(referenceDataContext.Object, message, CancellationToken.None).Result.Should().BeEquivalentTo(expectedReferenceDataRoot);
        }

        private DesktopReferenceDataRoot TestReferenceData()
        {
            return new DesktopReferenceDataRoot
            {
                MetaDatas = new MetaData
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
                },
                DevolvedPostocdes = new DevolvedPostcodes
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
                    }
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
                LARSFrameworks = new List<LARSFrameworkDesktop>
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
                    new Postcode
                    {
                        PostCode = "Postcode2",
                    },
                    new Postcode
                    {
                        PostCode = "Postcode3",
                    },
                }
            };
        }

        private DesktopReferenceDataMapperService NewService(
            IMessageMapperService messageMapperService = null,
            IDesktopReferenceDataFileRetrievalService desktopReferenceDataFileRetrievalService = null,
            IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>> larsLearningDeliveryMapperService = null)
        {
            return new DesktopReferenceDataMapperService(
                messageMapperService,
                desktopReferenceDataFileRetrievalService,
                larsLearningDeliveryMapperService,
                Mock.Of<ILogger>());
        }
    }
}
