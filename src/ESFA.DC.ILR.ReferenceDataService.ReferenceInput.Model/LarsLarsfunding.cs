using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsfunding
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

        public virtual LarsLarslearningDelivery LARS_LARSLearningDelivery_ { get; set; }
    }
}
