using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Postcodes
{
    public class SfaAreaCost
    {
        public decimal AreaCostFactor { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
