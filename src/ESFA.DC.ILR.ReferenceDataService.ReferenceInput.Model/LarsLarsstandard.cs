using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LarsLarsstandard
    {
        public LarsLarsstandard()
        {
            LarsLarsstandardApprenticeshipFundings = new HashSet<LarsLarsstandardApprenticeshipFunding>();
            LarsLarsstandardCommonComponents = new HashSet<LarsLarsstandardCommonComponent>();
            LarsLarsstandardFundings = new HashSet<LarsLarsstandardFunding>();
            LarsLarsstandardValidities = new HashSet<LarsLarsstandardValidity>();
        }

        public int Id { get; set; }
        public int StandardCode { get; set; }
        public string StandardSectorCode { get; set; }
        public string NotionalEndLevel { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public virtual ICollection<LarsLarsstandardApprenticeshipFunding> LarsLarsstandardApprenticeshipFundings { get; set; }
        public virtual ICollection<LarsLarsstandardCommonComponent> LarsLarsstandardCommonComponents { get; set; }
        public virtual ICollection<LarsLarsstandardFunding> LarsLarsstandardFundings { get; set; }
        public virtual ICollection<LarsLarsstandardValidity> LarsLarsstandardValidities { get; set; }
    }
}
