using System;
using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFramework
    {
        public DateTime? EffectiveFromNullable { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public List<LARSFrameworkCommonComponent> LARSFrameworkCommonComponents { get; set; }

        public List<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFundings { get; set; }
    }
}
