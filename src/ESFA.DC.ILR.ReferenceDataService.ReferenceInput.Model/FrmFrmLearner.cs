using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FrmFrmLearner
    {
        public FrmFrmLearner()
        {
            FrmLearningDeliveryFam = new HashSet<FrmLearningDeliveryFam>();
            FrmProviderSpecDeliveryMonitoring = new HashSet<FrmProviderSpecDeliveryMonitoring>();
            FrmProviderSpecLearnerMonitoring = new HashSet<FrmProviderSpecLearnerMonitoring>();
        }

        public int Id { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }
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
        public long? PrevUkprn { get; set; }
        public long? Pmukprn { get; set; }
        public long? PartnerUkprn { get; set; }
        public string PrevLearnRefNumber { get; set; }
        public string SwsupAimId { get; set; }
        public DateTime LearnPlanEndDate { get; set; }
        public DateTime? LearnActEndDate { get; set; }
        public int? PriorLearnFundAdj { get; set; }
        public int? OtherFundAdj { get; set; }
        public int CompStatus { get; set; }
        public int? Outcome { get; set; }

        public virtual ICollection<FrmLearningDeliveryFam> FrmLearningDeliveryFam { get; set; }
        public virtual ICollection<FrmProviderSpecDeliveryMonitoring> FrmProviderSpecDeliveryMonitoring { get; set; }
        public virtual ICollection<FrmProviderSpecLearnerMonitoring> FrmProviderSpecLearnerMonitoring { get; set; }
    }
}
