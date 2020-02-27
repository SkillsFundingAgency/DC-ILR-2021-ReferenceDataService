using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsEsfEligibilityRule
    {
        public FcsEsfEligibilityRule()
        {
            FcsEsfEligibilityRuleEmploymentStatuses = new HashSet<FcsEsfEligibilityRuleEmploymentStatus>();
            FcsEsfEligibilityRuleLocalAuthorities = new HashSet<FcsEsfEligibilityRuleLocalAuthority>();
            FcsEsfEligibilityRuleLocalEnterprisePartnerships = new HashSet<FcsEsfEligibilityRuleLocalEnterprisePartnership>();
            FcsEsfEligibilityRuleSectorSubjectAreaLevels = new HashSet<FcsEsfEligibilityRuleSectorSubjectAreaLevel>();
            FcsFcsContractAllocations = new HashSet<FcsFcsContractAllocation>();
        }

        public int Id { get; set; }
        public bool? Benefits { get; set; }
        public int? CalcMethod { get; set; }
        public string TenderSpecReference { get; set; }
        public string LotReference { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? MinLengthOfUnemployment { get; set; }
        public int? MaxLengthOfUnemployment { get; set; }
        public string MinPriorAttainment { get; set; }
        public string MaxPriorAttainment { get; set; }

        public virtual ICollection<FcsEsfEligibilityRuleEmploymentStatus> FcsEsfEligibilityRuleEmploymentStatuses { get; set; }
        public virtual ICollection<FcsEsfEligibilityRuleLocalAuthority> FcsEsfEligibilityRuleLocalAuthorities { get; set; }
        public virtual ICollection<FcsEsfEligibilityRuleLocalEnterprisePartnership> FcsEsfEligibilityRuleLocalEnterprisePartnerships { get; set; }
        public virtual ICollection<FcsEsfEligibilityRuleSectorSubjectAreaLevel> FcsEsfEligibilityRuleSectorSubjectAreaLevels { get; set; }
        public virtual ICollection<FcsFcsContractAllocation> FcsFcsContractAllocations { get; set; }
    }
}
