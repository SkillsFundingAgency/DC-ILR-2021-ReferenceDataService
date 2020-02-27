using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class EmployersLargeEmployerEffectiveDate
    {
        public int Id { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? EmployersEmployerId { get; set; }

        public virtual EmployersEmployer EmployersEmployer { get; set; }
    }
}
