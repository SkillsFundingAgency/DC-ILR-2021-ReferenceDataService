using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public sealed class LARSLearningDeliveryCategory
    {
       public string LearnAimRef { get; set; }

        public int CategoryRef { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
