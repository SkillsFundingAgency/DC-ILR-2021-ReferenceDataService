using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSStandardValidity
    {
        public int Id { get; set; }
        public string ValidityCategory { get; set; }
        public DateTime? LastNewStartDate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LARS_LARSStandard_Id { get; set; }

        public virtual LARS_LARSStandard LARS_LARSStandard_ { get; set; }
    }
}
