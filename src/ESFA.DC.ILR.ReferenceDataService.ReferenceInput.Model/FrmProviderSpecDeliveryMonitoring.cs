using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FrmProviderSpecDeliveryMonitoring
    {
        public int Id { get; set; }
        public string ProvSpecDelMonOccur { get; set; }
        public string ProvSpecDelMon { get; set; }
        public int? FrmFrmLearnerId { get; set; }

        public virtual FrmFrmLearner FrmFrmLearner { get; set; }
    }
}
