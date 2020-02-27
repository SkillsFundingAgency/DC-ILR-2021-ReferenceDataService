using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsFcsContractAllocation
    {
        public FcsFcsContractAllocation()
        {
            FcsFcsContractDeliverables = new HashSet<FcsFcsContractDeliverable>();
        }

        public int Id { get; set; }
        public string ContractAllocationNumber { get; set; }
        public int DeliveryUKPRN { get; set; }
        public decimal? LearningRatePremiumFactor { get; set; }
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public string FundingStreamPeriodCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StopNewStartsFromDate { get; set; }
        public int? EsfEligibilityRule_Id { get; set; }

        public virtual FcsEsfEligibilityRule EsfEligibilityRule_ { get; set; }
        public virtual ICollection<FcsFcsContractDeliverable> FcsFcsContractDeliverables { get; set; }
    }
}
