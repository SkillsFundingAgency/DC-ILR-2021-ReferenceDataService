using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class McaContracts_McaDevolvedContract
    {
        public int Id { get; set; }
        public string McaGlaShortCode { get; set; }
        public int Ukprn { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
