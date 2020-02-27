using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsEsfEligibilityRuleLocalEnterprisePartnership
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? FcsEsfEligibilityRuleId { get; set; }

        public virtual FcsEsfEligibilityRule FcsEsfEligibilityRule { get; set; }
    }
}
