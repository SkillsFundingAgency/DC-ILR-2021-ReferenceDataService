using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Employers_LargeEmployerEffectiveDate
    {
        public int Id { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Employers_Employer_Id { get; set; }

        public virtual Employers_Employer Employers_Employer_ { get; set; }
    }
}
