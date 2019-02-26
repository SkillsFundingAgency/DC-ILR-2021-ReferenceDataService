using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisation;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EpaOrganisationsService : IRetrievalService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>>
    {
        private readonly IEpaContext _epaContext;

        public EpaOrganisationsService(IEpaContext epaContext)
        {
            _epaContext = epaContext;
        }

        public async Task<IReadOnlyDictionary<string, List<EPAOrganisation>>> RetrieveAsync(IReadOnlyCollection<string> epaOrgIds, CancellationToken cancellationToken)
        {
            var epaOrganisations = await _epaContext?
                        .Periods?.Where(o => epaOrgIds.Contains(o.OrganisationId))
                        .Select(epa => new EPAOrganisation
                        {
                            ID = epa.OrganisationId,
                            Standard = epa.StandardCode,
                            EffectiveFrom = epa.EffectiveFrom,
                            EffectiveTo = epa.EffectiveTo
                        }).ToListAsync(cancellationToken);

            return epaOrganisations
                .GroupBy(e => e.ID)
                .ToDictionary(key => key.Key, value => value.ToList(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
