﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Tests.Repository
{
    public class LarsLearningDeliveryRepositoryServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var expectedLearningDeliveries = ExpectedLARSLearningDeliveries();

            var larsLearningDeliveryKeys = new List<LARSLearningDeliveryKey>
            {
                new LARSLearningDeliveryKey("LEARNAIMREF1", 1, 2, 3),
                new LARSLearningDeliveryKey("LEARNAIMREF2", 1, 2, 3),
            };

            IEnumerable<LarsLearningDelivery> larsLearningDeliveryList = new List<LarsLearningDelivery>
            {
                new LarsLearningDelivery
                {
                    LearnAimRef = "LearnAimRef1",
                    LearnAimRefTitle = "AimRefTitle1",
                    LearnAimRefTypeNavigation = new LarsLearnAimRefTypeLookup
                    {
                        LearnAimRefTypeDesc = "LearnAimRefTypeDesc1"
                    },
                    SectorSubjectAreaTier2 = 1.1m
                },
                new LarsLearningDelivery
                {
                    LearnAimRef = "LearnAimRef2",
                    LearnAimRefTitle = "AimRefTitle2",
                    LearnAimRefTypeNavigation = new LarsLearnAimRefTypeLookup
                    {
                        LearnAimRefTypeDesc = "LearnAimRefTypeDesc2"
                    },
                    SectorSubjectAreaTier2 = 2.1m
                }
            };

            IEnumerable<LarsAnnualValue> larsAnnualValueList = new List<LarsAnnualValue>
            {
                new LarsAnnualValue
                {
                    LearnAimRef = "LearnAimRef2",
                    BasicSkills = 1,
                    BasicSkillsType = 2,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = null,
                    FullLevel2EntitlementCategory = 3,
                    FullLevel3EntitlementCategory = 4,
                    FullLevel2Percent = 5,
                    FullLevel3Percent = 6,
                    CreatedBy = "CreatedBy",
                },
            };

            IEnumerable<LarsLearningDeliveryCategory> larsLearningDeliveryCategoryList = new List<LarsLearningDeliveryCategory>
            {
                new LarsLearningDeliveryCategory
                {
                    LearnAimRef = "LearnAimRef2",
                    CategoryRef = 1,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = null,
                    CreatedBy = "CreatedBy",
                },
                new LarsLearningDeliveryCategory
                {
                    LearnAimRef = "LearnAimRef2",
                    CategoryRef = 2,
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = null,
                    CreatedBy = "CreatedBy",
                },
            };

            IEnumerable<LarsFunding> larsFundingsList = new List<LarsFunding>
            {
                new LarsFunding
                {
                    LearnAimRef = "LearnAimRef1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = null,
                    FundingCategory = "Cat1",
                    RateUnWeighted = 1.0m,
                    RateWeighted = 2.0m,
                    WeightingFactor = "Factor",
                    CreatedBy = "CreatedBy",
                },
                new LarsFunding
                {
                    LearnAimRef = "LearnAimRef1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = null,
                    FundingCategory = "Cat2",
                    RateUnWeighted = 1.0m,
                    RateWeighted = 2.0m,
                    WeightingFactor = "Factor",
                    CreatedBy = "CreatedBy",
                },
                new LarsFunding
                {
                    LearnAimRef = "LearnAimRef1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    EffectiveTo = null,
                    FundingCategory = "Cat3",
                    RateUnWeighted = 1.0m,
                    RateWeighted = 2.0m,
                    WeightingFactor = "Factor",
                    CreatedBy = "CreatedBy",
                },
            };

            IEnumerable<LarsValidity> larsValiditiesList = new List<LarsValidity>
            {
                new LarsValidity
                {
                    LearnAimRef = "LearnAimRef1",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = null,
                    LastNewStartDate = new DateTime(2018, 8, 1),
                    ValidityCategory = "Cat1",
                    CreatedBy = "CreatedBy",
                },
                new LarsValidity
                {
                    LearnAimRef = "LearnAimRef1",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = null,
                    LastNewStartDate = new DateTime(2018, 8, 1),
                    ValidityCategory = "Cat2",
                    CreatedBy = "CreatedBy",
                },
            };

            IEnumerable<LarsFrameworkAim> larsFrameworkAimsList = new List<LarsFrameworkAim>
            {
                new LarsFrameworkAim
                {
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    FworkCode = 1,
                    ProgType = 2,
                    PwayCode = 3,
                    FrameworkComponentType = 1,
                    LearnAimRef = "LearnAimRef1",
                },
            };

            IEnumerable<LarsFramework> larsFrameworkList = new List<LarsFramework>
            {
                new LarsFramework
                {
                   FworkCode = 1,
                   ProgType = 2,
                   PwayCode = 3,
                   EffectiveFrom = new DateTime(2018, 8, 1),
                   LarsFrameworkCmnComps = new List<LarsFrameworkCmnComp>
                   {
                       new LarsFrameworkCmnComp
                       {
                           FworkCode = 1,
                           ProgType = 2,
                           PwayCode = 3,
                           CommonComponent = 4,
                           EffectiveFrom = new DateTime(2018, 8, 1),
                       },
                   },
                   LarsApprenticeshipFworkFundings = new List<LarsApprenticeshipFworkFunding>
                   {
                       new LarsApprenticeshipFworkFunding
                       {
                           FworkCode = 1,
                           ProgType = 2,
                           PwayCode = 3,
                           BandNumber = 1,
                           CareLeaverAdditionalPayment = 2.0m,
                           CoreGovContributionCap = 3.0m,
                           Duration = 4,
                           EffectiveFrom = new DateTime(2018, 8, 1),
                           EffectiveTo = null,
                           FundableWithoutEmployer = "5",
                           FundingCategory = "6",
                           MaxEmployerLevyCap = 7.0m,
                           ReservedValue2 = 8.0m,
                           ReservedValue3 = 9.0m,
                           _1618employerAdditionalPayment = 10.0m,
                           _1618frameworkUplift = 11.0m,
                           _1618incentive = 12.0m,
                           _1618providerAdditionalPayment = 13.0m,
                           CreatedBy = "CreatedBy",
                       },
                   },
                },
            };

            IEnumerable<LarsSectorSubjectAreaTier2Lookup> sectorSubjectAreaTier2 =
                new List<LarsSectorSubjectAreaTier2Lookup>()
                {
                    new LarsSectorSubjectAreaTier2Lookup()
                    {
                        SectorSubjectAreaTier2 = 1.1m,
                        SectorSubjectAreaTier2Desc = "1.1"
                    },
                    new LarsSectorSubjectAreaTier2Lookup()
                    {
                        SectorSubjectAreaTier2 = 2.1m,
                        SectorSubjectAreaTier2Desc = "2.1"
                    },
                };

            var larsLearningDeliveryMock = larsLearningDeliveryList.AsQueryable().BuildMockDbSet();
            var larsAnnualValueMock = larsAnnualValueList.AsQueryable().BuildMockDbSet();
            var larsCategoriesMock = larsLearningDeliveryCategoryList.AsQueryable().BuildMockDbSet();
            var larsFundingMock = larsFundingsList.AsQueryable().BuildMockDbSet();
            var larsValiditiesMock = larsValiditiesList.AsQueryable().BuildMockDbSet();
            var larsFrameworkAimsMock = larsFrameworkAimsList.AsQueryable().BuildMockDbSet();

            var larsFrameworkMock = larsFrameworkList.AsQueryable().BuildMockDbSet();

            var larsSectorSubjectAreaTier2Lookup = sectorSubjectAreaTier2.AsQueryable().BuildMockDbSet();

            var larsMock = new Mock<ILARSContext>();

            larsMock.Setup(l => l.LARS_LearningDeliveries).Returns(larsLearningDeliveryMock.Object);
            larsMock.Setup(l => l.LARS_AnnualValues).Returns(larsAnnualValueMock.Object);
            larsMock.Setup(l => l.LARS_LearningDeliveryCategories).Returns(larsCategoriesMock.Object);
            larsMock.Setup(l => l.LARS_Fundings).Returns(larsFundingMock.Object);
            larsMock.Setup(l => l.LARS_Validities).Returns(larsValiditiesMock.Object);
            larsMock.Setup(l => l.LARS_FrameworkAims).Returns(larsFrameworkAimsMock.Object);
            larsMock.Setup(l => l.LARS_Frameworks).Returns(larsFrameworkMock.Object);
            larsMock.Setup(l => l.LARS_SectorSubjectAreaTier2Lookups).Returns(larsSectorSubjectAreaTier2Lookup.Object);

            var larsContextFactoryMock = new Mock<IDbContextFactory<ILARSContext>>();
            larsContextFactoryMock.Setup(c => c.Create()).Returns(larsMock.Object);

            var larsLearningDeliveries = await NewService(larsContextFactoryMock.Object).RetrieveAsync(larsLearningDeliveryKeys, CancellationToken.None);

            expectedLearningDeliveries.Should().BeEquivalentTo(larsLearningDeliveries);
        }

        private IReadOnlyCollection<LARSLearningDelivery> ExpectedLARSLearningDeliveries()
        {
            return new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery
                {
                    LearnAimRef = "LEARNAIMREF1",
                    LearnAimRefTitle = "AimRefTitle1",
                    LearnAimRefTypeDesc = "LearnAimRefTypeDesc1",
                    SectorSubjectAreaTier2 = 1.1m,
                    SectorSubjectAreaTier2Desc = "1.1",
                    LARSFrameworks = new List<LARSFramework>
                    {
                        new LARSFramework
                        {
                            FworkCode = 1,
                            ProgType = 2,
                            PwayCode = 3,
                            EffectiveFromNullable = new DateTime(2018, 8, 1),
                            LARSFrameworkCommonComponents = new List<LARSFrameworkCommonComponent>
                            {
                                new LARSFrameworkCommonComponent
                                {
                                    CommonComponent = 4,
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                },
                            },
                            LARSFrameworkApprenticeshipFundings = new List<LARSFrameworkApprenticeshipFunding>
                            {
                                new LARSFrameworkApprenticeshipFunding
                                {
                                    BandNumber = 1,
                                    CareLeaverAdditionalPayment = 2.0m,
                                    CoreGovContributionCap = 3.0m,
                                    Duration = 4,
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                    EffectiveTo = null,
                                    FundableWithoutEmployer = "5",
                                    FundingCategory = "6",
                                    MaxEmployerLevyCap = 7.0m,
                                    ReservedValue2 = 8.0m,
                                    ReservedValue3 = 9.0m,
                                    SixteenToEighteenEmployerAdditionalPayment = 10.0m,
                                    SixteenToEighteenFrameworkUplift = 11.0m,
                                    SixteenToEighteenIncentive = 12.0m,
                                    SixteenToEighteenProviderAdditionalPayment = 13.0m,
                                },
                            },
                            LARSFrameworkAim = new LARSFrameworkAim
                            {
                                LearnAimRef = "LEARNAIMREF1",
                                FrameworkComponentType = 1,
                                EffectiveFrom = new DateTime(2018, 8, 1),
                                EffectiveTo = null,
                            },
                        },
                    },
                    LARSFundings = new List<LARSFunding>
                    {
                        new LARSFunding
                        {
                            LearnAimRef = "LEARNAIMREF1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundingCategory = "Cat1",
                            RateUnWeighted = 1.0m,
                            RateWeighted = 2.0m,
                            WeightingFactor = "Factor",
                        },
                        new LARSFunding
                        {
                            LearnAimRef = "LEARNAIMREF1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundingCategory = "Cat2",
                            RateUnWeighted = 1.0m,
                            RateWeighted = 2.0m,
                            WeightingFactor = "Factor",
                        },
                        new LARSFunding
                        {
                            LearnAimRef = "LEARNAIMREF1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundingCategory = "Cat3",
                            RateUnWeighted = 1.0m,
                            RateWeighted = 2.0m,
                            WeightingFactor = "Factor",
                        },
                    },
                    LARSValidities = new List<LARSValidity>
                    {
                        new LARSValidity
                        {
                            LearnAimRef = "LEARNAIMREF1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            LastNewStartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat1",
                        },
                        new LARSValidity
                        {
                            LearnAimRef = "LEARNAIMREF1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            LastNewStartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat2",
                        },
                    },
                    LARSLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>(),
                    LARSAnnualValues = new List<LARSAnnualValue>(),
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "LEARNAIMREF2",
                    LearnAimRefTitle = "AimRefTitle2",
                    LearnAimRefTypeDesc = "LearnAimRefTypeDesc2",
                    SectorSubjectAreaTier2 = 2.1m,
                    SectorSubjectAreaTier2Desc = "2.1",
                    LARSFrameworks = new List<LARSFramework>
                    {
                        new LARSFramework
                        {
                            FworkCode = 1,
                            ProgType = 2,
                            PwayCode = 3,

                            EffectiveFromNullable = new DateTime(2018, 8, 1),
                            LARSFrameworkCommonComponents = new List<LARSFrameworkCommonComponent>
                            {
                                new LARSFrameworkCommonComponent
                                {
                                    CommonComponent = 4,
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                },
                            },
                            LARSFrameworkApprenticeshipFundings = new List<LARSFrameworkApprenticeshipFunding>
                            {
                                new LARSFrameworkApprenticeshipFunding
                                {
                                    BandNumber = 1,
                                    CareLeaverAdditionalPayment = 2.0m,
                                    CoreGovContributionCap = 3.0m,
                                    Duration = 4,
                                    EffectiveFrom = new DateTime(2018, 8, 1),
                                    EffectiveTo = null,
                                    FundableWithoutEmployer = "5",
                                    FundingCategory = "6",
                                    MaxEmployerLevyCap = 7.0m,
                                    ReservedValue2 = 8.0m,
                                    ReservedValue3 = 9.0m,
                                    SixteenToEighteenEmployerAdditionalPayment = 10.0m,
                                    SixteenToEighteenFrameworkUplift = 11.0m,
                                    SixteenToEighteenIncentive = 12.0m,
                                    SixteenToEighteenProviderAdditionalPayment = 13.0m,
                                },
                            },
                        },
                    },
                    LARSFundings = new List<LARSFunding>(),
                    LARSValidities = new List<LARSValidity>(),
                    LARSLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>
                    {
                        new LARSLearningDeliveryCategory
                        {
                            LearnAimRef = "LEARNAIMREF2",
                            CategoryRef = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                        },
                        new LARSLearningDeliveryCategory
                        {
                            LearnAimRef = "LEARNAIMREF2",
                            CategoryRef = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                        },
                    },
                    LARSAnnualValues = new List<LARSAnnualValue>
                    {
                        new LARSAnnualValue
                        {
                            LearnAimRef = "LEARNAIMREF2",
                            BasicSkills = 1,
                            BasicSkillsType = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FullLevel2EntitlementCategory = 3,
                            FullLevel3EntitlementCategory = 4,
                            FullLevel2Percent = 5,
                            FullLevel3Percent = 6,
                        },
                    },
                },
            };
        }

        private LarsLearningDeliveryRepositoryService NewService(IDbContextFactory<ILARSContext> larsContextFactory = null)
        {
            return new LarsLearningDeliveryRepositoryService(larsContextFactory);
        }
    }
}
