using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataLookup
    {
        public MetaDataLookup()
        {
            MetaDataLookupSubCategories = new HashSet<MetaDataLookupSubCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<MetaDataLookupSubCategory> MetaDataLookupSubCategories { get; set; }
    }
}
