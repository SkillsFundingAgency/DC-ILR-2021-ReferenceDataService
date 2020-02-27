using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDevolutionMcaGlaSofLookup
    {
        public int Id { get; set; }
        public string SofCode { get; set; }
        public string McaGlaShortCode { get; set; }
        public string McaGlaFullName { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? PostcodesDevolution_DevolvedPostcodes_Id { get; set; }

        public virtual PostcodesDevolutionDevolvedPostcode PostcodesDevolution_DevolvedPostcodes_ { get; set; }
    }
}
