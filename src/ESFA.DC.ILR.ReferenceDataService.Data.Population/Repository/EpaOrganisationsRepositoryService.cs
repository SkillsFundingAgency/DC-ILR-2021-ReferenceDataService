using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EpaOrganisationsRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>>
    {
        private readonly IEpaContext _epaContext;

        public EpaOrganisationsRepositoryService(IEpaContext epaContext)
        {
            _epaContext = epaContext;
        }

        public async Task<IReadOnlyCollection<EPAOrganisation>> RetrieveAsync(IReadOnlyCollection<string> epaOrgIds, CancellationToken cancellationToken)
        {
            return await _epaContext?
                        .Periods?.Where(o => epaOrgIds.Contains(o.OrganisationId))
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
