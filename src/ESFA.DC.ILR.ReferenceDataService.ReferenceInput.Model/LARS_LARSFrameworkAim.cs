using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class LARS_LARSFrameworkAim
    {
        public int Id { get; set; }
        public string LearnAimRef { get; set; }
        public int? FrameworkComponentType { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? FworkCode { get; set; }
        public int? ProgType { get; set; }
        public int? PwayCode { get; set; }
        public string Discriminator { get; set; }
    }
}
