using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataLookupSubCategory
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? MetaDataLookupId { get; set; }

        public virtual MetaDataLookup MetaDataLookup { get; set; }
    }
}
