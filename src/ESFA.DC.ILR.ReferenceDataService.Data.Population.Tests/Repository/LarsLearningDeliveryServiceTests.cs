using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class LarsLearningDeliveryServiceTests
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
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FworkCode = 1,
                            LarsFramework = new LarsFramework
                            {
                                DataReceivedDate = new DateTime(2018, 8, 1)
                            }
                        },
                        new LarsFrameworkAim
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FworkCode = 2,
                            LarsFramework = new LarsFramework
                            {
                                DataReceivedDate = new DateTime(2018, 8, 1)
                            }
                        }
                    },
                    LarsFundings = new List<LarsFunding>
                    {
                        new LarsFunding
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FundingCategory = "Cat1"
                        },
                        new LarsFunding
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FundingCategory = "Cat2"
                        },
                        new LarsFunding
                        {
                            LearnAimRef = "LearnAimRef1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            FundingCategory = "Cat3"
                        }
                    },
                    LarsValidities = new List<LarsValidity>
                    {
                        new LarsValidity
                        {
                            LearnAimRef = "LearnAimRef1",
                            StartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat1"
                        },
                        new LarsValidity
                        {
                            LearnAimRef = "LearnAimRef1",
                            StartDate = new DateTime(2018, 8, 1),
                            ValidityCategory = "Cat2"
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
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            CategoryRef = 1
                        },
                        new LarsLearningDeliveryCategory
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            CategoryRef = 2
                        }
                    },
                    LarsAnnualValues = new List<LarsAnnualValue>
                    {
                        new LarsAnnualValue
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            BasicSkills = 1
                        }
                    },
                    LarsCareerLearningPilots = new List<LarsCareerLearningPilot>
                    {
                        new LarsCareerLearningPilot
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            AreaCode = "Area1"
                        },
                        new LarsCareerLearningPilot
                        {
                            LearnAimRef = "LearnAimRef2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            AreaCode = "Area1"
                        }
                    }
                },
            };

            var larsLearningDeliveryMock = larsLearningDeliveryList.AsQueryable().BuildMockDbSet();

            larsMock.Setup(l => l.LARS_LearningDeliveries).Returns(larsLearningDeliveryMock.Object);

            var lars = await NewService(larsMock.Object).RetrieveAsync(learnAimRefs, CancellationToken.None);

            lars.Select(k => k.Key).Should().HaveCount(2);
            lars.Select(k => k.Key).Should().Contain("LearnAimRef1");
            lars.Select(k => k.Key).Should().Contain("LearnAimRef2");
            lars.Select(k => k.Key).Should().NotContain("LearnAimRef3");

            lars["LearnAimRef1"].LearnAimRefTitle.Should().Be("AimRefTitle1");
            lars["LearnAimRef1"].LARSFrameworkAims.Should().HaveCount(2);
            lars["LearnAimRef1"].LARSFundings.Should().HaveCount(3);
            lars["LearnAimRef1"].LARSValidities.Should().HaveCount(2);

            lars["LearnAimRef2"].LearnAimRefTitle.Should().Be("AimRefTitle2");
            lars["LearnAimRef2"].LARSLearningDeliveryCategories.Should().HaveCount(2);
            lars["LearnAimRef2"].LARSAnnualValues.Should().HaveCount(1);
            lars["LearnAimRef2"].LARSCareerLearningPilots.Should().HaveCount(2);
        }

        [Fact]
        public void LARSAnnualValueFromEntity()
        {
            var annualValues = new LarsAnnualValue
            {
                LearnAimRef = "LearnAimRef",
                BasicSkills = 1,
                BasicSkillsType = 2,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                FullLevel2EntitlementCategory = 3,
                FullLevel3EntitlementCategory = 4,
                FullLevel3Percent = 5,
                CreatedBy = "CreatedBy"
            };

            var expectedAnnualValues = new LARSAnnualValue
            {
                BasicSkills = 1,
                BasicSkillsType = 2,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                FullLevel2EntitlementCategory = 3,
                FullLevel3EntitlementCategory = 4,
                FullLevel3Percent = 5
            };

            NewService().LARSAnnualValueFromEntity(annualValues).Should().BeEquivalentTo(expectedAnnualValues);
        }

        [Fact]
        public void LARSAnnualValueFromEntity_NullEntity()
        {
            NewService().LARSAnnualValueFromEntity(null).Should().BeEquivalentTo(new LARSAnnualValue());
        }

        [Fact]
        public void LARSCareerLearningPilotsFromEntity()
        {
            var careerLearningPilot = new LarsCareerLearningPilot
            {
                LearnAimRef = "LearnAimRef",
                AreaCode = "1",
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                SubsidyRate = 2,
                CreatedBy = "CreatedBy"
            };

            var expectedCareerLearningPilot = new LARSCareerLearningPilot
            {
                AreaCode = "1",
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                SubsidyRate = 2
            };

            NewService().LARSCareerLearningPilotsFromEntity(careerLearningPilot).Should().BeEquivalentTo(expectedCareerLearningPilot);
        }

        [Fact]
        public void LARSCareerLearningPilotsFromEntity_NullEntity()
        {
            NewService().LARSCareerLearningPilotsFromEntity(null).Should().BeEquivalentTo(new LARSCareerLearningPilot());
        }

        [Fact]
        public void LARSLearningDeliveryCategoriesFromEntity()
        {
            var category = new LarsLearningDeliveryCategory
            {
                LearnAimRef = "LearnAimRef",
                CategoryRef = 1,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                CreatedBy = "CreatedBy"
            };

            var expectedCategory = new LARSLearningDeliveryCategory
            {
                CategoryRef = 1,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null
            };

            NewService().LARSLearningDeliveryCategoriesFromEntity(category).Should().BeEquivalentTo(expectedCategory);
        }

        [Fact]
        public void LARSLearningDeliveryCategoriesFromEntity_NullEntity()
        {
            NewService().LARSLearningDeliveryCategoriesFromEntity(null).Should().BeEquivalentTo(new LARSLearningDeliveryCategory());
        }

        [Fact]
        public void LARSFundingsFromEntity()
        {
            var funding = new LarsFunding
            {
                LearnAimRef = "LearnAimRef",
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                FundingCategory = "Category",
                RateUnWeighted = 1.0m,
                RateWeighted = 2.0m,
                WeightingFactor = "Factor",
                CreatedBy = "CreatedBy"
            };

            var expectedFunding = new LARSFunding
            {
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                FundingCategory = "Category",
                RateUnWeighted = 1.0m,
                RateWeighted = 2.0m,
                WeightingFactor = "Factor"
            };

            NewService().LARSFundingsFromEntity(funding).Should().BeEquivalentTo(expectedFunding);
        }

        [Fact]
        public void LARSFundingsFromEntity_NullEntity()
        {
            NewService().LARSFundingsFromEntity(null).Should().BeEquivalentTo(new LARSFunding());
        }

        [Fact]
        public void LARSValiditiesFromEntity()
        {
            var validity = new LarsValidity
            {
                LearnAimRef = "LearnAimRef",
                StartDate = new DateTime(2018, 8, 1),
                EndDate = null,
                LastNewStartDate = new DateTime(2018, 8, 1),
                ValidityCategory = "Category",
                CreatedBy = "CreatedBy"
            };

            var expectedValidity = new LARSValidity
            {
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                LastNewStartDate = new DateTime(2018, 8, 1),
                ValidityCategory = "Category"
            };

            NewService().LARSValiditiesFromEntity(validity).Should().BeEquivalentTo(expectedValidity);
        }

        [Fact]
        public void LARSValiditiesFromEntity_NullEntity()
        {
            NewService().LARSValiditiesFromEntity(null).Should().BeEquivalentTo(new LARSValidity());
        }

        [Fact]
        public void LARSFrameworkAimFromEntity()
        {
            var frameworkAim = new LarsFrameworkAim
            {
                LearnAimRef = "LearnAimRef",
                FrameworkComponentType = 1,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                CreatedBy = "CreatedBy",
                LarsFramework = new LarsFramework
                {
                    FworkCode = 1,
                    ProgType = 2,
                    PwayCode = 3,
                    EffectiveFrom = new DateTime(2018, 8, 1)
                }
            };

            var expectedFrameworkAim = new LARSFrameworkAim
            {
                FrameworkComponentType = 1,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                LARSFramework = new LARSFramework
                {
                    FworkCode = 1,
                    ProgType = 2,
                    PwayCode = 3,
                    EffectiveFromNullable = new DateTime(2018, 8, 1),
                    LARSFrameworkApprenticeshipFundings = new List<LARSFrameworkApprenticeshipFunding>(),
                    LARSFrameworkCommonComponents = new List<LARSFrameworkCommonComponent>()
                }
            };

            NewService().LARSFrameworkAimFromEntity(frameworkAim).Should().BeEquivalentTo(expectedFrameworkAim);
        }

        [Fact]
        public void LARSFrameworkAimFromEntity_NullEntity()
        {
            NewService().LARSFrameworkAimFromEntity(null).Should().BeEquivalentTo(new LARSFrameworkAim());
        }

        [Fact]
        public void LARSFrameworkFromEntity()
        {
            var framework = new LarsFramework
            {
                FworkCode = 1,
                ProgType = 2,
                PwayCode = 3,
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            var expectedFramework = new LARSFramework
            {
                FworkCode = 1,
                ProgType = 2,
                PwayCode = 3,
                EffectiveFromNullable = new DateTime(2018, 8, 1),
                LARSFrameworkApprenticeshipFundings = new List<LARSFrameworkApprenticeshipFunding>(),
                LARSFrameworkCommonComponents = new List<LARSFrameworkCommonComponent>()
            };

            NewService().LARSFrameworkFromEntity(framework).Should().BeEquivalentTo(expectedFramework);
        }

        [Fact]
        public void LARSFrameworkFromEntity_NullEntity()
        {
            NewService().LARSFrameworkFromEntity(null).Should().BeEquivalentTo(new LARSFramework());
        }

        [Fact]
        public void LARSFrameworkComCmpFromEntity()
        {
            var cmnComp = new LarsFrameworkCmnComp
            {
                FworkCode = 1,
                ProgType = 2,
                PwayCode = 3,
                CommonComponent = 4,
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            var expectedCmnComp = new LARSFrameworkCommonComponent
            {
                CommonComponent = 4,
                EffectiveFrom = new DateTime(2018, 8, 1)
            };

            NewService().LARSFrameworkComCmpFromEntity(cmnComp).Should().BeEquivalentTo(expectedCmnComp);
        }

        [Fact]
        public void LARSFrameworkComCmpFromEntity_NullEntity()
        {
            NewService().LARSFrameworkComCmpFromEntity(null).Should().BeEquivalentTo(new LARSFrameworkCommonComponent());
        }

        [Fact]
        public void LARSFrameworkAppFundingFromEntity()
        {
            var funding = new LarsApprenticeshipFworkFunding
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
            };

            var expectedFunding = new LARSFrameworkApprenticeshipFunding
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
            };

            NewService().LARSFrameworkAppFundingFromEntity(funding).Should().BeEquivalentTo(expectedFunding);
        }

        [Fact]
        public void LARSFrameworkAppFundingFromEntity_NullEntity()
        {
            NewService().LARSFrameworkAppFundingFromEntity(null).Should().BeEquivalentTo(new LARSFrameworkApprenticeshipFunding());
        }

        private LarsLearningDeliveryService NewService(ILARSContext larsContext = null)
        {
            return new LarsLearningDeliveryService(larsContext);
        }
    }
}
