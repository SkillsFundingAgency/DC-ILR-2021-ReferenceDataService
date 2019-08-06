using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
{
    public class DesktopOrganisationsRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>>
    {
        private readonly IOrganisationsContext _organisations;

        public DesktopOrganisationsRepositoryService(IOrganisationsContext organisations)
        {
            _organisations = organisations;
        }

        public async Task<IReadOnlyCollection<Organisation>> RetrieveAsync(CancellationToken cancellationToken)
        {
            var specResourcesForUkprnDictionary = await BuildSpecResourceDictionary(cancellationToken);
            var campusIdentifiersDictionary = await BuildCampusIdentifiersDictionary(specResourcesForUkprnDictionary, cancellationToken);

            return await _organisations
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

        public List<OrganisationCampusIdentifier> GetCampusIdentifiers(long ukprn, Dictionary<long, List<OrganisationCampusIdentifier>> campusIdentifiers)
        {
            campusIdentifiers.TryGetValue(ukprn, out var campusIds);

            return campusIds ?? new List<OrganisationCampusIdentifier>();
        }

        private async Task<Dictionary<long, Dictionary<string, List<SpecialistResource>>>> BuildSpecResourceDictionary(CancellationToken cancellationToken)
        {
            var campusIdentifierSpecResources = await _organisations
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
            CancellationToken cancellationToken)
        {
            var campusIdentifiers = await _organisations
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
