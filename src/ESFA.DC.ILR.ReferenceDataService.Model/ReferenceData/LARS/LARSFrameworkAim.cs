using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS
{
    public class LARSFrameworkAim : AbstractTimeBoundedEntity
    {
        public int? FrameworkComponentType { get; set; }

        public LARSFramework LARSFramework { get; set; }

        public List<LARSFrameworkApprenticeshipFunding> LARSFrameworkApprenticeshipFundings { get; set; }
    }
}
