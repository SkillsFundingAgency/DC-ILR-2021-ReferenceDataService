using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Employers
{
    public class Employers
    {
        public int ERN { get; set; }

        public bool IsLargeEmployer { get; set; }

        public List<LargeEmployerEffectiveDates> LargeEmployerEffectiveDates { get; set; }
    }
}
