using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesSfaAreaCost
    {
        public int Id { get; set; }
        public decimal AreaCostFactor { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? PostcodesPostcodeId { get; set; }

        public virtual PostcodesPostcode PostcodesPostcode { get; set; }
    }
}
