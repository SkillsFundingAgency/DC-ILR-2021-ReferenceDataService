using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSAnnualValue
    {
        public int Id { get; set; }
        public string LearnAimRef { get; set; }
        public int? BasicSkills { get; set; }
        public int? BasicSkillsType { get; set; }
        public int? FullLevel2EntitlementCategory { get; set; }
        public int? FullLevel3EntitlementCategory { get; set; }
        public decimal? FullLevel2Percent { get; set; }
        public decimal? FullLevel3Percent { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LARS_LARSLearningDelivery_Id { get; set; }

        public virtual LARS_LARSLearningDelivery LARS_LARSLearningDelivery_ { get; set; }
    }
}
