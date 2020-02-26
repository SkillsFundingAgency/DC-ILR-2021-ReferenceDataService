using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsframeworkCommonComponent
    {
        public int Id { get; set; }
        public int CommonComponent { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? LarsLarsframeworkDesktopId { get; set; }
        public int? LarsLarsframeworkId { get; set; }

        public virtual LarsLarsframework LarsLarsframework { get; set; }
        public virtual LarsLarsframeworkDesktop LarsLarsframeworkDesktop { get; set; }
    }
}
