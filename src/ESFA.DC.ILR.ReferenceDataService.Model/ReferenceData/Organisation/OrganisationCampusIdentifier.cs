using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.Organisation
{
    public class OrganisationCampusIdentifier : AbstractTimeBoundedEntity
    {
        public string CampusIdentifier { get; set; }
    }
}
