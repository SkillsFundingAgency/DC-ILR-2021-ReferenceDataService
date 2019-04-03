using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.FCS
{
    public class FcsContractAllocation
    {
        public string ContractAllocationNumber { get; set; }

        public int DeliveryUKPRN { get; set; }

        public decimal? LearningRatePremiumFactor { get; set; }

        public string TenderSpecReference { get; set; }

        public string LotReference { get; set; }

        public string FundingStreamPeriodCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<FcsContractDeliverable> FCSContractDeliverables { get; set; }

        public EsfEligibilityRule EsfEligibilityRule { get; set; }
    }
}
