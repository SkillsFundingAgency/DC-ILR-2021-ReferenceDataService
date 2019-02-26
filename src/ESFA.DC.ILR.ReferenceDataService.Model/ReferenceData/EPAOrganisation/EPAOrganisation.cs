using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.EPAOrganisation
{
    public class EPAOrganisation : AbstractTimeBoundedEntity
    {
        public string ID { get; set; }

        public string Standard { get; set; }
    }
}
