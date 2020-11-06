using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Abstract;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class OrganisationsRepositoryService : AbstractOrganisationsRepositoryService, IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>>
    {
        public OrganisationsRepositoryService(
            IDbContextFactory<IOrganisationsContext> organisationsFactory,
            IAcademicYearDataService academicYearDataService)
            : base(organisationsFactory, academicYearDataService)
        {
        }

        public async Task<IReadOnlyCollection<Organisation>> RetrieveAsync(IReadOnlyCollection<int> ukprnsInput, CancellationToken cancellationToken)
        {
            var ukprns = ukprnsInput.Select(u => (long)u).ToList();

            using (var context = _organisationsFactory.Create())
            {
                var specResourcesForUkprnDictionary = await BuildCampusIdSpecResourceDictionary(ukprns, context, cancellationToken);
                var campusIdentifiersDictionary = await BuildCampusIdentifiersDictionary(ukprns, specResourcesForUkprnDictionary, context, cancellationToken);
                var postcodeSpecResourcesDictionary = await BuildPostcodeSpecResDictionary(ukprns, context, cancellationToken);
                var shortTermFundingInitiativesDictionary = await BuildShortTermFundingInitiativesDictionary(ukprns, context, cancellationToken);

                var orgs = await context
                    .MasterOrganisations
                    .Include(mo => mo.OrgDetail)
                    .Include(mo => mo.OrgPartnerUkprns)
                    .Include(mo => mo.OrgFundings)
                    .Include(mo => mo.ConditionOfFundingRemovals)
                    .Where(mo => ukprns.Contains(mo.Ukprn))
                    .ToListAsync(cancellationToken);

                return BuildOrganisations(orgs, specResourcesForUkprnDictionary, campusIdentifiersDictionary, postcodeSpecResourcesDictionary, shortTermFundingInitiativesDictionary);
            }
        }

        private async Task<Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>>> BuildCampusIdSpecResourceDictionary(
            List<long> ukprns,
            IOrganisationsContext context,
            CancellationToken cancellationToken)
        {
            var campusIdentifierSpecResources = await context.CampusIdentifierSpecResources
                .Where(c => ukprns.Contains(c.MasterUkprn))
                .Select(x => new CampusIdentifierSpecResource()
                {
                    MasterUkprn = x.MasterUkprn,
                    CampusIdentifier = x.CampusIdentifier.ToUpperCase(),
                    EffectiveFrom = x.EffectiveFrom,
                    EffectiveTo = x.EffectiveTo,
                    SpecialistResources = x.SpecialistResources
                })
                .ToListAsync(cancellationToken);

            return BuildCampusIdSpecResourceDictionary(campusIdentifierSpecResources);
        }

        private async Task<Dictionary<long, List<OrganisationCampusIdentifier>>> BuildCampusIdentifiersDictionary(
            List<long> ukprns,
            Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>> specResourcesForUkprnDictionary,
            IOrganisationsContext context,
            CancellationToken cancellationToken)
        {
            var campusIdentifiers = await context
                .CampusIdentifiers?
                .Where(c => ukprns.Contains(c.MasterUkprn))
                .Select(ci => BuildCampusIdentifiers(ci, specResourcesForUkprnDictionary))
                .ToListAsync(cancellationToken);

            return BuildCampusIdentifiersDictionary(campusIdentifiers);
        }

        private async Task<Dictionary<long, List<OrganisationPostcodeSpecialistResource>>> BuildPostcodeSpecResDictionary(List<long> ukprns, IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var specResources = await context.ProviderPostcodeSpecialistResources.Where(c => ukprns.Contains(c.Ukprn)).ToListAsync(cancellationToken);

            return BuildPostcodeSpecResDictionary(specResources);
        }

        private async Task<Dictionary<long, List<OrganisationShortTermFundingInitiative>>> BuildShortTermFundingInitiativesDictionary(List<long> ukprns, IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var shortTermFundingInitiatives = await context.ShortTermFundingInitiatives.Where(c => ukprns.Contains(c.Ukprn)).ToListAsync(cancellationToken);

            return BuildShortTermFundingInitiativesDictionary(shortTermFundingInitiatives);
        }
    }
}
