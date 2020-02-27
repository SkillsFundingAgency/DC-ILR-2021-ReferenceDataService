using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsOrganisationCampusIdentifier
    {
        public OrganisationsOrganisationCampusIdentifier()
        {
            OrganisationsSpecialistResources = new HashSet<OrganisationsSpecialistResource>();
        }

        public int Id { get; set; }
        public long UKPRN { get; set; }
        public string CampusIdentifier { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Organisations_Organisation_Id { get; set; }

        public virtual OrganisationsOrganisation Organisations_Organisation_ { get; set; }
        public virtual ICollection<OrganisationsSpecialistResource> OrganisationsSpecialistResources { get; set; }
    }
}
