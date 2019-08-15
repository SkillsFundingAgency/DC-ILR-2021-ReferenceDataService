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
    public class DesktopLarsFrameworkAimsRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFrameworkAimDesktop>>
    {
        private readonly ILARSContext _larsContext;

        public DesktopLarsFrameworkAimsRepositoryService(ILARSContext larsContext)
        {
            _larsContext = larsContext;
        }

        public async Task<IReadOnlyCollection<LARSFrameworkAimDesktop>> RetrieveAsync(CancellationToken cancellationToken)
        {
            var larsFrameworkAims = await _larsContext.LARS_FrameworkAims.ToListAsync(cancellationToken);

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
