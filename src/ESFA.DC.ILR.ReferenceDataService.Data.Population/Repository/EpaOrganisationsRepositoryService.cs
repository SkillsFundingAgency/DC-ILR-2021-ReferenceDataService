using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EpaOrganisationsRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>>
    {
        private readonly IDbContextFactory<IEpaContext> _epaContextFactory;

        public EpaOrganisationsRepositoryService(IDbContextFactory<IEpaContext> epaContextFactory)
        {
            _epaContextFactory = epaContextFactory;
        }

        public async Task<IReadOnlyCollection<EPAOrganisation>> RetrieveAsync(IReadOnlyCollection<string> epaOrgIds, CancellationToken cancellationToken)
        {
            using (var context = _epaContextFactory.Create())
            {
                return await context?
                        .Periods?.Where(o => epaOrgIds.Contains(o.OrganisationId, StringComparer.OrdinalIgnoreCase))
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
}
