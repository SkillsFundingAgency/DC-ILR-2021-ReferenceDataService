using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS
{
    public class LARSStandardFunding : AbstractTimeBoundedEntity
    {
        public string FundingCategory { get; set; }

        public int? BandNumber { get; set; }

        public decimal? CoreGovContributionCap { get; set; }

        public decimal? SixteenToEighteenIncentive { get; set; }

        public decimal? SmallBusinessIncentive { get; set; }

        public decimal? AchievementIncentive { get; set; }

        public string FundableWithoutEmployer { get; set; }
    }
}
