using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Postcodes_DasDisadvantage
    {
        public int Id { get; set; }
        public decimal? Uplift { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Postcodes_Postcode_Id { get; set; }

        public virtual Postcodes_Postcode Postcodes_Postcode_ { get; set; }
    }
}
