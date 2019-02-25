using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS.Abstract
{
    public class AbstractLARSApprenticeshipFunding : AbstractTimeBoundedEntity
    {
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
    }
}
