using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class LarsStandardRepositoryService : ILarsStandardRepositoryService
    {
        private readonly ILARSContext _larsContext;

        public LarsStandardRepositoryService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyCollection<LARSStandard>> RetrieveAsync(IReadOnlyCollection<int> stdCodes, CancellationToken cancellationToken)
        {
            return await _larsContext.LARS_Standards
                .Include(ls => ls.LarsStandardAims)
                .Include(ls => ls.LarsApprenticeshipStdFundings)
                .Include(ls => ls.LarsStandardCommonComponents)
                .Include(ls => ls.LarsStandardFundings)
                .Include(ls => ls.LarsStandardValidities)
                .Where(l => stdCodes.Contains(l.StandardCode))
                .Select(
                    ls => new LARSStandard
                    {
                        StandardCode = ls.StandardCode,
                        StandardSectorCode = ls.StandardSectorCode,
                        NotionalEndLevel = ls.NotionalEndLevel,
                        EffectiveFrom = ls.EffectiveFrom.Value,
                        EffectiveTo = ls.EffectiveTo,
                        LARSStandardApprenticeshipFundings = ls.LarsApprenticeshipStdFundings.Select(lsa => LARSStandardAppFundingFromEntity(lsa)).ToList(),
                        LARSStandardCommonComponents = ls.LarsStandardCommonComponents.Select(lsc => LARSStandardComCmpFromEntity(lsc)).ToList(),
                        LARSStandardFundings = ls.LarsStandardFundings.Select(lsf => LARSStandardFundingFromEntity(lsf)).ToList(),
                        LARSStandardValidities = ls.LarsStandardValidities.Select(lsv => LARSStandardValidityFromEntity(lsv)).ToList()
                    }).ToListAsync(cancellationToken);
        }

        public LARSStandardFunding LARSStandardFundingFromEntity(LarsStandardFunding larsStandardFunding)
        {
            if (larsStandardFunding == null)
            {
                return new LARSStandardFunding();
            }

            return new LARSStandardFunding
            {
                AchievementIncentive = larsStandardFunding.AchievementIncentive,
                BandNumber = larsStandardFunding.BandNumber,
                CoreGovContributionCap = larsStandardFunding.CoreGovContributionCap,
                EffectiveFrom = larsStandardFunding.EffectiveFrom,
                EffectiveTo = larsStandardFunding.EffectiveTo,
                FundableWithoutEmployer = larsStandardFunding.FundableWithoutEmployer,
                FundingCategory = larsStandardFunding.FundingCategory,
                SixteenToEighteenIncentive = larsStandardFunding._1618incentive,
                SmallBusinessIncentive = larsStandardFunding.SmallBusinessIncentive
            };
        }

        public LARSStandardCommonComponent LARSStandardComCmpFromEntity(LarsStandardCommonComponent larsStandardCommonComponent)
        {
            if (larsStandardCommonComponent == null)
            {
                return new LARSStandardCommonComponent();
            }

            return new LARSStandardCommonComponent
            {
                CommonComponent = larsStandardCommonComponent.CommonComponent,
                EffectiveFrom = larsStandardCommonComponent.EffectiveFrom,
                EffectiveTo = larsStandardCommonComponent.EffectiveTo
            };
        }

        public LARSStandardApprenticeshipFunding LARSStandardAppFundingFromEntity(LarsApprenticeshipStdFunding larsApprenticeshipStdFunding)
        {
            if (larsApprenticeshipStdFunding == null)
            {
                return new LARSStandardApprenticeshipFunding();
            }

            return new LARSStandardApprenticeshipFunding
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
                ProgType = larsApprenticeshipStdFunding.ProgType,
                PwayCode = larsApprenticeshipStdFunding.PwayCode,
                ReservedValue2 = larsApprenticeshipStdFunding.ReservedValue2,
                ReservedValue3 = larsApprenticeshipStdFunding.ReservedValue3,
                SixteenToEighteenEmployerAdditionalPayment = larsApprenticeshipStdFunding._1618employerAdditionalPayment,
                SixteenToEighteenFrameworkUplift = larsApprenticeshipStdFunding._1618frameworkUplift,
                SixteenToEighteenIncentive = larsApprenticeshipStdFunding._1618incentive,
                SixteenToEighteenProviderAdditionalPayment = larsApprenticeshipStdFunding._1618providerAdditionalPayment
            };
        }

        public LARSStandardValidity LARSStandardValidityFromEntity(LarsStandardValidity larsValidity)
        {
            if (larsValidity == null)
            {
                return new LARSStandardValidity();
            }

            return new LARSStandardValidity
            {
                EffectiveFrom = larsValidity.StartDate,
                EffectiveTo = larsValidity.EndDate,
                LastNewStartDate = larsValidity.LastNewStartDate,
                ValidityCategory = larsValidity.ValidityCategory
            };
        }
    }
}
