using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsEsfEligibilityRuleEmploymentStatus
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int? FcsEsfEligibilityRuleId { get; set; }

        public virtual FcsEsfEligibilityRule FcsEsfEligibilityRule { get; set; }
    }
}
