using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsOrganisation
    {
        public OrganisationsOrganisation()
        {
            OrganisationsOrganisationCampusIdentifiers = new HashSet<OrganisationsOrganisationCampusIdentifier>();
            OrganisationsOrganisationCoFremovals = new HashSet<OrganisationsOrganisationCoFremoval>();
            OrganisationsOrganisationFundings = new HashSet<OrganisationsOrganisationFunding>();
        }

        public int Id { get; set; }
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public bool? PartnerUkprn { get; set; }
        public string LegalOrgType { get; set; }
        public bool? LongTermResid { get; set; }

        public virtual ICollection<OrganisationsOrganisationCampusIdentifier> OrganisationsOrganisationCampusIdentifiers { get; set; }
        public virtual ICollection<OrganisationsOrganisationCoFremoval> OrganisationsOrganisationCoFremovals { get; set; }
        public virtual ICollection<OrganisationsOrganisationFunding> OrganisationsOrganisationFundings { get; set; }
    }
}
