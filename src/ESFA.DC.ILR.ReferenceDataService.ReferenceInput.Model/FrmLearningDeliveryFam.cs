using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class FrmLearningDeliveryFam
    {
        public int Id { get; set; }
        public string LearnDelFamtype { get; set; }
        public string LearnDelFamcode { get; set; }
        public DateTime? LearnDelFamdateFrom { get; set; }
        public DateTime? LearnDelFamdateTo { get; set; }
        public int? FrmFrmLearnerId { get; set; }

        public virtual FrmFrmLearner FrmFrmLearner { get; set; }
    }
}
