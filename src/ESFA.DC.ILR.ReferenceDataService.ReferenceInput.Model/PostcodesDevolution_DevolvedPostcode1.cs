using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDevolution_DevolvedPostcode1
    {
        public int Id { get; set; }
        public string Postcode { get; set; }
        public string Area { get; set; }
        public string SourceOfFunding { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? PostcodesDevolution_DevolvedPostcodes_Id { get; set; }

        public virtual PostcodesDevolution_DevolvedPostcode PostcodesDevolution_DevolvedPostcodes_ { get; set; }
    }
}
