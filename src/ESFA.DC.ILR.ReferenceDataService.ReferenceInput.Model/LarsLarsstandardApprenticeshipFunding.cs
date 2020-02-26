using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsstandardApprenticeshipFunding
    {
        public int Id { get; set; }
        public int ProgType { get; set; }
        public int? PwayCode { get; set; }
        public string FundingCategory { get; set; }
        public int? BandNumber { get; set; }
        public decimal? CoreGovContributionCap { get; set; }
        public decimal? SixteenToEighteenIncentive { get; set; }
        public decimal? SixteenToEighteenProviderAdditionalPayment { get; set; }
        public decimal? SixteenToEighteenEmployerAdditionalPayment { get; set; }
        public decimal? SixteenToEighteenFrameworkUplift { get; set; }
        public decimal? CareLeaverAdditionalPayment { get; set; }
        public decimal? Duration { get; set; }
        public decimal? ReservedValue2 { get; set; }
        public decimal? ReservedValue3 { get; set; }
        public decimal? MaxEmployerLevyCap { get; set; }
        public string FundableWithoutEmployer { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LarsLarsstandardId { get; set; }

        public virtual LarsLarsstandard LarsLarsstandard { get; set; }
    }
}
