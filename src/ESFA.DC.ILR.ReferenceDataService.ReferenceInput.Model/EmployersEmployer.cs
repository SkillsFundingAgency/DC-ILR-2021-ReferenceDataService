using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class EmployersEmployer
    {
        public EmployersEmployer()
        {
            EmployersLargeEmployerEffectiveDates = new HashSet<EmployersLargeEmployerEffectiveDates>();
        }

        public int Id { get; set; }
        public int Ern { get; set; }

        public virtual ICollection<EmployersLargeEmployerEffectiveDates> EmployersLargeEmployerEffectiveDates { get; set; }
    }
}
