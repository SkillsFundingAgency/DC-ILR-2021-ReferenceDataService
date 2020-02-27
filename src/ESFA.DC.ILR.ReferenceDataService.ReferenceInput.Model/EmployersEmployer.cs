using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class EmployersEmployer
    {
        public EmployersEmployer()
        {
            EmployersLargeEmployerEffectiveDates = new HashSet<EmployersLargeEmployerEffectiveDate>();
        }

        public int Id { get; set; }
        public int Ern { get; set; }

        public virtual ICollection<EmployersLargeEmployerEffectiveDate> EmployersLargeEmployerEffectiveDates { get; set; }
    }
}
