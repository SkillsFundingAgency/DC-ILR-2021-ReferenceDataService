using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.Employers
{
    public class Employers
    {
        public int ERN { get; set; }

        public List<LargeEmployerEffectiveDates> LargeEmployerEffectiveDates { get; set; }
    }
}
