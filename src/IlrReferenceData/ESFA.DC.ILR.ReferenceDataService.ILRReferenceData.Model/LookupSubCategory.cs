using System;

namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model
{
    public partial class LookupSubCategory
    {
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual Lookup Lookup { get; set; }
    }
}
