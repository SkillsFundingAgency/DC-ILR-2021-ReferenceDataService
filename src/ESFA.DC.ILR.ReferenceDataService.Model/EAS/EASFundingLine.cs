using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.EAS
{
    public class EASFundingLine
    {
        public string FundLine { get; set; }

        public IReadOnlyCollection<EasSubmissionValue> EasSubmissionValues { get; set; }
    }
}
