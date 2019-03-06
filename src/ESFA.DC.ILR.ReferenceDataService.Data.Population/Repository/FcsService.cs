using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class FcsService : IRetrievalService<IReadOnlyDictionary<string, FcsContractAllocation>, int>
    {
        private readonly IFcsContext _fcs;

        public FcsService()
        {
        }

        public FcsService(IFcsContext fcs)
        {
            _fcs = fcs;
        }

        public async Task<IReadOnlyDictionary<string, FcsContractAllocation>> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            var contractAllocations = await _fcs.ContractAllocations
                .Include(ca => ca.ContractDeliverables)
                .Where(ca => ca.DeliveryUkprn == ukprn)
                .Select(ca => new FcsContractAllocation
                {
                    ContractAllocationNumber = ca.ContractAllocationNumber,
                    FundingStreamPeriodCode = ca.FundingStreamPeriodCode,
                    LotReference = ca.LotReference,
                    TenderSpecReference = ca.TenderSpecReference,
                    StartDate = ca.StartDate,
                    EndDate = ca.EndDate,
                    DeliveryUKPRN = ca.DeliveryUkprn.Value, // <= this will not be null on feed
                    FCSContractDeliverables = ca.ContractDeliverables.Select(cd => new FcsContractDeliverable
                    {
                        DeliverableCode = cd.DeliverableCode,
                        DeliverableDescription = cd.Description,
                        PlannedValue = cd.PlannedValue,
                        PlannedVolume = cd.PlannedVolume,
                        UnitCost = cd.UnitCost,
                        ExternalDeliverableCode = _fcs.ContractDeliverableCodeMappings
                        .Where(dc =>
                            dc.FundingStreamPeriodCode == ca.FundingStreamPeriodCode
                            && dc.FcsdeliverableCode == cd.DeliverableCode.ToString())
                        .Select(e => e.ExternalDeliverableCode).FirstOrDefault()
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            var eligibilityRules = await _fcs.EsfEligibilityRules
                .Include(e => e.EsfEligibilityRuleEmploymentStatuses)
                .Include(e => e.EsfEligibilityRuleLocalAuthorities)
                .Include(e => e.EsfEligibilityRuleLocalEnterprisePartnerships)
                .Include(e => e.EsfEligibilityRuleSectorSubjectAreaLevels)
               .Select(r => new EsfEligibilityRule
               {
                   LotReference = r.LotReference,
                   TenderSpecReference = r.TenderSpecReference,
                   MinAge = r.MinAge,
                   MaxAge = r.MaxAge,
                   Benefits = r.Benefits ?? false,
                   CalcMethod = r.CalcMethod,
                   MinLengthOfUnemployment = r.MinLengthOfUnemployment,
                   MaxLengthOfUnemployment = r.MaxLengthOfUnemployment,
                   MinPriorAttainment = r.MinPriorAttainment,
                   MaxPriorAttainment = r.MaxPriorAttainment,
                   EmploymentStatuses = r.EsfEligibilityRuleEmploymentStatuses
                   .Select(s => new EsfEligibilityRuleEmploymentStatus
                   {
                       Code = s.Code,
                   })
                   .ToList(),
                   LocalAuthorities = r.EsfEligibilityRuleLocalAuthorities
                   .Select(a => new EsfEligibilityRuleLocalAuthority
                   {
                       Code = a.Code,
                   })
                   .ToList(),
                   LocalEnterprisePartnerships = r.EsfEligibilityRuleLocalEnterprisePartnerships
                   .Select(p => new EsfEligibilityRuleLocalEnterprisePartnership
                   {
                       Code = p.Code,
                   })
                   .ToList(),
                   SectorSubjectAreaLevels = r.EsfEligibilityRuleSectorSubjectAreaLevels
                   .Select(l => new EsfEligibilityRuleSectorSubjectAreaLevel
                   {
                       MaxLevelCode = l.MaxLevelCode,
                       MinLevelCode = l.MinLevelCode,
                       SectorSubjectAreaCode = l.SectorSubjectAreaCode,
                   })
                   .ToList()
               }).ToListAsync(cancellationToken);

            foreach (var contractAllocation in contractAllocations)
            {
                contractAllocation.EsfEligibilityRule = eligibilityRules
                    .SingleOrDefault(
                        r => r.LotReference.Equals(contractAllocation.LotReference, StringComparison.OrdinalIgnoreCase)
                        && r.TenderSpecReference.Equals(contractAllocation.TenderSpecReference, StringComparison.OrdinalIgnoreCase));
            }

           return contractAllocations
                .ToDictionary(ca => ca.ContractAllocationNumber, ca => ca, StringComparer.OrdinalIgnoreCase);
        }
    }
}
