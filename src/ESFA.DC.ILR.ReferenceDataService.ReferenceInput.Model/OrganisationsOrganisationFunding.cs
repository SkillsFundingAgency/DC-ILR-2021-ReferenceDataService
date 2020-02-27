using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class OrganisationsOrganisationFunding
    {
        public int Id { get; set; }
        public string OrgFundFactor { get; set; }
        public string OrgFundFactType { get; set; }
        public string OrgFundFactValue { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? OrganisationsOrganisationId { get; set; }

        public virtual OrganisationsOrganisation OrganisationsOrganisation { get; set; }
    }
}
