using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsframeworkDesktop
    {
        public LarsLarsframeworkDesktop()
        {
            LarsLarsframeworkApprenticeshipFunding = new HashSet<LarsLarsframeworkApprenticeshipFunding>();
            LarsLarsframeworkCommonComponent = new HashSet<LarsLarsframeworkCommonComponent>();
        }

        public int Id { get; set; }
        public int FworkCode { get; set; }
        public int ProgType { get; set; }
        public int PwayCode { get; set; }
        public DateTime? EffectiveFromNullable { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<LarsLarsframeworkApprenticeshipFunding> LarsLarsframeworkApprenticeshipFunding { get; set; }
        public virtual ICollection<LarsLarsframeworkCommonComponent> LarsLarsframeworkCommonComponent { get; set; }
    }
}
