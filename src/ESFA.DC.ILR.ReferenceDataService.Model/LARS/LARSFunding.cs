using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSFunding
    {
        public string FundingCategory { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public decimal? RateUnWeighted { get; set; }

        public decimal? RateWeighted { get; set; }

        public string WeightingFactor { get; set; }
    }
}
