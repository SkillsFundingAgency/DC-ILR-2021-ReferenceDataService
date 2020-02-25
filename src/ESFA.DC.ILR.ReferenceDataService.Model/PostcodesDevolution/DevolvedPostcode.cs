using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution
{
    public class DevolvedPostcode
    {
        public string Postcode { get; set; }

        public string Area { get; set; }

        public string SourceOfFunding { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
