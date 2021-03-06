﻿using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSStandardFunding
    {
        public int Id { get; set; }
        public string FundingCategory { get; set; }
        public int? BandNumber { get; set; }
        public decimal? CoreGovContributionCap { get; set; }
        public decimal? SixteenToEighteenIncentive { get; set; }
        public decimal? SmallBusinessIncentive { get; set; }
        public decimal? AchievementIncentive { get; set; }
        public string FundableWithoutEmployer { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LARS_LARSStandard_Id { get; set; }

        public virtual LARS_LARSStandard LARS_LARSStandard_ { get; set; }
    }
}
