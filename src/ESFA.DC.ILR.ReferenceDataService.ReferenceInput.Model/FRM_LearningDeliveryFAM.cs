using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FRM_LearningDeliveryFAM
    {
        public int Id { get; set; }
        public string LearnDelFAMType { get; set; }
        public string LearnDelFAMCode { get; set; }
        public DateTime? LearnDelFAMDateFrom { get; set; }
        public DateTime? LearnDelFAMDateTo { get; set; }
        public int? FRM_FrmLearner_Id { get; set; }

        public virtual FRM_FrmLearner FRM_FrmLearner_ { get; set; }
    }
}
