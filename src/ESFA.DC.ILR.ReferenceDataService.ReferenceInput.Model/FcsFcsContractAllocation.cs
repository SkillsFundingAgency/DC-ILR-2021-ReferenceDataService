using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsFcsContractAllocation
    {
        public FcsFcsContractAllocation()
        {
            FcsFcsContractDeliverable = new HashSet<FcsFcsContractDeliverable>();
        }

        public int Id { get; set; }
        public string ContractAllocationNumber { get; set; }
        public int DeliveryUkprn { get; set; }
        public decimal? LearningRatePremiumFactor { get; set; }
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public string FundingStreamPeriodCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StopNewStartsFromDate { get; set; }
        public int? EsfEligibilityRuleId { get; set; }

        public virtual FcsEsfEligibilityRule EsfEligibilityRule { get; set; }
        public virtual ICollection<FcsFcsContractDeliverable> FcsFcsContractDeliverable { get; set; }
    }
}
