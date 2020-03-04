using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Organisations_Organisation
    {
        public Organisations_Organisation()
        {
            Organisations_OrganisationCampusIdentifiers = new HashSet<Organisations_OrganisationCampusIdentifier>();
            Organisations_OrganisationCoFRemovals = new HashSet<Organisations_OrganisationCoFRemoval>();
            Organisations_OrganisationFundings = new HashSet<Organisations_OrganisationFunding>();
        }

        public int Id { get; set; }
        public int UKPRN { get; set; }
        public string Name { get; set; }
        public bool? PartnerUKPRN { get; set; }
        public string LegalOrgType { get; set; }
        public bool? LongTermResid { get; set; }

        public virtual ICollection<Organisations_OrganisationCampusIdentifier> Organisations_OrganisationCampusIdentifiers { get; set; }
        public virtual ICollection<Organisations_OrganisationCoFRemoval> Organisations_OrganisationCoFRemovals { get; set; }
        public virtual ICollection<Organisations_OrganisationFunding> Organisations_OrganisationFundings { get; set; }
    }
}
