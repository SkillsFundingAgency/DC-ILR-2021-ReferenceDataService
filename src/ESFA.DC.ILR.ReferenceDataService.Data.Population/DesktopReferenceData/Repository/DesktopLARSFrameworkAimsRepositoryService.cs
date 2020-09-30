using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopLarsFrameworkAimsRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkAimDesktop>>
    {
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;
        private readonly IReferenceDataStatisticsService _referenceDataStatisticsService;

        public DesktopLarsFrameworkAimsRepositoryService(
            IDbContextFactory<ILARSContext> larsContextFactory,
            IReferenceDataStatisticsService referenceDataStatisticsService)
        {
            _larsContextFactory = larsContextFactory;
            _referenceDataStatisticsService = referenceDataStatisticsService;
        }

        public async Task<IReadOnlyCollection<LARSFrameworkAimDesktop>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _larsContextFactory.Create())
            {
                var larsFrameworkAims = await context.LARS_FrameworkAims.ToListAsync(cancellationToken);
                _referenceDataStatisticsService.AddRecordCount("LARS FrameworkAims", larsFrameworkAims.Count);

                return larsFrameworkAims
                    .Select(lf => new LARSFrameworkAimDesktop
                    {
                        FworkCode = lf.FworkCode,
                        ProgType = lf.ProgType,
                        PwayCode = lf.PwayCode,
                        FrameworkComponentType = lf.FrameworkComponentType,
                        LearnAimRef = lf.LearnAimRef,
                        EffectiveFrom = lf.EffectiveFrom,
                        EffectiveTo = lf.EffectiveTo,
                    }).ToList();
            }
        }
    }
}
