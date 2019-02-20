using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.Organisation;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.ReferenceData
{
    public class OrganisationsDataRetrievalService : IOrganisationsDataRetrievalService
    {
        private readonly IOrganisationsContext _organisations;

        public OrganisationsDataRetrievalService()
        {
        }

        public OrganisationsDataRetrievalService(IOrganisationsContext organisations)
        {
            _organisations = organisations;
        }

        public async Task<IReadOnlyDictionary<long, Organisation>> RetrieveAsync(IReadOnlyCollection<int> ukprnsInput, CancellationToken cancellationToken)
        {
            var ukprns = ukprnsInput.Select(u => (long)u).ToList();

            var campusIdentifiers =
                await _organisations
                .CampusIdentifiers
                .Where(c => ukprns.Contains(c.MasterUkprn))
                .GroupBy(mu => mu.MasterUkprn)
                .ToDictionaryAsync(k => k.Key, v => v.Select(c => c.CampusIdentifier1).ToList(), cancellationToken);

            return await Task.Run(
                () =>
                {
                    return _organisations
                          .MasterOrganisations
                          .Include(mo => mo.OrgDetail)
                          .Include(mo => mo.OrgPartnerUkprns)
                          .Include(mo => mo.OrgFundings)
                          .Where(mo => ukprns.Contains(mo.Ukprn))
                          .ToDictionary(
                              o => o.Ukprn,
                              o => new Organisation
                              {
                                  UKPRN = o.Ukprn,
                                  LegalOrgType = o.OrgDetail?.LegalOrgType,
                                  PartnerUKPRN = o.OrgPartnerUkprns?.Any(op => op.Ukprn == o.Ukprn),
                                  CampusIdentifers = GetCampusIdentifiers(o.Ukprn, campusIdentifiers),
                                  OrganisationFundings = o.OrgFundings.Where(of => ukprns.Contains(of.Ukprn)).Select(OrgFundingFromEntity).ToList(),
                              });
                }, cancellationToken);
        }

        public List<string> GetCampusIdentifiers(long ukprn, Dictionary<long, List<string>> campusIdentifiers)
        {
            campusIdentifiers.TryGetValue(ukprn, out var campusIds);

            return campusIds;
        }

        public OrganisationFunding OrgFundingFromEntity(OrgFunding entity)
        {
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
