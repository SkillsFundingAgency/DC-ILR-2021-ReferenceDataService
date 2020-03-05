using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopOrganisationsRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>>
    {
        private const int LongTermResidValue = 1;
        private readonly IDbContextFactory<IOrganisationsContext> _organisationsFactory;

        public DesktopOrganisationsRepositoryService(IDbContextFactory<IOrganisationsContext> organisationsFactory)
        {
            _organisationsFactory = organisationsFactory;
        }

        public async Task<IReadOnlyCollection<Organisation>> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _organisationsFactory.Create())
            {
                var specResourcesForUkprnDictionary = await BuildSpecResourceDictionary(context, cancellationToken);
                var campusIdentifiersDictionary = await BuildCampusIdentifiersDictionary(specResourcesForUkprnDictionary, context, cancellationToken);

                return await context
                    .MasterOrganisations
                      .Include(mo => mo.OrgDetail)
                      .Include(mo => mo.OrgPartnerUkprns)
                      .Include(mo => mo.OrgFundings)
                      .Include(mo => mo.ConditionOfFundingRemovals)
                      .Select(
                          o => new Organisation
                          {
                              UKPRN = (int)o.Ukprn,
                              Name = o.OrgDetail.Name,
                              LegalOrgType = o.OrgDetail.LegalOrgType,
                              PartnerUKPRN = o.OrgPartnerUkprns.Any(op => op.Ukprn == o.Ukprn),
                              LongTermResid = o.OrgDetail.LongTermResid == LongTermResidValue,
                              CampusIdentifers = GetCampusIdentifiers(o.Ukprn, campusIdentifiersDictionary),
                              OrganisationFundings = o.OrgFundings.Select(of =>
                              new OrganisationFunding()
                              {
                                  OrgFundFactor = of.FundingFactor,
                                  OrgFundFactType = of.FundingFactorType,
                                  OrgFundFactValue = of.FundingFactorValue,
                                  EffectiveFrom = of.EffectiveFrom,
                                  EffectiveTo = of.EffectiveTo,
                              }).ToList(),
                              OrganisationCoFRemovals = o.ConditionOfFundingRemovals.Select(c =>
                              new OrganisationCoFRemoval
                              {
                                  CoFRemoval = c.CoFremoval,
                                  EffectiveFrom = c.EffectiveFrom,
                                  EffectiveTo = c.EffectiveTo,
                              }).ToList(),
                          }).ToListAsync(cancellationToken);
            }
        }

        public List<OrganisationCampusIdentifier> GetCampusIdentifiers(long ukprn, Dictionary<long, List<OrganisationCampusIdentifier>> campusIdentifiers)
        {
            campusIdentifiers.TryGetValue(ukprn, out var campusIds);

            return campusIds ?? new List<OrganisationCampusIdentifier>();
        }

        private async Task<Dictionary<long, Dictionary<string, List<SpecialistResource>>>> BuildSpecResourceDictionary(IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var campusIdentifierSpecResources = await context
             .CampusIdentifierSpecResources?
             .ToListAsync(cancellationToken);

            return campusIdentifierSpecResources?
               .GroupBy(sr => sr.MasterUkprn)
               .ToDictionary(
                  k1 => k1.Key,
                  v1 => v1.GroupBy(ci => ci.CampusIdentifier)
                  .ToDictionary(
                      k2 => k2.Key,
                      v2 => v2.Select(sr => new SpecialistResource
                      {
                          IsSpecialistResource = sr.SpecialistResources,
                          EffectiveFrom = sr.EffectiveFrom,
                          EffectiveTo = sr.EffectiveTo
                      }).ToList()));
        }

        private async Task<Dictionary<long, List<OrganisationCampusIdentifier>>> BuildCampusIdentifiersDictionary(
            Dictionary<long, Dictionary<string, List<SpecialistResource>>> specResourcesForUkprnDictionary,
            IOrganisationsContext context,
            CancellationToken cancellationToken)
        {
            var campusIdentifiers = await context
              .CampusIdentifiers?
              .Select(ci => new OrganisationCampusIdentifier
              {
                  UKPRN = ci.MasterUkprn,
                  CampusIdentifier = ci.CampusIdentifier1,
                  EffectiveFrom = ci.EffectiveFrom,
                  EffectiveTo = ci.EffectiveTo,
                  SpecialistResources = GetSpecialistResources(ci.MasterUkprn, ci.CampusIdentifier1, specResourcesForUkprnDictionary).ToList()
              })
              .ToListAsync(cancellationToken);

            return
                campusIdentifiers
                .GroupBy(ci => ci.UKPRN)
                .ToDictionary(
                k => k.Key,
                v => v.Select(c => c).ToList());
        }

        private IEnumerable<SpecialistResource> GetSpecialistResources(long ukprn, string campusIdentifier, Dictionary<long, Dictionary<string, List<SpecialistResource>>> specResourcesDictionary)
        {
            specResourcesDictionary.TryGetValue(ukprn, out var campusIdSpecResources);

            return campusIdSpecResources != null ?
                  campusIdSpecResources.TryGetValue(campusIdentifier, out var resources) ? resources : Enumerable.Empty<SpecialistResource>() : Enumerable.Empty<SpecialistResource>();
        }
    }
}
