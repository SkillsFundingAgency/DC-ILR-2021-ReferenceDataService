using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FRM_ProviderSpecLearnerMonitoring
    {
        public int Id { get; set; }
        public string ProvSpecLearnMonOccur { get; set; }
        public string ProvSpecLearnMon { get; set; }
        public int? FRM_FrmLearner_Id { get; set; }

        public virtual FRM_FrmLearner FRM_FrmLearner_ { get; set; }
    }
}
