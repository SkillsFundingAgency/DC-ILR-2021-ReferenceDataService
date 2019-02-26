using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.LARS
{
    public class LARSFunding : AbstractTimeBoundedEntity
    {
        public string FundingCategory { get; set; }

        public decimal? RateUnWeighted { get; set; }

        public decimal? RateWeighted { get; set; }

        public string WeightingFactor { get; set; }
    }
}
