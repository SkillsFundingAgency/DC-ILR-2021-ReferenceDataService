using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
{
    public class DesktopLarsFrameworkAimsRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkAimDesktop>>
    {
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;

        public DesktopLarsFrameworkAimsRepositoryService(IDbContextFactory<ILARSContext> larsContextFactory)
        {
            _larsContextFactory = larsContextFactory;
        }

        public async Task<IReadOnlyCollection<LARSFrameworkAimDesktop>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _larsContextFactory.Create())
            {
                var larsFrameworkAims = await context.LARS_FrameworkAims.ToListAsync(cancellationToken);

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
