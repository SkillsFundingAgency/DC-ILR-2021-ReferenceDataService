using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsFcsContractDeliverable
    {
        public int Id { get; set; }
        public int? DeliverableCode { get; set; }
        public string DeliverableDescription { get; set; }
        public string ExternalDeliverableCode { get; set; }
        public decimal? UnitCost { get; set; }
        public int? PlannedVolume { get; set; }
        public decimal? PlannedValue { get; set; }
        public int? FcsFcsContractAllocationId { get; set; }

        public virtual FcsFcsContractAllocation FcsFcsContractAllocation { get; set; }
    }
}
