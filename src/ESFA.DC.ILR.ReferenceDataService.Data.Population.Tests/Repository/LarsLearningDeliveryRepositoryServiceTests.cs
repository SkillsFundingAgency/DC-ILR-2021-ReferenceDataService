﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;
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
                new LARSLearningDeliveryKey("LearnAimRef1", 1, 2, 3),
                new LARSLearningDeliveryKey("LearnAimRef2", 1, 2, 3)
            };

            var larsMock = new Mock<ILARSContext>();

            IEnumerable<LarsLearningDelivery> larsLearningDeliveryList = new List<LarsLearningDelivery>
            {
                new LarsLearningDelivery
                {
                    LearnAimRef = "LearnAimRef1",
                    LearnAimRefTitle = "AimRefTitle1",
                    LarsFundings = new List<LarsFunding>
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
                            CreatedBy = "CreatedBy"
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
                            CreatedBy = "CreatedBy"
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
                            CreatedBy = "CreatedBy"
                        }
                    },
                    LarsValidities = new List<LarsValidity>
                    {
                        new LarsValidity
                        {
                            LearnAimRef = "LearnAimRef1",
                            StartDate = new DateTime(2018, 8, 1),
                            EndDate = null,
                            LastNewStartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat1",
                            CreatedBy = "CreatedBy"
                        },
                        new LarsValidity
                        {
                            LearnAimRef = "LearnAimRef1",
                            StartDate = new DateTime(2018, 8, 1),
                            EndDate = null,
                            LastNewStartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat2",
                            CreatedBy = "CreatedBy"
                        }
                    }
                },
                new LarsLearningDelivery
                {
                    LearnAimRef = "LearnAimRef2",
                    LearnAimRefTitle = "AimRefTitle2",
                    LarsLearningDeliveryCategories = new List<LarsLearningDeliveryCategory>
                    {
                        new LarsLearningDeliveryCategory
                        {
                            LearnAimRef = "LearnAimRef2",
                            CategoryRef = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            CreatedBy = "CreatedBy"
                        },
                        new LarsLearningDeliveryCategory
                        {
                           LearnAimRef = "LearnAimRef2",
                            CategoryRef = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            CreatedBy = "CreatedBy"
                        }
                    },
                    LarsAnnualValues = new List<LarsAnnualValue>
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
                            FullLevel3Percent = 5,
                            CreatedBy = "CreatedBy"
                        }
                    },
                    LarsCareerLearningPilots = new List<LarsCareerLearningPilot>
                    {
                        new LarsCareerLearningPilot
                        {
                            LearnAimRef = "LearnAimRef2",
                            AreaCode = "1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            SubsidyRate = 2,
                            CreatedBy = "CreatedBy"
                        },
                        new LarsCareerLearningPilot
                        {
                            LearnAimRef = "LearnAimRef2",
                            AreaCode = "1.2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            SubsidyRate = 2,
                            CreatedBy = "CreatedBy"
                        }
                    }
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
                   LarsFrameworkAims = new List<LarsFrameworkAim>
                   {
                       new LarsFrameworkAim
                       {
                           EffectiveFrom = new DateTime(2018, 8, 1),
                           FrameworkComponentType = 1,
                           LearnAimRef = "LearnAimRef1"
                       },
                   },
                   LarsFrameworkCmnComps = new List<LarsFrameworkCmnComp>
                   {
                       new LarsFrameworkCmnComp
                       {
                           FworkCode = 1,
                           ProgType = 2,
                           PwayCode = 3,
                           CommonComponent = 4,
                           EffectiveFrom = new DateTime(2018, 8, 1)
                       }
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
                           CreatedBy = "CreatedBy"
                       }
                   }
                }
            };

            var larsLearningDeliveryMock = larsLearningDeliveryList.AsQueryable().BuildMockDbSet();
            var larsFrameworkMock = larsFrameworkList.AsQueryable().BuildMockDbSet();

            larsMock.Setup(l => l.LARS_LearningDeliveries).Returns(larsLearningDeliveryMock.Object);
            larsMock.Setup(l => l.LARS_Frameworks).Returns(larsFrameworkMock.Object);

            var larsLearningDeliveries = await NewService(larsMock.Object).RetrieveAsync(larsLearningDeliveryKeys, CancellationToken.None);

            expectedLearningDeliveries.Should().BeEquivalentTo(larsLearningDeliveries);
        }

        private IReadOnlyCollection<LARSLearningDelivery> ExpectedLARSLearningDeliveries()
        {
            return new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery
                {
                    LearnAimRef = "LearnAimRef1",
                    LearnAimRefTitle = "AimRefTitle1",
                    LARSFramework = new LARSFramework
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
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
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
                                SixteenToEighteenProviderAdditionalPayment = 13.0m
                            }
                        },
                        LARSFrameworkAim = new LARSFrameworkAim
                        {
                            FrameworkComponentType = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null
                        }
                    },
                    LARSFundings = new List<LARSFunding>
                    {
                        new LARSFunding
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundingCategory = "Cat1",
                            RateUnWeighted = 1.0m,
                            RateWeighted = 2.0m,
                            WeightingFactor = "Factor"
                        },
                        new LARSFunding
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundingCategory = "Cat2",
                            RateUnWeighted = 1.0m,
                            RateWeighted = 2.0m,
                            WeightingFactor = "Factor"
                        },
                        new LARSFunding
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundingCategory = "Cat3",
                            RateUnWeighted = 1.0m,
                            RateWeighted = 2.0m,
                            WeightingFactor = "Factor"
                        }
                    },
                    LARSValidities = new List<LARSValidity>
                    {
                        new LARSValidity
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            LastNewStartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat1"
                        },
                        new LARSValidity
                        {
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            LastNewStartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat2"
                        }
                    },
                    LARSCareerLearningPilots = new List<LARSCareerLearningPilot>(),
                    LARSLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>(),
                    LARSAnnualValues = new List<LARSAnnualValue>()
                },
                new LARSLearningDelivery
                {
                    LearnAimRef = "LearnAimRef2",
                    LearnAimRefTitle = "AimRefTitle2",
                    LARSFramework = new LARSFramework
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
                                EffectiveFrom = new DateTime(2018, 8, 1)
                            }
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
                                SixteenToEighteenProviderAdditionalPayment = 13.0m
                            }
                        }
                    },
                    LARSFundings = new List<LARSFunding>(),
                    LARSValidities = new List<LARSValidity>(),
                    LARSLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>
                    {
                        new LARSLearningDeliveryCategory
                        {
                            CategoryRef = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null
                        },
                        new LARSLearningDeliveryCategory
                        {
                            CategoryRef = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null
                        }
                    },
                    LARSAnnualValues = new List<LARSAnnualValue>
                    {
                        new LARSAnnualValue
                        {
                            BasicSkills = 1,
                            BasicSkillsType = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FullLevel2EntitlementCategory = 3,
                            FullLevel3EntitlementCategory = 4,
                            FullLevel3Percent = 5
                        }
                    },
                    LARSCareerLearningPilots = new List<LARSCareerLearningPilot>
                    {
                        new LARSCareerLearningPilot
                        {
                            AreaCode = "1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            SubsidyRate = 2
                        },
                        new LARSCareerLearningPilot
                        {
                            AreaCode = "1.2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            SubsidyRate = 2
                        }
                    }
                },
            };
        }

        private LarsLearningDeliveryRepositoryService NewService(ILARSContext larsContext = null)
        {
            return new LarsLearningDeliveryRepositoryService(larsContext);
        }
    }
}
