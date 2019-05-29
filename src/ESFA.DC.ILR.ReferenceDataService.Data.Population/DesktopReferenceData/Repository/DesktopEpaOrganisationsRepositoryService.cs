using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
{
    public class DesktopEpaOrganisationsRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>>
    {
        private readonly IEpaContext _epaContext;

        public DesktopEpaOrganisationsRepositoryService(IEpaContext epaContext)
        {
            _epaContext = epaContext;
        }

        public async Task<IReadOnlyCollection<EPAOrganisation>> RetrieveAsync(CancellationToken cancellationToken)
        {
            return await _epaContext?
                        .Periods?
                        .Select(epa => new EPAOrganisation
                        {
                            ID = epa.OrganisationId,
                            Standard = epa.StandardCode,
                            EffectiveFrom = epa.EffectiveFrom,
                            EffectiveTo = epa.EffectiveTo,
                        }).ToListAsync(cancellationToken);
        }
    }
}
