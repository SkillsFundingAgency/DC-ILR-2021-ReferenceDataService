using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFramework : AbstractTimeBoundedEntity
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public List<LARSFrameworkCommonComponent> LARSFrameworkCommonComponents { get; set; }
    }
}
