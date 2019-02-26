using ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS
{
    public class LARSStandardApprenticeshipFunding : AbstractLARSApprenticeshipFunding
    {
        public int ProgType { get; set; }

        public int? PwayCode { get; set; }
    }
}
