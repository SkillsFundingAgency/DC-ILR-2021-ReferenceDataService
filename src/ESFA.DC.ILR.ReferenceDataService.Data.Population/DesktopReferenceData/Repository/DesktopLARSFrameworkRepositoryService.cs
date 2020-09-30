using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Constants;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopLarsFrameworkRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkDesktop>>
    {
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;
        private readonly IReferenceDataStatisticsService _referenceDataStatisticsService;

        public DesktopLarsFrameworkRepositoryService(
            IDbContextFactory<ILARSContext> larsContextFactory,
            IReferenceDataStatisticsService referenceDataStatisticsService)
        {
            _larsContextFactory = larsContextFactory;
            _referenceDataStatisticsService = referenceDataStatisticsService;
        }

        public async Task<IReadOnlyCollection<LARSFrameworkDesktop>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _larsContextFactory.Create())
            {
                var larsFrameworks = await context.LARS_Frameworks
                .Include(l => l.LarsApprenticeshipFworkFundings)
                .Include(l => l.LarsFrameworkCmnComps)
                .ToListAsync(cancellationToken);

                _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.LarsFrameworks, larsFrameworks.Count);
                _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.LarsFrameworksCommonComponents, larsFrameworks.Select(lf => lf.LarsFrameworkCmnComps).Count());
                _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.LarsFrameworksApprenticeshipFunding, larsFrameworks.Select(lf => lf.LarsApprenticeshipFworkFundings).Count());

                return larsFrameworks
                    .Select(lf => new LARSFrameworkDesktop
                    {
                        FworkCode = lf.FworkCode,
                        ProgType = lf.ProgType,
                        PwayCode = lf.PwayCode,
                        EffectiveFromNullable = lf.EffectiveFrom,
                        EffectiveTo = lf.EffectiveTo,
                        LARSFrameworkApprenticeshipFundings = lf.LarsApprenticeshipFworkFundings.Select(laf =>
                        new LARSFrameworkApprenticeshipFunding
                        {
                            BandNumber = laf.BandNumber,
                            CareLeaverAdditionalPayment = laf.CareLeaverAdditionalPayment,
                            CoreGovContributionCap = laf.CoreGovContributionCap,
                            Duration = laf.Duration,
                            EffectiveFrom = laf.EffectiveFrom,
                            EffectiveTo = laf.EffectiveTo,
                            FundableWithoutEmployer = laf.FundableWithoutEmployer,
                            FundingCategory = laf.FundingCategory,
                            MaxEmployerLevyCap = laf.MaxEmployerLevyCap,
                            ReservedValue2 = laf.ReservedValue2,
                            ReservedValue3 = laf.ReservedValue3,
                            SixteenToEighteenEmployerAdditionalPayment = laf._1618employerAdditionalPayment,
                            SixteenToEighteenFrameworkUplift = laf._1618frameworkUplift,
                            SixteenToEighteenIncentive = laf._1618incentive,
                            SixteenToEighteenProviderAdditionalPayment = laf._1618providerAdditionalPayment,
                        }).ToList(),
                        LARSFrameworkCommonComponents = lf.LarsFrameworkCmnComps.Select(lcc =>
                        new LARSFrameworkCommonComponent
                        {
                            CommonComponent = lcc.CommonComponent,
                            EffectiveFrom = lcc.EffectiveFrom,
                            EffectiveTo = lcc.EffectiveTo,
                        }).ToList(),
                    }).ToList();
            }
        }
    }
}
