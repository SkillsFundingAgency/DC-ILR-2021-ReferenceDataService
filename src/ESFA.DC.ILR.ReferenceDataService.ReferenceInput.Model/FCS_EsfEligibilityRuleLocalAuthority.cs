namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FCS_EsfEligibilityRuleLocalAuthority
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? FCS_EsfEligibilityRule_Id { get; set; }

        public virtual FCS_EsfEligibilityRule FCS_EsfEligibilityRule_ { get; set; }
    }
}
