using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class OrganisationsRepositoryService : IOrganisationsRepositoryService
    {
        private readonly IOrganisationsContext _organisations;

        public OrganisationsRepositoryService(IOrganisationsContext organisations)
        {
            _organisations = organisations;
        }

        public async Task<IReadOnlyDictionary<int, Organisation>> RetrieveAsync(IReadOnlyCollection<int> ukprnsInput, CancellationToken cancellationToken)
        {
            var ukprns = ukprnsInput.Select(u => (long)u).ToList();

            var campusIdentifiers = await _organisations
                .CampusIdentifiers
                .Where(c => ukprns.Contains(c.MasterUkprn))
                .GroupBy(mu => mu.MasterUkprn)
                .ToDictionaryAsync(k => k.Key, v => v.Select(c => c.CampusIdentifier1).ToList(), cancellationToken) ?? new Dictionary<long, List<string>>();

            var masters = await _organisations
              .MasterOrganisations
                .Where(mo => ukprns.Contains(mo.Ukprn)).ToListAsync(cancellationToken);

          var organisations = await _organisations
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
                          OrganisationFundings = o.OrgFundings.Select(of => OrgFundingFromEntity(of)).ToList(),
                      })
                      .ToListAsync(cancellationToken);

            return organisations.
                GroupBy(o => o.UKPRN)
                .ToDictionary(key => key.Key, value => value.FirstOrDefault());
        }

        public List<string> GetCampusIdentifiers(long ukprn, Dictionary<long, List<string>> campusIdentifiers)
        {
            campusIdentifiers.TryGetValue(ukprn, out var campusIds);

            return campusIds;
        }

        public OrganisationFunding OrgFundingFromEntity(OrgFunding entity)
        {
            if (entity == null)
            {
                return new OrganisationFunding();
            }

            return new OrganisationFunding()
            {
                OrgFundFactor = entity.FundingFactor,
                OrgFundFactType = entity.FundingFactorType,
                OrgFundFactValue = entity.FundingFactorValue,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
            };
        }
    }
}
