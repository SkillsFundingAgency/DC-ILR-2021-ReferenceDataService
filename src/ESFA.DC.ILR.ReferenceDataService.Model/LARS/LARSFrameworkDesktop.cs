using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFrameworkDesktop : AbstractLARSFramework
    {
        public List<LARSFrameworkAim> LARSFrameworkAims { get; set; }
    }
}
