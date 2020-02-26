using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_Lookup
    {
        public MetaData_Lookup()
        {
            MetaData_LookupSubCategories = new HashSet<MetaData_LookupSubCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<MetaData_LookupSubCategory> MetaData_LookupSubCategories { get; set; }
    }
}
