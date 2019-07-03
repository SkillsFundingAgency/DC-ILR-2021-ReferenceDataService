using System;
using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFramework
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public DateTime? EffectiveFromNullable { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public LARSFrameworkAim LARSFrameworkAim { get; set; }

        public List<LARSFrameworkCommonComponent> LARSFrameworkCommonComponents { get; set; }

        public List<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFundings { get; set; }
    }
}
