using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Postcodes.Abstract
{
    public class AbstractDisadvantage
    {
        public decimal? Uplift { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
