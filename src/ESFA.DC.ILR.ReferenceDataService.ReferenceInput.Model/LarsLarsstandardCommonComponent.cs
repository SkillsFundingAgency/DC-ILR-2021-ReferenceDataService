using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsstandardCommonComponent
    {
        public int Id { get; set; }
        public int CommonComponent { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LarsLarsstandardId { get; set; }

        public virtual LarsLarsstandard LarsLarsstandard { get; set; }
    }
}
