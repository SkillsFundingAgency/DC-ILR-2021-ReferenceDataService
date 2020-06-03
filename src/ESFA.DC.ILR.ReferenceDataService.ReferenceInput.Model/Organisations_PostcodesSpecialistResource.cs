using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Organisations_PostcodesSpecialistResource
    {
        public int Id { get; set; }
        public long UKPRN { get; set; }
        public string Postcode { get; set; }
        public string SpecialistResources { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Organisations_Organisation_Id { get; set; }

        public virtual Organisations_Organisation Organisations_Organisation_ { get; set; }
    }
}
