using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Abstract
{
    public class AbstractTimeBoundedEntity
    {
        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
