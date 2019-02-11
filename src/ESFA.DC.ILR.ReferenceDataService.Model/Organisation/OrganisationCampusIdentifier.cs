using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisation
{
    public class OrganisationCampusIdentifier : AbstractTimeBoundedEntity
    {
        public string CampusIdentifier { get; set; }
    }
}
