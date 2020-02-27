using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDasDisadvantage
    {
        public int Id { get; set; }
        public decimal? Uplift { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Postcodes_Postcode_Id { get; set; }

        public virtual PostcodesPostcode Postcodes_Postcode_ { get; set; }
    }
}
