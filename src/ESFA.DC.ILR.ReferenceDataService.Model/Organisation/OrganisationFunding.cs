using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisation
{
    public class OrganisationFunding
    {
        public long UKPRN { get; set; }

        public DateTime OrgFundEffectiveFrom { get; set; }

        public DateTime? OrgFundEffectiveTo { get; set; }

        public string OrgFundFactor { get; set; }

        public string OrgFundFactType { get; set; }

        public string OrgFundFactValue { get; set; }
    }
}
