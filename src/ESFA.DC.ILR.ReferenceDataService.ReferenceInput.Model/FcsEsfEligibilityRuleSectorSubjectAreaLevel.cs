using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FcsEsfEligibilityRuleSectorSubjectAreaLevel
    {
        public int Id { get; set; }
        public decimal? SectorSubjectAreaCode { get; set; }
        public string MinLevelCode { get; set; }
        public string MaxLevelCode { get; set; }
        public int? FCS_EsfEligibilityRule_Id { get; set; }

        public virtual FcsEsfEligibilityRule FCS_EsfEligibilityRule_ { get; set; }
    }
}
