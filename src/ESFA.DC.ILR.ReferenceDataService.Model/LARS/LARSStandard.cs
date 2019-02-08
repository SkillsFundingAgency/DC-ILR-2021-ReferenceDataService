using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSStandard
    {
        public int StandardCode { get; set; }

        public string StandardSectorCode { get; set; }

        public string NotionalEndLevel { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
