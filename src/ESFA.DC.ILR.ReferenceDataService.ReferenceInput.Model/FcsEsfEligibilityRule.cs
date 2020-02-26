using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsEsfEligibilityRule
    {
        public FcsEsfEligibilityRule()
        {
            FcsEsfEligibilityRuleEmploymentStatus = new HashSet<FcsEsfEligibilityRuleEmploymentStatus>();
            FcsEsfEligibilityRuleLocalAuthority = new HashSet<FcsEsfEligibilityRuleLocalAuthority>();
            FcsEsfEligibilityRuleLocalEnterprisePartnership = new HashSet<FcsEsfEligibilityRuleLocalEnterprisePartnership>();
            FcsEsfEligibilityRuleSectorSubjectAreaLevel = new HashSet<FcsEsfEligibilityRuleSectorSubjectAreaLevel>();
            FcsFcsContractAllocation = new HashSet<FcsFcsContractAllocation>();
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

        public virtual ICollection<FcsEsfEligibilityRuleEmploymentStatus> FcsEsfEligibilityRuleEmploymentStatus { get; set; }
        public virtual ICollection<FcsEsfEligibilityRuleLocalAuthority> FcsEsfEligibilityRuleLocalAuthority { get; set; }
        public virtual ICollection<FcsEsfEligibilityRuleLocalEnterprisePartnership> FcsEsfEligibilityRuleLocalEnterprisePartnership { get; set; }
        public virtual ICollection<FcsEsfEligibilityRuleSectorSubjectAreaLevel> FcsEsfEligibilityRuleSectorSubjectAreaLevel { get; set; }
        public virtual ICollection<FcsFcsContractAllocation> FcsFcsContractAllocation { get; set; }
    }
}
