using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSCareerLearningPilot : AbstractTimeBoundedEntity
    {
        public string LearnAimRef { get; set; }

        public string AreaCode { get; set; }

        public decimal? SubsidyRate { get; set; }
    }
}
