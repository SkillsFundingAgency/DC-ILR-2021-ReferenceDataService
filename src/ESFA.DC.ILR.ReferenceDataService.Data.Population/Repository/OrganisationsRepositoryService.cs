using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class OrganisationsRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>>
    {
        private readonly IOrganisationsContext _organisations;

        public OrganisationsRepositoryService(IOrganisationsContext organisations)
        {
            _organisations = organisations;
        }

        public async Task<IReadOnlyCollection<Organisation>> RetrieveAsync(IReadOnlyCollection<int> ukprnsInput, CancellationToken cancellationToken)
        {
            var ukprns = ukprnsInput.Select(u => (long)u).ToList();

            var campusIdentifiers = await _organisations
                .CampusIdentifiers
                .Where(c => ukprns.Contains(c.MasterUkprn))
                .GroupBy(mu => mu.MasterUkprn)
                .ToDictionaryAsync(k => k.Key, v => v.Select(c => c.CampusIdentifier1).ToList(), cancellationToken) ?? new Dictionary<long, List<string>>();

            return await _organisations
                .MasterOrganisations
                  .Include(mo => mo.OrgDetail)
                  .Include(mo => mo.OrgPartnerUkprns)
                  .Include(mo => mo.OrgFundings)
                  .Where(mo => ukprns.Contains(mo.Ukprn))
                  .Select(
                      o => new Organisation
                      {
                          UKPRN = (int)o.Ukprn,
                          LegalOrgType = o.OrgDetail.LegalOrgType,
                          PartnerUKPRN = o.OrgPartnerUkprns.Any(op => op.Ukprn == o.Ukprn),
                          CampusIdentifers = GetCampusIdentifiers(o.Ukprn, campusIdentifiers),
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

        public List<string> GetCampusIdentifiers(long ukprn, Dictionary<long, List<string>> campusIdentifiers)
        {
            campusIdentifiers.TryGetValue(ukprn, out var campusIds);

            return campusIds ?? new List<string>();
        }
    }
}
