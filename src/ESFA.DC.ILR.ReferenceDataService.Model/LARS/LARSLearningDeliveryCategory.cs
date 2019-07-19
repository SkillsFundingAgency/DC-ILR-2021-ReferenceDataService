using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public sealed class LARSLearningDeliveryCategory : AbstractTimeBoundedEntity
    {
        public string LearnAimRef { get; set; }

        public int CategoryRef { get; set; }
    }
}
