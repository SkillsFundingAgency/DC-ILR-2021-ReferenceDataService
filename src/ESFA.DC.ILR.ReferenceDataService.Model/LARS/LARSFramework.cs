using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFramework
    {
        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public List<LARSFrameworkCommonComponent> LARSFrameworkCommonComponents { get; set; }
    }
}
