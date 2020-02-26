using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsOrganisationCampusIdentifier
    {
        public OrganisationsOrganisationCampusIdentifier()
        {
            OrganisationsSpecialistResource = new HashSet<OrganisationsSpecialistResource>();
        }

        public int Id { get; set; }
        public long Ukprn { get; set; }
        public string CampusIdentifier { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? OrganisationsOrganisationId { get; set; }

        public virtual OrganisationsOrganisation OrganisationsOrganisation { get; set; }
        public virtual ICollection<OrganisationsSpecialistResource> OrganisationsSpecialistResource { get; set; }
    }
}
