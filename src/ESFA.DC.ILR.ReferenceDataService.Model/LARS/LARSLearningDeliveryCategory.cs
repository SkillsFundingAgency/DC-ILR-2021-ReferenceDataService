using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public sealed class LARSLearningDeliveryCategory
    {
        public int CategoryRef { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
