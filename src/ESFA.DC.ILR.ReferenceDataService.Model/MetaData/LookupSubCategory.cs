using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public class LookupSubCategory
    {
        public string Code { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
