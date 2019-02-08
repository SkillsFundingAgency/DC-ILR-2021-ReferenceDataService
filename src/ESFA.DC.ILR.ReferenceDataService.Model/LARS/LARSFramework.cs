using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFramework
    {
        public int FworkCode { get; set; }

        public int ProgType { get; set; }

        public int PwayCode { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
