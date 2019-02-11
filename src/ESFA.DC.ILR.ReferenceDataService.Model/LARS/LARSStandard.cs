using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSStandard
    {
        public int StandardCode { get; set; }

        public string StandardSectorCode { get; set; }

        public string NotionalEndLevel { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public List<LARSStandardApprenticeshipFunding> LARSStandardApprenticeshipFundings { get; set; }

        public List<LARSStandardCommonComponent> LARSStandardCommonComponents { get; set; }

        public List<LARSStandardFunding> LARSStandardFundings { get; set; }

        public List<LARSStandardValidity> LARSStandardValidities { get; set; }
    }
}
