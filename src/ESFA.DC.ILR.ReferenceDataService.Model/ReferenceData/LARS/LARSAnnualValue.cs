using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS
{
    public class LARSAnnualValue : AbstractTimeBoundedEntity
    {
        public int? BasicSkills { get; set; }

        public int? BasicSkillsType { get; set; }

        public int? FullLevel2EntitlementCategory { get; set; }

        public int? FullLevel3EntitlementCategory { get; set; }

        public decimal? FullLevel3Percent { get; set; }
    }
}
