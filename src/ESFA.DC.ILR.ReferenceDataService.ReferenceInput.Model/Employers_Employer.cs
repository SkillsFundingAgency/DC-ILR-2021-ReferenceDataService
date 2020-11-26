using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Employers_Employer
    {
        public Employers_Employer()
        {
            Employers_LargeEmployerEffectiveDates = new HashSet<Employers_LargeEmployerEffectiveDate>();
        }

        public int Id { get; set; }
        public int ERN { get; set; }

        public virtual ICollection<Employers_LargeEmployerEffectiveDate> Employers_LargeEmployerEffectiveDates { get; set; }
    }
}
