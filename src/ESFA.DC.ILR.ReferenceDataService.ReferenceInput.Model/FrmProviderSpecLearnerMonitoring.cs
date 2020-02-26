using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FrmProviderSpecLearnerMonitoring
    {
        public int Id { get; set; }
        public string ProvSpecLearnMonOccur { get; set; }
        public string ProvSpecLearnMon { get; set; }
        public int? FrmFrmLearnerId { get; set; }

        public virtual FrmFrmLearner FrmFrmLearner { get; set; }
    }
}
