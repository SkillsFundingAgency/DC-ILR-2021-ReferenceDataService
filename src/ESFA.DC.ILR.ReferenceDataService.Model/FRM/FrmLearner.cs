using System;
using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.FRM
{
    public class FrmLearner : AbstractLearner
    {
        public int AimSeqNumber { get; set; }

        public string LearningAimTitle { get; set; }

        public string OrgName { get; set; }

        public string PartnerOrgName { get; set; }

        public int AimType { get; set; }

        public long? PartnerUKPRN { get; set; }

        public string SWSupAimId { get; set; }

        public DateTime LearnPlanEndDate { get; set; }

        public int? PriorLearnFundAdj { get; set; }

        public int? OtherFundAdj { get; set; }

        public int CompStatus { get; set; }

        public int? Outcome { get; set; }

        public ICollection<LearningDeliveryFAM> LearningDeliveryFAMs { get; set; }

        public ICollection<ProviderSpecLearnerMonitoring> ProviderSpecLearnerMonitorings { get; set; }

        public ICollection<ProviderSpecDeliveryMonitoring> ProvSpecDeliveryMonitorings { get; set; }
    }
}
