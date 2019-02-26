using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public sealed class LARSLearningDeliveryCategory : AbstractTimeBoundedEntity
    {
        public int CategoryRef { get; set; }
    }
}
