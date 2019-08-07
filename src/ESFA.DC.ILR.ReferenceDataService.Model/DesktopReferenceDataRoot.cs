using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Model
{
    public class DesktopReferenceDataRoot : ReferenceDataRoot
    {
        public IReadOnlyCollection<LARSFrameworkDesktop> LARSFrameworks { get; set; }
    }
}
