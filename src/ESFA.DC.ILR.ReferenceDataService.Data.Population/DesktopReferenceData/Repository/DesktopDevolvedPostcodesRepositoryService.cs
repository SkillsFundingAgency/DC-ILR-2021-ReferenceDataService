using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
{
    public class DesktopDevolvedPostcodesRepositoryService : IDesktopReferenceDataRepositoryService<DevolvedPostcodes>
    {
        private readonly IPostcodesContext _postcodesContext;

        public DesktopDevolvedPostcodesRepositoryService(IPostcodesContext postcodesContext)
        {
            _postcodesContext = postcodesContext;
        }

        public async Task<DevolvedPostcodes> RetrieveAsync(CancellationToken cancellationToken)
        {
            var mcaGlaFullNames = await _postcodesContext.McaglaFullNames?
                .Where(e => e.EffectiveTo == null)
                .ToDictionaryAsync(
                k => k.McaglaShortCode,
                v => v.FullName,
                StringComparer.OrdinalIgnoreCase,
                cancellationToken);

            var mcaGlaSofCodes = await _postcodesContext.McaglaSofs?.ToListAsync(cancellationToken);

            var mcaGlaSofLookups = mcaGlaSofCodes.Select(m => new McaGlaSofLookup
            {
                SofCode = m.SofCode,
                McaGlaShortCode = m.McaglaShortCode,
                McaGlaFullName = mcaGlaFullNames.TryGetValue(m.McaglaShortCode, out var fullname) ? fullname : string.Empty,
                EffectiveFrom = m.EffectiveFrom,
                EffectiveTo = m.EffectiveTo
            }).ToList();

            return new DevolvedPostcodes
            {
                McaGlaSofLookups = mcaGlaSofLookups
            };
        }
    }
}
