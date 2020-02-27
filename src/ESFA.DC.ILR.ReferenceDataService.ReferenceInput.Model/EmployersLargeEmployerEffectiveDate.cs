using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class EmployersLargeEmployerEffectiveDate
    {
        public int Id { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Employers_Employer_Id { get; set; }

        public virtual EmployersEmployer Employers_Employer_ { get; set; }
    }
}
