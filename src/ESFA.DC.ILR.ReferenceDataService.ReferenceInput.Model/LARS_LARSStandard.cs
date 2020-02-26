using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSStandard
    {
        public LARS_LARSStandard()
        {
            LARS_LARSStandardApprenticeshipFundings = new HashSet<LARS_LARSStandardApprenticeshipFunding>();
            LARS_LARSStandardCommonComponents = new HashSet<LARS_LARSStandardCommonComponent>();
            LARS_LARSStandardFundings = new HashSet<LARS_LARSStandardFunding>();
            LARS_LARSStandardValidities = new HashSet<LARS_LARSStandardValidity>();
        }

        public int Id { get; set; }
        public int StandardCode { get; set; }
        public string StandardSectorCode { get; set; }
        public string NotionalEndLevel { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<LARS_LARSStandardApprenticeshipFunding> LARS_LARSStandardApprenticeshipFundings { get; set; }
        public virtual ICollection<LARS_LARSStandardCommonComponent> LARS_LARSStandardCommonComponents { get; set; }
        public virtual ICollection<LARS_LARSStandardFunding> LARS_LARSStandardFundings { get; set; }
        public virtual ICollection<LARS_LARSStandardValidity> LARS_LARSStandardValidities { get; set; }
    }
}
