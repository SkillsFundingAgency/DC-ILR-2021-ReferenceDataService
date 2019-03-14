using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Employers
{
    public class Employer
    {
        public int ERN { get; set; }

        public List<LargeEmployerEffectiveDates> LargeEmployerEffectiveDates { get; set; }
    }
}
