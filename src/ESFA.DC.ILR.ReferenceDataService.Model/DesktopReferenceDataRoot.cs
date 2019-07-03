using System;
using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;

namespace ESFA.DC.ILR.ReferenceDataService.Model
{
    public class DesktopReferenceDataRoot : ReferenceDataRoot
    {
        public DateTime DateGenerated { get; set; }

        public IReadOnlyCollection<LARSFramework> LARSFrameworks { get; set; }
    }
}
