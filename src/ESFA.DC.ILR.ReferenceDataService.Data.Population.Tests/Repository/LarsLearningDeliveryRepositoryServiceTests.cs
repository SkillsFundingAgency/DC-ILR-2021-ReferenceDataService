﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
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
            var learnAimRefs = new List<string> { "LearnAimRef1", "LearnAimRef2", "LearnAimRef3" };

            var larsMock = new Mock<ILARSContext>();

            IEnumerable<LarsLearningDelivery> larsLearningDeliveryList = new List<LarsLearningDelivery>
            {
                new LarsLearningDelivery
                {
                    LearnAimRef = "LearnAimRef1",
                    LearnAimRefTitle = "AimRefTitle1",
                    LarsFrameworkAims = new List<LarsFrameworkAim>
                    {
                        new LarsFrameworkAim
                        {
                            FworkCode = 1,
                            ProgType = 2,
                            PwayCode = 3,
                            LearnAimRef = "LearnAimRef1",
                            FrameworkComponentType = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            CreatedBy = "CreatedBy",
                            LarsFramework = new LarsFramework
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
                        },
                        new LarsFrameworkAim
                        {
                            FworkCode = 2,
                            ProgType = 2,
                            PwayCode = 3,
                            LearnAimRef = "LearnAimRef1",
                            FrameworkComponentType = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            CreatedBy = "CreatedBy",
                            LarsFramework = new LarsFramework
                            {
                                FworkCode = 2,
                                ProgType = 2,
                                PwayCode = 3,
                                EffectiveFrom = new DateTime(2018, 8, 1),
                                LarsFrameworkCmnComps = new List<LarsFrameworkCmnComp>
                                {
                                    new LarsFrameworkCmnComp
                                    {
                                        FworkCode = 2,
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
                                        FworkCode = 2,
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
                        },
                    },
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

            var larsLearningDeliveryMock = larsLearningDeliveryList.AsQueryable().BuildMockDbSet();

            larsMock.Setup(l => l.LARS_LearningDeliveries).Returns(larsLearningDeliveryMock.Object);

            var lars = await NewService(larsMock.Object).RetrieveAsync(learnAimRefs, CancellationToken.None);

            lars.Should().HaveCount(2);
            lars.Select(l => l.LearnAimRef).Should().Contain("LearnAimRef1");
            lars.Select(l => l.LearnAimRef).Should().Contain("LearnAimRef2");
            lars.Select(l => l.LearnAimRef).Should().NotContain("LearnAimRef3");

            lars.Where(l => l.LearnAimRef == "LearnAimRef1").Select(l => l.LearnAimRefTitle).Should().BeEquivalentTo("AimRefTitle1");
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSFrameworkAims).Should().HaveCount(2);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSFundings).Should().HaveCount(3);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSValidities).Should().HaveCount(2);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSFrameworkAims.Select(lfa => lfa.LARSFramework)).Should().HaveCount(2);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSFrameworkAims.Select(lfa => lfa.LARSFramework.LARSFrameworkApprenticeshipFundings)).Should().HaveCount(2);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSFrameworkAims.Select(lfa => lfa.LARSFramework.LARSFrameworkCommonComponents)).Should().HaveCount(2);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSCareerLearningPilots).Should().HaveCount(0);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSLearningDeliveryCategories).Should().HaveCount(0);
            lars.Where(l => l.LearnAimRef == "LearnAimRef1").SelectMany(l => l.LARSAnnualValues).Should().HaveCount(0);

            lars.Where(l => l.LearnAimRef == "LearnAimRef2").Select(l => l.LearnAimRefTitle).Should().BeEquivalentTo("AimRefTitle2");
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSCareerLearningPilots).Should().HaveCount(2);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSLearningDeliveryCategories).Should().HaveCount(2);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSAnnualValues).Should().HaveCount(1);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSFrameworkAims).Should().HaveCount(0);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSFundings).Should().HaveCount(0);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSValidities).Should().HaveCount(0);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSFrameworkAims.Select(lfa => lfa.LARSFramework)).Should().HaveCount(0);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSFrameworkAims.Select(lfa => lfa.LARSFramework.LARSFrameworkApprenticeshipFundings)).Should().HaveCount(0);
            lars.Where(l => l.LearnAimRef == "LearnAimRef2").SelectMany(l => l.LARSFrameworkAims.Select(lfa => lfa.LARSFramework.LARSFrameworkCommonComponents)).Should().HaveCount(0);
        }

        private LarsLearningDeliveryRepositoryService NewService(ILARSContext larsContext = null)
        {
            return new LarsLearningDeliveryRepositoryService(larsContext);
        }
    }
}
