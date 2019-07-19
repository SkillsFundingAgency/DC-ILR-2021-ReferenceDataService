using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSAnnualValue : AbstractTimeBoundedEntity
    {
        public string LearnAimRef { get; set; }

        public int? BasicSkills { get; set; }

        public int? BasicSkillsType { get; set; }

        public int? FullLevel2EntitlementCategory { get; set; }

        public int? FullLevel3EntitlementCategory { get; set; }

        public decimal? FullLevel2Percent { get; set; }

        public decimal? FullLevel3Percent { get; set; }
    }
}
