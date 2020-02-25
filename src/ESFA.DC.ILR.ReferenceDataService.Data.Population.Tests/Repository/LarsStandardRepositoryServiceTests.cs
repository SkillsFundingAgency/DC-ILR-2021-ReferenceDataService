using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
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
            var expectedLARSStandards = ExpectedLARSStandards();

            var stdCodes = new List<int> { 1, 2, 3 };

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
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
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
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
                        },
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 1,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 2,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
                        },
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
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
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
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
                        },
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat1",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat2",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 1,
                            CreatedBy = "CreatedBy",
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
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
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
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
                        },
                    },
                    LarsStandardCommonComponents = new List<LarsStandardCommonComponent>
                    {
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 3,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
                        },
                        new LarsStandardCommonComponent
                        {
                            CommonComponent = 4,
                            MinLevel = "2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
                        },
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
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
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
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
                        },
                    },
                    LarsStandardValidities = new List<LarsStandardValidity>
                    {
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat3",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
                        },
                        new LarsStandardValidity
                        {
                            ValidityCategory = "Cat4",
                            StartDate = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                            StandardCode = 2,
                            CreatedBy = "CreatedBy",
                        },
                    },
                },
            };

            var larsStandardMock = larsStandardList.AsQueryable().BuildMockDbSet();

            var larsMock = new Mock<ILARSContext>();
            larsMock.Setup(l => l.LARS_Standards).Returns(larsStandardMock.Object);

            var larsContextFactoryMock = new Mock<IDbContextFactory<ILARSContext>>();
            larsContextFactoryMock.Setup(c => c.Create()).Returns(larsMock.Object);

            var larsStandards = await NewService(larsContextFactoryMock.Object).RetrieveAsync(stdCodes, CancellationToken.None);

            expectedLARSStandards.Should().BeEquivalentTo(larsStandards);
        }

        private IReadOnlyCollection<LARSStandard> ExpectedLARSStandards()
        {
            return new List<LARSStandard>
            {
                new LARSStandard
                {
                    StandardCode = 1,
                    StandardSectorCode = "SSC1",
                    NotionalEndLevel = "NEL1",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LARSStandardApprenticeshipFundings = new List<LARSStandardApprenticeshipFunding>
                    {
                        new LARSStandardApprenticeshipFunding
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
                            SixteenToEighteenProviderAdditionalPayment = 13.0m,
                        },
                        new LARSStandardApprenticeshipFunding
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
                            SixteenToEighteenEmployerAdditionalPayment = 10.0m,
                            SixteenToEighteenFrameworkUplift = 11.0m,
                            SixteenToEighteenIncentive = 12.0m,
                            SixteenToEighteenProviderAdditionalPayment = 13.0m,
                        },
                    },
                    LARSStandardCommonComponents = new List<LARSStandardCommonComponent>
                    {
                        new LARSStandardCommonComponent
                        {
                            CommonComponent = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                        },
                        new LARSStandardCommonComponent
                        {
                            CommonComponent = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                        },
                    },
                    LARSStandardFundings = new List<LARSStandardFunding>
                    {
                        new LARSStandardFunding
                        {
                            AchievementIncentive = 1.0m,
                            BandNumber = 2,
                            CoreGovContributionCap = 3.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundableWithoutEmployer = "4",
                            FundingCategory = "5",
                            SixteenToEighteenIncentive = 6.0m,
                            SmallBusinessIncentive = 7.0m,
                        },
                        new LARSStandardFunding
                        {
                            AchievementIncentive = 2.0m,
                            BandNumber = 2,
                            CoreGovContributionCap = 3.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundableWithoutEmployer = "4",
                            FundingCategory = "5",
                            SixteenToEighteenIncentive = 6.0m,
                            SmallBusinessIncentive = 7.0m,
                        },
                    },
                    LARSStandardValidities = new List<LARSStandardValidity>
                    {
                        new LARSStandardValidity
                        {
                            ValidityCategory = "Cat1",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                        },
                        new LARSStandardValidity
                        {
                            ValidityCategory = "Cat2",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                        },
                    },
                },
                new LARSStandard
                {
                    StandardCode = 2,
                    StandardSectorCode = "SSC2",
                    NotionalEndLevel = "NEL2",
                    EffectiveFrom = new DateTime(2018, 8, 1),
                    LARSStandardApprenticeshipFundings = new List<LARSStandardApprenticeshipFunding>
                    {
                        new LARSStandardApprenticeshipFunding
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
                            SixteenToEighteenEmployerAdditionalPayment = 10.0m,
                            SixteenToEighteenFrameworkUplift = 11.0m,
                            SixteenToEighteenIncentive = 12.0m,
                            SixteenToEighteenProviderAdditionalPayment = 13.0m,
                        },
                        new LARSStandardApprenticeshipFunding
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
                            SixteenToEighteenEmployerAdditionalPayment = 10.0m,
                            SixteenToEighteenFrameworkUplift = 11.0m,
                            SixteenToEighteenIncentive = 12.0m,
                            SixteenToEighteenProviderAdditionalPayment = 13.0m,
                        },
                    },
                    LARSStandardCommonComponents = new List<LARSStandardCommonComponent>
                    {
                        new LARSStandardCommonComponent
                        {
                            CommonComponent = 3,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                        },
                        new LARSStandardCommonComponent
                        {
                            CommonComponent = 4,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                        },
                    },
                    LARSStandardFundings = new List<LARSStandardFunding>
                    {
                        new LARSStandardFunding
                        {
                            AchievementIncentive = 3.0m,
                            BandNumber = 2,
                            CoreGovContributionCap = 3.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundableWithoutEmployer = "4",
                            FundingCategory = "5",
                            SixteenToEighteenIncentive = 6.0m,
                            SmallBusinessIncentive = 7.0m,
                        },
                        new LARSStandardFunding
                        {
                            AchievementIncentive = 4.0m,
                            BandNumber = 2,
                            CoreGovContributionCap = 3.0m,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = null,
                            FundableWithoutEmployer = "4",
                            FundingCategory = "5",
                            SixteenToEighteenIncentive = 6.0m,
                            SmallBusinessIncentive = 7.0m,
                        },
                    },
                    LARSStandardValidities = new List<LARSStandardValidity>
                    {
                        new LARSStandardValidity
                        {
                            ValidityCategory = "Cat3",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                        },
                        new LARSStandardValidity
                        {
                            ValidityCategory = "Cat4",
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            LastNewStartDate = new DateTime(2018, 9, 1),
                        },
                    },
                },
            };
        }

        private LarsStandardRepositoryService NewService(IDbContextFactory<ILARSContext> larsContextFactory = null)
        {
            return new LarsStandardRepositoryService(larsContextFactory);
        }
    }
}
