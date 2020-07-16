using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class OrganisationsRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>>
    {
        private const int LongTermResidValue = 1;
        private readonly List<OrganisationCampusIdentifier> _defaultCampusIdentifiers = new List<OrganisationCampusIdentifier>();
        private readonly List<OrganisationPostcodeSpecialistResource> _defaultPostcodeSepcialistResources = new List<OrganisationPostcodeSpecialistResource>();
        private readonly List<OrganisationShortTermFundingInitiative> _defaultShortTermFundingInitiatives = new List<OrganisationShortTermFundingInitiative>();
        private readonly IDbContextFactory<IOrganisationsContext> _organisationsFactory;

        public OrganisationsRepositoryService(IDbContextFactory<IOrganisationsContext> organisationsFactory)
        {
            _organisationsFactory = organisationsFactory;
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

                return await context
                    .MasterOrganisations
                      .Include(mo => mo.OrgDetail)
                      .Include(mo => mo.OrgPartnerUkprns)
                      .Include(mo => mo.OrgFundings)
                      .Where(mo => ukprns.Contains(mo.Ukprn))
                      .Select(
                          o => new Organisation
                          {
                              UKPRN = (int)o.Ukprn,
                              Name = o.OrgDetail.Name,
                              LegalOrgType = o.OrgDetail.LegalOrgType,
                              PartnerUKPRN = o.OrgPartnerUkprns.Any(op => op.Ukprn == o.Ukprn),
                              LongTermResid = o.OrgDetail.LongTermResid == LongTermResidValue,
                              CampusIdentifers = GetCampusIdentifiers(o.Ukprn, campusIdentifiersDictionary),
                              PostcodeSpecialistResources = GetPostcodeSpecResources(o.Ukprn, postcodeSpecResourcesDictionary),
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
                              OrganisationShortTermFundingInitiatives = GetShortTermFundingInitiatives(o.Ukprn, shortTermFundingInitiativesDictionary),
                          }).ToListAsync(cancellationToken);
            }
        }

        public List<OrganisationCampusIdentifier> GetCampusIdentifiers(long ukprn, Dictionary<long, List<OrganisationCampusIdentifier>> campusIdentifiers)
        {
            campusIdentifiers.TryGetValue(ukprn, out var campusIds);

            return campusIds ?? _defaultCampusIdentifiers;
        }

        public List<OrganisationPostcodeSpecialistResource> GetPostcodeSpecResources(long ukprn, Dictionary<long, List<OrganisationPostcodeSpecialistResource>> specResourcesDictionary)
        {
            specResourcesDictionary.TryGetValue(ukprn, out var specRes);

            return specRes ?? _defaultPostcodeSepcialistResources;
        }

        public List<OrganisationShortTermFundingInitiative> GetShortTermFundingInitiatives(long ukprn, Dictionary<long, List<OrganisationShortTermFundingInitiative>> shortTermFundingInitiativesDictionary)
        {
            shortTermFundingInitiativesDictionary.TryGetValue(ukprn, out var shortTermFundingInitiative);

            return shortTermFundingInitiative ?? _defaultShortTermFundingInitiatives;
        }

        private async Task<Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>>> BuildCampusIdSpecResourceDictionary(
            List<long> ukprns,
            IOrganisationsContext context,
            CancellationToken cancellationToken)
        {
            var campusIdentifierSpecResources = await context
            .CampusIdentifierSpecResources
             .Where(c => ukprns.Contains(c.MasterUkprn))
            .ToListAsync(cancellationToken);

            return campusIdentifierSpecResources?
               .Where(c => ukprns.Contains(c.MasterUkprn))
               .GroupBy(sr => sr.MasterUkprn)
               .ToDictionary(
                  k1 => k1.Key,
                  v1 => v1.GroupBy(ci => ci.CampusIdentifier)
                  .ToDictionary(
                      k2 => k2.Key,
                      v2 => v2.Select(sr => new OrganisationCampusIdSpecialistResource
                      {
                          IsSpecialistResource = sr.SpecialistResources,
                          EffectiveFrom = sr.EffectiveFrom,
                          EffectiveTo = sr.EffectiveTo
                      }).ToList()));
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
              .Select(ci => new OrganisationCampusIdentifier
              {
                  UKPRN = ci.MasterUkprn,
                  CampusIdentifier = ci.CampusIdentifier1,
                  EffectiveFrom = ci.EffectiveFrom,
                  EffectiveTo = ci.EffectiveTo,
                  SpecialistResources = GetCampusIdSpecialistResources(ci.MasterUkprn, ci.CampusIdentifier1, specResourcesForUkprnDictionary).ToList()
              })
              .ToListAsync(cancellationToken);

            return
                campusIdentifiers
                .GroupBy(ci => ci.UKPRN)
                .ToDictionary(
                k => k.Key,
                v => v.Select(c => c).ToList());
        }

        private async Task<Dictionary<long, List<OrganisationPostcodeSpecialistResource>>> BuildPostcodeSpecResDictionary(List<long> ukprns, IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var specResources = await context
                .ProviderPostcodeSpecialistResources
                .Where(c => ukprns.Contains(c.Ukprn))
                .ToListAsync(cancellationToken);

            return specResources?
               .GroupBy(sr => sr.Ukprn)
               .ToDictionary(
                  k1 => k1.Key,
                  v1 => v1.Select(sr => new OrganisationPostcodeSpecialistResource
                  {
                      UKPRN = sr.Ukprn,
                      Postcode = sr.Postcode,
                      SpecialistResources = sr.SpecialistResources,
                      EffectiveFrom = sr.EffectiveFrom,
                      EffectiveTo = sr.EffectiveTo
                  }).ToList());
        }

        private IEnumerable<OrganisationCampusIdSpecialistResource> GetCampusIdSpecialistResources(long ukprn, string campusIdentifier, Dictionary<long, Dictionary<string, List<OrganisationCampusIdSpecialistResource>>> specResourcesDictionary)
        {
            specResourcesDictionary.TryGetValue(ukprn, out var campusIdSpecResources);

            return campusIdSpecResources != null ?
                  campusIdSpecResources.TryGetValue(campusIdentifier, out var resources) ? resources : Enumerable.Empty<OrganisationCampusIdSpecialistResource>() : Enumerable.Empty<OrganisationCampusIdSpecialistResource>();
        }

        private async Task<Dictionary<long, List<OrganisationShortTermFundingInitiative>>> BuildShortTermFundingInitiativesDictionary(List<long> ukprns, IOrganisationsContext context, CancellationToken cancellationToken)
        {
            var shortTermFundingInitiatives = await context
                .ShortTermFundingInitiatives
                .Where(c => ukprns.Contains(c.Ukprn))
                .ToListAsync(cancellationToken);

            return shortTermFundingInitiatives?
                .GroupBy(stfi => stfi.Ukprn)
                .ToDictionary(
                    k1 => k1.Key,
                    v1 => v1.Select(stfi => new OrganisationShortTermFundingInitiative()
                    {
                        UKPRN = stfi.Ukprn,
                        LdmCode = stfi.Ldmcode,
                        Reason = stfi.Reason,
                        EffectiveFrom = stfi.EffectiveFrom,
                        EffectiveTo = stfi.EffectiveTo
                    }).ToList());
        }
    }
}
