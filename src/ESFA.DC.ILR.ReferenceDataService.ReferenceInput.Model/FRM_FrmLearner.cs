using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FRM_FrmLearner
    {
        public FRM_FrmLearner()
        {
            FRM_LearningDeliveryFAMs = new HashSet<FRM_LearningDeliveryFAM>();
            FRM_ProviderSpecDeliveryMonitorings = new HashSet<FRM_ProviderSpecDeliveryMonitoring>();
            FRM_ProviderSpecLearnerMonitorings = new HashSet<FRM_ProviderSpecLearnerMonitoring>();
        }

        public int Id { get; set; }
        public long UKPRN { get; set; }
        public long ULN { get; set; }
        public int AimSeqNumber { get; set; }
        public string LearnRefNumber { get; set; }
        public string LearnAimRef { get; set; }
        public string LearningAimTitle { get; set; }
        public string OrgName { get; set; }
        public string PartnerOrgName { get; set; }
        public int? ProgTypeNullable { get; set; }
        public int? StdCodeNullable { get; set; }
        public int? FworkCodeNullable { get; set; }
        public int? PwayCodeNullable { get; set; }
        public DateTime LearnStartDate { get; set; }
        public int AimType { get; set; }
        public int FundModel { get; set; }
        public long? PrevUKPRN { get; set; }
        public long? PMUKPRN { get; set; }
        public long? PartnerUKPRN { get; set; }
        public string PrevLearnRefNumber { get; set; }
        public string SWSupAimId { get; set; }
        public DateTime LearnPlanEndDate { get; set; }
        public DateTime? LearnActEndDate { get; set; }
        public int? PriorLearnFundAdj { get; set; }
        public int? OtherFundAdj { get; set; }
        public int CompStatus { get; set; }
        public int? Outcome { get; set; }

        public virtual ICollection<FRM_LearningDeliveryFAM> FRM_LearningDeliveryFAMs { get; set; }
        public virtual ICollection<FRM_ProviderSpecDeliveryMonitoring> FRM_ProviderSpecDeliveryMonitorings { get; set; }
        public virtual ICollection<FRM_ProviderSpecLearnerMonitoring> FRM_ProviderSpecLearnerMonitorings { get; set; }
    }
}
