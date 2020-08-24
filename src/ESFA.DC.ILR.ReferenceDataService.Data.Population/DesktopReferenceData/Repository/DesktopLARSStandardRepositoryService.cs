using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopLarsStandardRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>>
    {
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;

        public DesktopLarsStandardRepositoryService(IDbContextFactory<ILARSContext> larsContextFactory)
        {
            _larsContextFactory = larsContextFactory;
        }

        public async Task<IReadOnlyCollection<LARSStandard>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _larsContextFactory.Create())
            {
                var larsStandards = await context.LARS_Standards
                .Include(l => l.LarsApprenticeshipStdFundings)
                .Include(l => l.LarsStandardCommonComponents)
                .Include(l => l.LarsStandardFundings)
                .Include(l => l.LarsStandardValidities)
                .ToListAsync(cancellationToken);

                return larsStandards
                    .Select(
                        ls => new LARSStandard
                        {
                            StandardCode = ls.StandardCode,
                            StandardSectorCode = ls.StandardSectorCode,
                            NotionalEndLevel = ls.NotionalEndLevel,
                            EffectiveFrom = ls.EffectiveFrom.Value,
                            EffectiveTo = ls.EffectiveTo,
                            LastDateStarts = ls.LastDateStarts,
                            LARSStandardApprenticeshipFundings = ls.LarsApprenticeshipStdFundings.Select(lsa =>
                            new LARSStandardApprenticeshipFunding
                            {
                                BandNumber = lsa.BandNumber,
                                CareLeaverAdditionalPayment = lsa.CareLeaverAdditionalPayment,
                                CoreGovContributionCap = lsa.CoreGovContributionCap,
                                Duration = lsa.Duration,
                                EffectiveFrom = lsa.EffectiveFrom,
                                EffectiveTo = lsa.EffectiveTo,
                                FundableWithoutEmployer = lsa.FundableWithoutEmployer,
                                FundingCategory = lsa.FundingCategory,
                                MaxEmployerLevyCap = lsa.MaxEmployerLevyCap,
                                ProgType = lsa.ProgType,
                                PwayCode = lsa.PwayCode,
                                ReservedValue2 = lsa.ReservedValue2,
                                ReservedValue3 = lsa.ReservedValue3,
                                SixteenToEighteenEmployerAdditionalPayment = lsa._1618employerAdditionalPayment,
                                SixteenToEighteenFrameworkUplift = lsa._1618frameworkUplift,
                                SixteenToEighteenIncentive = lsa._1618incentive,
                                SixteenToEighteenProviderAdditionalPayment = lsa._1618providerAdditionalPayment,
                            }).ToList(),
                            LARSStandardCommonComponents = ls.LarsStandardCommonComponents.Select(lsc =>
                            new LARSStandardCommonComponent
                            {
                                CommonComponent = lsc.CommonComponent,
                                EffectiveFrom = lsc.EffectiveFrom,
                                EffectiveTo = lsc.EffectiveTo,
                            }).ToList(),
                            LARSStandardFundings = ls.LarsStandardFundings.Select(lsf =>
                            new LARSStandardFunding
                            {
                                AchievementIncentive = lsf.AchievementIncentive,
                                BandNumber = lsf.BandNumber,
                                CoreGovContributionCap = lsf.CoreGovContributionCap,
                                EffectiveFrom = lsf.EffectiveFrom,
                                EffectiveTo = lsf.EffectiveTo,
                                FundableWithoutEmployer = lsf.FundableWithoutEmployer,
                                FundingCategory = lsf.FundingCategory,
                                SixteenToEighteenIncentive = lsf._1618incentive,
                                SmallBusinessIncentive = lsf.SmallBusinessIncentive,
                            }).ToList(),
                            LARSStandardValidities = ls.LarsStandardValidities.Select(lsv =>
                            new LARSStandardValidity
                            {
                                EffectiveFrom = lsv.StartDate,
                                EffectiveTo = lsv.EndDate,
                                LastNewStartDate = lsv.LastNewStartDate,
                                ValidityCategory = lsv.ValidityCategory,
                            }).ToList(),
                        }).ToList();
            }
        }
    }
}
