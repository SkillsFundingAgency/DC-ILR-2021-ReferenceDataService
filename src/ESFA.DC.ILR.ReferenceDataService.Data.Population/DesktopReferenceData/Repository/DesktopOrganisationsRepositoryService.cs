using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Abstract;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopOrganisationsRepositoryService : AbstractOrganisationsRepositoryService, IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>>
    {
        public DesktopOrganisationsRepositoryService(
            IDbContextFactory<IOrganisationsContext> organisationsFactory,
            IAcademicYearDataService academicYearDataService)
            : base(organisationsFactory, academicYearDataService)
        {
        }

        public async Task<IReadOnlyCollection<Organisation>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _organisationsFactory.Create())
            {
                var specResourcesForUkprnDictionary = await BuildCampusIdSpecResourceDictionary(context, cancellationToken);
                var campusIdentifiersDictionary = await BuildCampusIdentifiersDictionary(specResourcesForUkprnDictionary, context, cancellationToken);
                var postcodeSpecResourcesDictionary = await BuildPostcodeSpecResDictionary(context, cancellationToken);
                var shortTermFundingInitiativesDictionary = await BuildShortTermFundingInitiativesDictionary(context, cancellationToken);

                return await context
                  .MasterOrganisations
                  .Include(mo => mo.OrgDetail)
                  .Include(mo => mo.OrgPartnerUkprns)
                  .Include(mo => mo.OrgFundings)
                  .Include(mo => mo.ConditionOfFundingRemovals)
                  .Select(x => BuildOrganisations(x, specResourcesForUkprnDictionary, campusIdentifiersDictionary, postcodeSpecResourcesDictionary, shortTermFundingInitiativesDictionary))
                  .ToListAsync(cancellationToken);
            }
        }

        private async Task<Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>>> BuildCampusIdSpecResourceDictionary(IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var campusIdentifierSpecResources = await context.CampusIdentifierSpecResources?.ToListAsync(cancellationToken);

            return BuildCampusIdSpecResourceDictionary(campusIdentifierSpecResources);
        }

        private async Task<Dictionary<long, List<OrganisationCampusIdentifier>>> BuildCampusIdentifiersDictionary(
            Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>> specResourcesForUkprnDictionary,
            IOrganisationsContext context,
            CancellationToken cancellationToken)
        {
            var campusIdentifiers = await context
                .CampusIdentifiers?
                .Select(ci => BuildCampusIdentifiers(ci, specResourcesForUkprnDictionary))
                .ToListAsync(cancellationToken);

            return BuildCampusIdentifiersDictionary(campusIdentifiers);
        }

        private async Task<Dictionary<long, List<OrganisationPostcodeSpecialistResource>>> BuildPostcodeSpecResDictionary(IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var specResources = await context.ProviderPostcodeSpecialistResources.ToListAsync(cancellationToken);

            return BuildPostcodeSpecResDictionary(specResources);
        }

        private async Task<Dictionary<long, List<OrganisationShortTermFundingInitiative>>> BuildShortTermFundingInitiativesDictionary(IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var stfis = await context.ShortTermFundingInitiatives.ToListAsync(cancellationToken);

            return BuildShortTermFundingInitiativesDictionary(stfis);
        }
    }
}
