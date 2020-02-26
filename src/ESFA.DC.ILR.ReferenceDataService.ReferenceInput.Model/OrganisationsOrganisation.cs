using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsOrganisation
    {
        public OrganisationsOrganisation()
        {
            OrganisationsOrganisationCampusIdentifier = new HashSet<OrganisationsOrganisationCampusIdentifier>();
            OrganisationsOrganisationCoFremoval = new HashSet<OrganisationsOrganisationCoFremoval>();
            OrganisationsOrganisationFunding = new HashSet<OrganisationsOrganisationFunding>();
        }

        public int Id { get; set; }
        public int Ukprn { get; set; }
        public string Name { get; set; }
        public bool? PartnerUkprn { get; set; }
        public string LegalOrgType { get; set; }
        public bool? LongTermResid { get; set; }

        public virtual ICollection<OrganisationsOrganisationCampusIdentifier> OrganisationsOrganisationCampusIdentifier { get; set; }
        public virtual ICollection<OrganisationsOrganisationCoFremoval> OrganisationsOrganisationCoFremoval { get; set; }
        public virtual ICollection<OrganisationsOrganisationFunding> OrganisationsOrganisationFunding { get; set; }
    }
}
