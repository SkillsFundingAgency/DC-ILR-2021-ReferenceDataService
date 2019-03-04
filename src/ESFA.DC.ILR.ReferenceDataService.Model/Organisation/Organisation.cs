using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisation
{
    public class Organisation
    {
        public int UKPRN { get; set; }

        public bool? PartnerUKPRN { get; set; }

        public string LegalOrgType { get; set; }

        public List<string> CampusIdentifers { get; set; }

        public List<OrganisationFunding> OrganisationFundings { get; set; }
    }
}
