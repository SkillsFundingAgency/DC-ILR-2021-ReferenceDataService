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
    public class LarsStandardRepositoryServiceTests
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
                        },
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 2,
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
                        }
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 1,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 2,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
                        }
                    },
                    LarsStandardFundings = new List<LarsStandardFunding>
                    {
                        new LarsStandardFunding
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
                        },
                        new LarsStandardFunding
                        {
                            AchievementIncentive = 2.0m,
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
                        }
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat1",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat2",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
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
                        },
                        new LarsApprenticeshipStdFunding
                        {
                            BandNumber = 4,
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
                        }
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 3,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 4,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
                        }
                    },
                    LarsStandardFundings = new List<LarsStandardFunding>
                    {
                        new LarsStandardFunding
                        {
                            AchievementIncentive = 3.0m,
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
                        },
                        new LarsStandardFunding
                        {
                            AchievementIncentive = 4.0m,
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
                        }
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat3",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat4",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 8,
                            CreatedBy = "CreatedBy"
                        }
                    }
                }
            };

            var larsStandardMock = larsStandardList.AsQueryable().BuildMockDbSet();

            larsMock.Setup(l => l.LARS_Standards).Returns(larsStandardMock.Object);

            var lars = await NewService(larsMock.Object).RetrieveAsync(stdCodes, CancellationToken.None);

            lars.Should().HaveCount(2);
            lars.Select(l => l.StandardCode).Should().Contain(1);
            lars.Select(l => l.StandardCode).Should().Contain(2);
            lars.Select(l => l.StandardCode).Should().NotContain(3);

            lars.Where(l => l.StandardCode == 1).Select(l => l.StandardSectorCode).Should().BeEquivalentTo("SSC1");
            lars.Where(l => l.StandardCode == 1).SelectMany(l => l.LARSStandardApprenticeshipFundings).Should().HaveCount(2);
            lars.Where(l => l.StandardCode == 1).SelectMany(l => l.LARSStandardCommonComponents).Should().HaveCount(2);
            lars.Where(l => l.StandardCode == 1).SelectMany(l => l.LARSStandardFundings).Should().HaveCount(2);
            lars.Where(l => l.StandardCode == 1).SelectMany(l => l.LARSStandardValidities).Should().HaveCount(2);

            lars.Where(l => l.StandardCode == 2).Select(l => l.StandardSectorCode).Should().BeEquivalentTo("SSC2");
            lars.Where(l => l.StandardCode == 2).SelectMany(l => l.LARSStandardApprenticeshipFundings).Should().HaveCount(2);
            lars.Where(l => l.StandardCode == 2).SelectMany(l => l.LARSStandardCommonComponents).Should().HaveCount(2);
            lars.Where(l => l.StandardCode == 2).SelectMany(l => l.LARSStandardFundings).Should().HaveCount(2);
            lars.Where(l => l.StandardCode == 2).SelectMany(l => l.LARSStandardValidities).Should().HaveCount(2);
        }

        private LarsStandardRepositoryService NewService(ILARSContext larsContext = null)
        {
            return new LarsStandardRepositoryService(larsContext);
        }
    }
}
