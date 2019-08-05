﻿using System;
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
            var campusIdentifiers = await _organisations
               .CampusIdentifiers
               .ToListAsync(cancellationToken);

            var campusIdentifiersList = new List<OrganisationCampusIdentifier>();

            foreach (var ci in campusIdentifiers)
            {
                var specialistResource = ci.CampusIdentifierUkprn?.CampusIdentifierSpecResources?.Select(sr => new SpecialistResource
                {
                    IsSpecialistResource = sr.SpecialistResources,
                    EffectiveFrom = sr.EffectiveFrom,
                    EffectiveTo = sr.EffectiveTo,
                }).ToList() ?? new List<SpecialistResource>();

                campusIdentifiersList.Add(new OrganisationCampusIdentifier
                {
                    UKPRN = ci.MasterUkprn,
                    CampusIdentifier = ci.CampusIdentifier1,
                    EffectiveFrom = ci.EffectiveFrom,
                    EffectiveTo = ci.EffectiveTo,
                    SpecialistResources = specialistResource
                });
            }

            var campusIdentifiersDictionary =
                campusIdentifiersList
                .GroupBy(ci => ci.UKPRN)
                .ToDictionary(
                k => k.Key,
                v => v.Select(c => c).ToList());

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
    }
}
