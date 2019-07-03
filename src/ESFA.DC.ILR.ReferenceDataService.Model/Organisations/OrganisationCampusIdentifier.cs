using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisations
{
    public class OrganisationCampusIdentifier : AbstractTimeBoundedEntity
    {
        public string CampusIdentifier { get; set; }
    }
}
