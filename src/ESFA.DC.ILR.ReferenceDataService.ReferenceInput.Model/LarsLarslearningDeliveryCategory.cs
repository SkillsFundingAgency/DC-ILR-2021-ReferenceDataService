using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarslearningDeliveryCategory
    {
        public int Id { get; set; }
        public string LearnAimRef { get; set; }
        public int CategoryRef { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LarsLarslearningDeliveryId { get; set; }

        public virtual LarsLarslearningDelivery LarsLarslearningDelivery { get; set; }
    }
}
