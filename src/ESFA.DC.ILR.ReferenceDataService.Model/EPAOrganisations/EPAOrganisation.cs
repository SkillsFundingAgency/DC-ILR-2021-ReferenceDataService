using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations
{
    public class EPAOrganisation : AbstractTimeBoundedEntity
    {
        public string ID { get; set; }

        public string Standard { get; set; }
    }
}
