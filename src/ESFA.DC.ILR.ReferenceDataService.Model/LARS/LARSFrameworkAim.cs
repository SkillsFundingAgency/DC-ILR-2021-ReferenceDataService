using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFrameworkAim
    {
        public int? FrameworkComponentType { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public LARSFramework LARSFramework { get; set; }

        public List<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFundings { get; set; }
    }
}
