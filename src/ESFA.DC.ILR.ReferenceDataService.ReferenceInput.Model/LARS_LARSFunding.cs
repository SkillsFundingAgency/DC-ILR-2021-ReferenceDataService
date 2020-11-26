using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSFunding
    {
        public int Id { get; set; }
        public string LearnAimRef { get; set; }
        public string FundingCategory { get; set; }
        public decimal? RateUnWeighted { get; set; }
        public decimal? RateWeighted { get; set; }
        public string WeightingFactor { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LARS_LARSLearningDelivery_Id { get; set; }

        public virtual LARS_LARSLearningDelivery LARS_LARSLearningDelivery_ { get; set; }
    }
}
