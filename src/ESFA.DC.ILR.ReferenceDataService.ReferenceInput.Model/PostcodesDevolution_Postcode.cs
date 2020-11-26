using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDevolution_Postcode
    {
        public int Id { get; set; }
        public string Postcode { get; set; }
        public string Area { get; set; }
        public string SourceOfFunding { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
