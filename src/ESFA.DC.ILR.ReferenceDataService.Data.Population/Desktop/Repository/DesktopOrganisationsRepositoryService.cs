using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
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
            var campusIdentifiers = await _organisations
                .CampusIdentifiers
                .GroupBy(mu => mu.MasterUkprn)
                .ToDictionaryAsync(k => k.Key, v => v.Select(c => c.CampusIdentifier1).ToList(), cancellationToken) ?? new Dictionary<long, List<string>>();

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
