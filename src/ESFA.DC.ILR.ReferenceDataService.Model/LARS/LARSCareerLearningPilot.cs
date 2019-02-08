using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.ReferenceDataService.Model.LARS
{
    public class LARSCareerLearningPilot
    {
        public string LearnAimRef { get; set; }

        public string AreaCode { get; set; }

        public decimal? SubsidyRate { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
