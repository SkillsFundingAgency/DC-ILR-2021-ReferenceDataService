using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsEsfEligibilityRuleEmploymentStatus
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int? FCS_EsfEligibilityRule_Id { get; set; }

        public virtual FcsEsfEligibilityRule FCS_EsfEligibilityRule_ { get; set; }
    }
}
