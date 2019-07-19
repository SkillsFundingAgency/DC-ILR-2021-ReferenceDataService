using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFunding : AbstractTimeBoundedEntity
    {
        public string LearnAimRef { get; set; }

        public string FundingCategory { get; set; }

        public decimal? RateUnWeighted { get; set; }

        public decimal? RateWeighted { get; set; }

        public string WeightingFactor { get; set; }
    }
}
