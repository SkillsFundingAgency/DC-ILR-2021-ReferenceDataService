using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisations
{
    public class OrganisationCampusIdentifier : AbstractTimeBoundedEntity
    {
        public long UKPRN { get; set; }

        public string CampusIdentifier { get; set; }

        public List<OrganisationCampusIdSpecialistResource> SpecialistResources { get; set; }
    }
}
