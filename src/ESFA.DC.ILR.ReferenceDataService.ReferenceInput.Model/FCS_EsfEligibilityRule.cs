using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FCS_EsfEligibilityRule
    {
        public FCS_EsfEligibilityRule()
        {
            FCS_EsfEligibilityRuleEmploymentStatuses = new HashSet<FCS_EsfEligibilityRuleEmploymentStatus>();
            FCS_EsfEligibilityRuleLocalAuthorities = new HashSet<FCS_EsfEligibilityRuleLocalAuthority>();
            FCS_EsfEligibilityRuleLocalEnterprisePartnerships = new HashSet<FCS_EsfEligibilityRuleLocalEnterprisePartnership>();
            FCS_EsfEligibilityRuleSectorSubjectAreaLevels = new HashSet<FCS_EsfEligibilityRuleSectorSubjectAreaLevel>();
            FCS_FcsContractAllocations = new HashSet<FCS_FcsContractAllocation>();
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

        public virtual ICollection<FCS_EsfEligibilityRuleEmploymentStatus> FCS_EsfEligibilityRuleEmploymentStatuses { get; set; }
        public virtual ICollection<FCS_EsfEligibilityRuleLocalAuthority> FCS_EsfEligibilityRuleLocalAuthorities { get; set; }
        public virtual ICollection<FCS_EsfEligibilityRuleLocalEnterprisePartnership> FCS_EsfEligibilityRuleLocalEnterprisePartnerships { get; set; }
        public virtual ICollection<FCS_EsfEligibilityRuleSectorSubjectAreaLevel> FCS_EsfEligibilityRuleSectorSubjectAreaLevels { get; set; }
        public virtual ICollection<FCS_FcsContractAllocation> FCS_FcsContractAllocations { get; set; }
    }
}
