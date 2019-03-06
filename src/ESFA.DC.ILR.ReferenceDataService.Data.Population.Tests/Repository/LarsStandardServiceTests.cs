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
    public class LarsStandardServiceTests
    {
        [Fact]
        public async Task RetrieveAsync()
        {
            var stdCodes = new List<int> { 1, 2, 3 };

            var larsMock = new Mock<ILARSContext>();

            IEnumerable<LarsStandard> larsStandardList = new List<LarsStandard>
            {
                new LarsStandard
                {
                    StandardCode = 1,
                    StandardSectorCode = "SSC1",
                    NotionalEndLevel = "NEL1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LarsApprenticeshipStdFundings = new List<LarsApprenticeshipStdFunding>
                    {
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 1,
                            CareLeaverAdditionalPayment = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 2,
                            CareLeaverAdditionalPayment = 2.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardFundings = new List<LarsStandardFunding>
                    {
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 2.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        }
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat1",
                            StartDate = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat2",
                            StartDate = new DateTime(2018, 8, 1),
                        },
                    },
                },
                new LarsStandard
                {
                    StandardCode = 2,
                    StandardSectorCode = "SSC2",
                    NotionalEndLevel = "NEL2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LarsApprenticeshipStdFundings = new List<LarsApprenticeshipStdFunding>
                    {
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 3,
                            CareLeaverAdditionalPayment = 1.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 4,
                            CareLeaverAdditionalPayment = 2.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 3,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 4,
                            EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                    LarsStandardFundings = new List<LarsStandardFunding>
                    {
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 3.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardFunding
                        {
                            CoreGovContributionCap = 4.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        }
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat3",
                            StartDate = new DateTime(2018, 8, 1),
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat4"
                        }
                    }
                }
            };

            var larsStandardMock = larsStandardList.AsQueryable().BuildMockDbSet();

            larsMock.Setup(l => l.LARS_Standards).Returns(larsStandardMock.Object);

            var lars = await NewService(larsMock.Object).RetrieveAsync(stdCodes, CancellationToken.None);

            lars.Select(k => k.Key).Should().HaveCount(2);
            lars.Select(k => k.Key).Should().Contain(1);
            lars.Select(k => k.Key).Should().Contain(2);
            lars.Select(k => k.Key).Should().NotContain(3);

            lars[1].StandardSectorCode.Should().Be("SSC1");
            lars[1].LARSStandardApprenticeshipFundings.Should().HaveCount(2);
            lars[1].LARSStandardCommonComponents.Should().HaveCount(2);
            lars[1].LARSStandardFundings.Should().HaveCount(2);
            lars[1].LARSStandardValidities.Should().HaveCount(2);

            lars[2].StandardSectorCode.Should().Be("SSC2");
            lars[2].LARSStandardApprenticeshipFundings.Should().HaveCount(2);
            lars[2].LARSStandardCommonComponents.Should().HaveCount(2);
            lars[2].LARSStandardFundings.Should().HaveCount(2);
            lars[2].LARSStandardValidities.Should().HaveCount(2);
        }

        [Fact]
        public void LARSStandardFundingFromEntity()
        {
            var funding = new LarsStandardFunding
            {
                AchievementIncentive = 1.0m,
                BandNumber = 2,
                CoreGovContributionCap = 3.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                FundableWithoutEmployer = "4",
                FundingCategory = "5",
                _1618incentive = 6.0m,
                SmallBusinessIncentive = 7.0m,
                StandardCode = 8,
                CreatedBy = "CreatedBy"
            };

            var expectedFunding = new LARSStandardFunding
            {
                AchievementIncentive = 1.0m,
                BandNumber = 2,
                CoreGovContributionCap = 3.0m,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                FundableWithoutEmployer = "4",
                FundingCategory = "5",
                SixteenToEighteenIncentive = 6.0m,
                SmallBusinessIncentive = 7.0m
            };

            NewService().LARSStandardFundingFromEntity(funding).Should().BeEquivalentTo(expectedFunding);
        }

        [Fact]
        public void LARSStandardFundingFromEntity_NullEntity()
        {
             NewService().LARSStandardFundingFromEntity(null).Should().BeEquivalentTo(new LARSStandardFunding());
        }

        [Fact]
        public void LARSStandardComCmpFromEntity()
        {
            var cmnComp = new LarsStandardCommonComponent
            {
                CommonComponent = 1,
                MinLevel = "2",
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null,
                StandardCode = 8,
                CreatedBy = "CreatedBy"
            };

            var expectedCmnComp = new LARSStandardCommonComponent
            {
                CommonComponent = 1,
                EffectiveFrom = new DateTime(2018, 8, 1),
                EffectiveTo = null
            };

            NewService().LARSStandardComCmpFromEntity(cmnComp).Should().BeEquivalentTo(expectedCmnComp);
        }

        [Fact]
        public void LARSStandardComCmpFromEntity_NullEntity()
        {
            NewService().LARSStandardComCmpFromEntity(null).Should().BeEquivalentTo(new LARSStandardCommonComponent());
        }

        [Fact]
        public void LARSStandardAppFundingFromEntity()
        {
            var funding = new LarsApprenticeshipStdFunding
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
                ProgType = 25,
                PwayCode = 0,
                ReservedValue2 = 8.0m,
                ReservedValue3 = 9.0m,
                _1618employerAdditionalPayment = 10.0m,
                _1618frameworkUplift = 11.0m,
                _1618incentive = 12.0m,
                _1618providerAdditionalPayment = 13.0m,
                StandardCode = 8,
                CreatedBy = "CreatedBy"
            };

            var expectedFunding = new LARSStandardApprenticeshipFunding
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
                ProgType = 25,
                PwayCode = 0,
                ReservedValue2 = 8.0m,
                ReservedValue3 = 9.0m,
                SixteenToEighteenEmployerAdditionalPayment = 10.0m,
                SixteenToEighteenFrameworkUplift = 11.0m,
                SixteenToEighteenIncentive = 12.0m,
                SixteenToEighteenProviderAdditionalPayment = 13.0m
            };

            NewService().LARSStandardAppFundingFromEntity(funding).Should().BeEquivalentTo(expectedFunding);
        }

        [Fact]
        public void LARSStandardAppFundingFromEntity_NullEntity()
        {
            NewService().LARSStandardAppFundingFromEntity(null).Should().BeEquivalentTo(new LARSStandardApprenticeshipFunding());
        }

        [Fact]
        public void LARSStandardValidityFromEntity()
        {
            var validity = new LarsStandardValidity
            {
                ValidityCategory = "Category",
                StartDate = new DateTime(2018, 8, 1),
                LastNewStartDate = new DateTime(2018, 9, 1),
                StandardCode = 8,
                CreatedBy = "CreatedBy"
            };

            var expectedValidity = new LARSStandardValidity
            {
                ValidityCategory = "Category",
                EffectiveFrom = new DateTime(2018, 8, 1),
                LastNewStartDate = new DateTime(2018, 9, 1),
                EffectiveTo = null
            };

            NewService().LARSStandardValidityFromEntity(validity).Should().BeEquivalentTo(expectedValidity);
        }

        [Fact]
        public void LARSStandardValidityFromEntity_NullEntity()
        {
            NewService().LARSStandardValidityFromEntity(null).Should().BeEquivalentTo(new LARSStandardValidity());
        }

        private LarsStandardService NewService(ILARSContext larsContext = null)
        {
            return new LarsStandardService(larsContext);
        }
    }
}
