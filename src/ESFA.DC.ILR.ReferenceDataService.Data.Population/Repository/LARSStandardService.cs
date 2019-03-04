using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class LarsStandardService : IRetrievalService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>>
    {
        private readonly ILARSContext _larsContext;

        public LarsStandardService()
        {
        }

        public LarsStandardService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyDictionary<int, LARSStandard>> RetrieveAsync(IReadOnlyCollection<int> stdCodes, CancellationToken cancellationToken)
        {
            var larsStandards = await _larsContext.LARS_Standards
                .Where(l => stdCodes.Contains(l.StandardCode))
                .Select(
                    ls => new LARSStandard
                    {
                        StandardCode = ls.StandardCode,
                        StandardSectorCode = ls.StandardSectorCode,
                        NotionalEndLevel = ls.NotionalEndLevel,
                        EffectiveFrom = ls.EffectiveFrom.Value,
                        EffectiveTo = ls.EffectiveTo,
                        LARSStandardApprenticeshipFundings = ls.LarsApprenticeshipStdFundings.Select(LARSStandardAppFundingFromEntity).ToList(),
                        LARSStandardCommonComponents = ls.LarsStandardCommonComponents.Select(LARSStandardComCmpFromEntity).ToList(),
                        LARSStandardFundings = ls.LarsStandardFundings.Select(LARSStandardFundingFromEntity).ToList(),
                        LARSStandardValidities = ls.LarsStandardValidities.Select(LARSStandardValidityFromEntity).ToList()
                    }).ToListAsync(cancellationToken);

            return larsStandards
                .GroupBy(ls => ls.StandardCode)
                .ToDictionary(key => key.Key, value => value.FirstOrDefault());
        }

        private LARSStandardFunding LARSStandardFundingFromEntity(LarsStandardFunding larsStandardFunding)
        {
            var funding = new LARSStandardFunding
            {
                AchievementIncentive = larsStandardFunding.AchievementIncentive,
                BandNumber = larsStandardFunding.BandNumber,
                CoreGovContributionCap = larsStandardFunding.CoreGovContributionCap,
                EffectiveFrom = larsStandardFunding.EffectiveFrom,
                EffectiveTo = larsStandardFunding.EffectiveTo,
                FundableWithoutEmployer = larsStandardFunding.FundableWithoutEmployer,
                FundingCategory = larsStandardFunding.FundingCategory,
                SixteenToEighteenIncentive = larsStandardFunding._1618incentive
            };

            return funding ?? new LARSStandardFunding();
        }

        private LARSStandardCommonComponent LARSStandardComCmpFromEntity(LarsStandardCommonComponent larsStandardCommonComponent)
        {
            var commonComponent = new LARSStandardCommonComponent
            {
                CommonComponent = larsStandardCommonComponent.CommonComponent,
                EffectiveFrom = larsStandardCommonComponent.EffectiveFrom,
                EffectiveTo = larsStandardCommonComponent.EffectiveTo
            };

            return commonComponent ?? new LARSStandardCommonComponent();
        }

        private LARSStandardApprenticeshipFunding LARSStandardAppFundingFromEntity(LarsApprenticeshipStdFunding larsApprenticeshipStdFunding)
        {
            var larsAppStdFunding = new LARSStandardApprenticeshipFunding
            {
                BandNumber = larsApprenticeshipStdFunding.BandNumber,
                CareLeaverAdditionalPayment = larsApprenticeshipStdFunding.CareLeaverAdditionalPayment,
                CoreGovContributionCap = larsApprenticeshipStdFunding.CoreGovContributionCap,
                Duration = larsApprenticeshipStdFunding.Duration,
                EffectiveFrom = larsApprenticeshipStdFunding.EffectiveFrom,
                EffectiveTo = larsApprenticeshipStdFunding.EffectiveTo,
                FundableWithoutEmployer = larsApprenticeshipStdFunding.FundableWithoutEmployer,
                FundingCategory = larsApprenticeshipStdFunding.FundingCategory,
                MaxEmployerLevyCap = larsApprenticeshipStdFunding.MaxEmployerLevyCap,
                ProgType = 25,
                PwayCode = 0,
                ReservedValue2 = larsApprenticeshipStdFunding.ReservedValue2,
                ReservedValue3 = larsApprenticeshipStdFunding.ReservedValue3,
                SixteenToEighteenEmployerAdditionalPayment = larsApprenticeshipStdFunding._1618employerAdditionalPayment,
                SixteenToEighteenFrameworkUplift = larsApprenticeshipStdFunding._1618frameworkUplift,
                SixteenToEighteenIncentive = larsApprenticeshipStdFunding._1618incentive,
                SixteenToEighteenProviderAdditionalPayment = larsApprenticeshipStdFunding._1618providerAdditionalPayment
            };

            return larsAppStdFunding ?? new LARSStandardApprenticeshipFunding();
        }

        private LARSStandardValidity LARSStandardValidityFromEntity(LarsStandardValidity larsValidity)
        {
            var validity = new LARSStandardValidity
            {
                EffectiveFrom = larsValidity.StartDate,
                EffectiveTo = larsValidity.EndDate,
                LastNewStartDate = larsValidity.LastNewStartDate,
                ValidityCategory = larsValidity.ValidityCategory
            };

            return validity ?? new LARSStandardValidity();
        }
    }
}
