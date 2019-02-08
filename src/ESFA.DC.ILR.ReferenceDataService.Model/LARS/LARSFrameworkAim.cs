using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFrameworkAim
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public int? FrameworkComponentType { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public List<LARSFramework> LARSFrameworks { get; set; }

        public List<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFundings { get; set; }
    }
}
