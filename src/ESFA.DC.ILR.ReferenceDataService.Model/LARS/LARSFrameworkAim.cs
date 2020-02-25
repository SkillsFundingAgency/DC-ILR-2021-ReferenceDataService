using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFrameworkAim : AbstractTimeBoundedEntity
    {
        public string LearnAimRef { get; set; }

        public int? FrameworkComponentType { get; set; }
    }
}
