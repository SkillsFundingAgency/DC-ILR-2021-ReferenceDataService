using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.FRM
{
    public class Frm06Learner
    {
        public long UKPRN { get; set; }

        public string LearnRefNumber { get; set; }

        public string LearnAimRef { get; set; }

        public int? ProgTypeNullable { get; set; }

        public int? StdCodeNullable { get; set; }

        public int? FworkCodeNullable { get; set; }

        public DateTime LearnStartDate { get; set; }

        public int AimType { get; set; }

        public int FundModel { get; set; }
    }
}
