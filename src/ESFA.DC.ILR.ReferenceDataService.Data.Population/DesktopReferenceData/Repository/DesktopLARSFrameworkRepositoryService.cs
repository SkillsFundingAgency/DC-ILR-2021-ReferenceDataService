using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
{
    public class DesktopLarsFrameworkRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFramework>>
    {
        private readonly ILARSContext _larsContext;

        public DesktopLarsFrameworkRepositoryService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyCollection<LARSFramework>> RetrieveAsync(CancellationToken cancellationToken)
        {
            var larsFrameworks = await _larsContext.LARS_Frameworks
                .Include(l => l.LarsFrameworkAims)
                .Include(l => l.LarsApprenticeshipFworkFundings)
                .Include(l => l.LarsFrameworkCmnComps)
                .ToListAsync(cancellationToken);

            return larsFrameworks
                .Select(lf => new LARSFramework
                {
                    FworkCode = lf.FworkCode,
                    ProgType = lf.ProgType,
                    PwayCode = lf.PwayCode,
                    EffectiveFromNullable = lf.EffectiveFrom,
                    EffectiveTo = lf.EffectiveTo,
                    LARSFrameworkAim = lf.LarsFrameworkAims.Select(lfa =>
                    new LARSFrameworkAim
                    {
                        EffectiveFrom = lfa.EffectiveFrom,
                        EffectiveTo = lfa.EffectiveTo,
                        FrameworkComponentType = lfa.FrameworkComponentType,
                    }).FirstOrDefault(),
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
