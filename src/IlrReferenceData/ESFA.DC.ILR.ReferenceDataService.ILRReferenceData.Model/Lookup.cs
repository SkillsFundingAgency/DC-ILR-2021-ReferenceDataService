using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model
{
    public partial class Lookup
    {
        public Lookup()
        {
            LookupSubCategories = new HashSet<LookupSubCategory>();
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<LookupSubCategory> LookupSubCategories { get; set; }
    }
}
