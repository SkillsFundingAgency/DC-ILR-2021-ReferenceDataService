using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class FcsRepositoryService : IReferenceDataRetrievalService<int, IReadOnlyCollection<FcsContractAllocation>>
    {
        private readonly IDbContextFactory<IFcsContext> _fcsContextFactory;

        public FcsRepositoryService(IDbContextFactory<IFcsContext> fcsContextFactory)
        {
            _fcsContextFactory = fcsContextFactory;
        }

        public async Task<IReadOnlyCollection<FcsContractAllocation>> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            using (var context = _fcsContextFactory.Create())
            {
                var contractAllocations = await context.ContractAllocations
                .Include(ca => ca.ContractDeliverables)
                .Where(ca => ca.DeliveryUkprn == ukprn)
                .Select(ca => new FcsContractAllocation
                {
                    ContractAllocationNumber = ca.ContractAllocationNumber.ToUpperCase(),
                    FundingStreamPeriodCode = ca.FundingStreamPeriodCode,
                    LearningRatePremiumFactor = ca.LearningRatePremiumFactor,
                    LotReference = ca.LotReference,
                    TenderSpecReference = ca.TenderSpecReference,
                    StartDate = ca.StartDate,
                    EndDate = ca.EndDate,
                    StopNewStartsFromDate = ca.StopNewStartsFromDate,
                    DeliveryUKPRN = ca.DeliveryUkprn.Value, // <= this will not be null on feed
                    FCSContractDeliverables = ca.ContractDeliverables.Select(cd => new FcsContractDeliverable
                    {
                        DeliverableCode = cd.DeliverableCode,
                        DeliverableDescription = cd.Description,
                        PlannedValue = cd.PlannedValue,
                        PlannedVolume = cd.PlannedVolume,
                        UnitCost = cd.UnitCost,
                        ExternalDeliverableCode = context.ContractDeliverableCodeMappings
                        .Where(dc =>
                            dc.FundingStreamPeriodCode.Equals(ca.FundingStreamPeriodCode, StringComparison.OrdinalIgnoreCase)
                            && dc.FcsdeliverableCode.Equals(cd.DeliverableCode.ToString(), StringComparison.OrdinalIgnoreCase))
                        .Select(e => e.ExternalDeliverableCode).FirstOrDefault(),
                    }).ToList(),
                })
                .ToListAsync(cancellationToken);

                var eligibilityRules = await context.EsfEligibilityRules
                   .Select(r => new EsfEligibilityRule
                   {
                       LotReference = r.LotReference,
                       TenderSpecReference = r.TenderSpecReference,
                       MinAge = r.MinAge,
                       MaxAge = r.MaxAge,
                       Benefits = r.Benefits,
                       CalcMethod = r.CalcMethod,
                       MinLengthOfUnemployment = r.MinLengthOfUnemployment,
                       MaxLengthOfUnemployment = r.MaxLengthOfUnemployment,
                       MinPriorAttainment = r.MinPriorAttainment,
                       MaxPriorAttainment = r.MaxPriorAttainment,
                       EmploymentStatuses = r.EsfEligibilityRuleEmploymentStatuses
                       .Where(esf =>
                               esf.TenderSpecReference.Equals(r.TenderSpecReference, StringComparison.OrdinalIgnoreCase)
                               && esf.LotReference.Equals(r.LotReference, StringComparison.OrdinalIgnoreCase))
                       .Select(s => new EsfEligibilityRuleEmploymentStatus
                       {
                           Code = s.Code,
                       })
                       .ToList(),
                       LocalAuthorities = r.EsfEligibilityRuleLocalAuthorities
                       .Where(esf =>
                             esf.TenderSpecReference.Equals(r.TenderSpecReference, StringComparison.OrdinalIgnoreCase)
                             && esf.LotReference.Equals(r.LotReference, StringComparison.OrdinalIgnoreCase))
                       .Select(a => new EsfEligibilityRuleLocalAuthority
                       {
                           Code = a.Code,
                       })
                       .ToList(),
                       LocalEnterprisePartnerships = r.EsfEligibilityRuleLocalEnterprisePartnerships
                       .Where(esf =>
                             esf.TenderSpecReference.Equals(r.TenderSpecReference, StringComparison.OrdinalIgnoreCase)
                             && esf.LotReference.Equals(r.LotReference, StringComparison.OrdinalIgnoreCase))
                       .Select(p => new EsfEligibilityRuleLocalEnterprisePartnership
                       {
                           Code = p.Code,
                       })
                       .ToList(),
                       SectorSubjectAreaLevels = r.EsfEligibilityRuleSectorSubjectAreaLevels
                       .Where(esf =>
                             esf.TenderSpecReference.Equals(r.TenderSpecReference, StringComparison.OrdinalIgnoreCase)
                             && esf.LotReference.Equals(r.LotReference, StringComparison.OrdinalIgnoreCase))
                       .Select(l => new EsfEligibilityRuleSectorSubjectAreaLevel
                       {
                           MaxLevelCode = l.MaxLevelCode,
                           MinLevelCode = l.MinLevelCode,
                           SectorSubjectAreaCode = l.SectorSubjectAreaCode,
                       })
                       .ToList(),
                   }).ToListAsync(cancellationToken);

                foreach (var contractAllocation in contractAllocations)
                {
                    contractAllocation.EsfEligibilityRule = eligibilityRules
                        .FirstOrDefault(
                            r => r.LotReference.Equals(contractAllocation.LotReference, StringComparison.OrdinalIgnoreCase)
                            && r.TenderSpecReference.Equals(contractAllocation.TenderSpecReference, StringComparison.OrdinalIgnoreCase));
                }

                return contractAllocations;
            }
        }
    }
}
