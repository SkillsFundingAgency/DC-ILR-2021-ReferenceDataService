using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisation
{
    public class OrganisationFunding : AbstractTimeBoundedEntity
    {
        public string OrgFundFactor { get; set; }

        public string OrgFundFactType { get; set; }

        public string OrgFundFactValue { get; set; }
    }
}
