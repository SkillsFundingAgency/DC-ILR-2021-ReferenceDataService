using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsOrganisationCoFremoval
    {
        public int Id { get; set; }
        public decimal CoFremoval { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? OrganisationsOrganisationId { get; set; }

        public virtual OrganisationsOrganisation OrganisationsOrganisation { get; set; }
    }
}
