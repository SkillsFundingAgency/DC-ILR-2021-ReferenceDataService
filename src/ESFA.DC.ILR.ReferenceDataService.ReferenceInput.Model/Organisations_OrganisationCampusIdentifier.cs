using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Organisations_OrganisationCampusIdentifier
    {
        public Organisations_OrganisationCampusIdentifier()
        {
            Organisations_SpecialistResources = new HashSet<Organisations_SpecialistResource>();
        }

        public int Id { get; set; }
        public long UKPRN { get; set; }
        public string CampusIdentifier { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Organisations_Organisation_Id { get; set; }

        public virtual Organisations_Organisation Organisations_Organisation_ { get; set; }
        public virtual ICollection<Organisations_SpecialistResource> Organisations_SpecialistResources { get; set; }
    }
}
