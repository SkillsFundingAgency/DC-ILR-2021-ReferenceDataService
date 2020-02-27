using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsOrganisationCoFremoval
    {
        public int Id { get; set; }
        public decimal CoFRemoval { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Organisations_Organisation_Id { get; set; }

        public virtual OrganisationsOrganisation Organisations_Organisation_ { get; set; }
    }
}
