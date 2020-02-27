using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsSpecialistResource
    {
        public int Id { get; set; }
        public bool IsSpecialistResource { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Organisations_OrganisationCampusIdentifier_Id { get; set; }

        public virtual OrganisationsOrganisationCampusIdentifier Organisations_OrganisationCampusIdentifier_ { get; set; }
    }
}
