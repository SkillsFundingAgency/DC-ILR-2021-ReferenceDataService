using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsstandard
    {
        public LarsLarsstandard()
        {
            LarsLarsstandardApprenticeshipFunding = new HashSet<LarsLarsstandardApprenticeshipFunding>();
            LarsLarsstandardCommonComponent = new HashSet<LarsLarsstandardCommonComponent>();
            LarsLarsstandardFunding = new HashSet<LarsLarsstandardFunding>();
            LarsLarsstandardValidity = new HashSet<LarsLarsstandardValidity>();
        }

        public int Id { get; set; }
        public int StandardCode { get; set; }
        public string StandardSectorCode { get; set; }
        public string NotionalEndLevel { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<LarsLarsstandardApprenticeshipFunding> LarsLarsstandardApprenticeshipFunding { get; set; }
        public virtual ICollection<LarsLarsstandardCommonComponent> LarsLarsstandardCommonComponent { get; set; }
        public virtual ICollection<LarsLarsstandardFunding> LarsLarsstandardFunding { get; set; }
        public virtual ICollection<LarsLarsstandardValidity> LarsLarsstandardValidity { get; set; }
    }
}
