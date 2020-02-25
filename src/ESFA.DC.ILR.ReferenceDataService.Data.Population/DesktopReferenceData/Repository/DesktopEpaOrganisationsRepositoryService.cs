using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
{
    public class DesktopEpaOrganisationsRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>>
    {
        private readonly IDbContextFactory<IEpaContext> _epaContextFactory;

        public DesktopEpaOrganisationsRepositoryService(IDbContextFactory<IEpaContext> epaContextFactory)
        {
            _epaContextFactory = epaContextFactory;
        }

        public async Task<IReadOnlyCollection<EPAOrganisation>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _epaContextFactory.Create())
            {
                return await context?
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
}
