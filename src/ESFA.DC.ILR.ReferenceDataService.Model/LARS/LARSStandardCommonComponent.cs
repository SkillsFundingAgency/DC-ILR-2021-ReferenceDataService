using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSStandardCommonComponent
    {
        public int CommonComponent { get; set; }

        public int StandardCode { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
