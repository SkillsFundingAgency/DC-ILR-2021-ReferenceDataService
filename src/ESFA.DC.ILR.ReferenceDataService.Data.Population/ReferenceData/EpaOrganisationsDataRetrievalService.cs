using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.EPAOrganisation;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.ReferenceData
{
    public class EpaOrganisationsDataRetrievalService : IEpaOrganisationsDataRetrievalService
    {
        private readonly IEpaContext _epaContext;

        public EpaOrganisationsDataRetrievalService()
        {
        }

        public EpaOrganisationsDataRetrievalService(IEpaContext epaContext)
        {
            _epaContext = epaContext;
        }

        public async Task<IReadOnlyDictionary<string, List<EPAOrganisation>>> RetrieveAsync(IReadOnlyCollection<string> epaOrgIds, CancellationToken cancellationToken)
        {
            return await _epaContext?
                        .Periods?.Where(o => epaOrgIds.Contains(o.OrganisationId))
                        .GroupBy(o => o.OrganisationId)
                        .ToDictionaryAsync(
                           k => k.Key,
                           v => v.Select(epa => new EPAOrganisation
                           {
                               ID = epa.OrganisationId,
                               Standard = epa.StandardCode,
                               EffectiveFrom = epa.EffectiveFrom,
                               EffectiveTo = epa.EffectiveTo
                           }).ToList(),
                           StringComparer.OrdinalIgnoreCase,
                           cancellationToken);
        }
    }
}
