using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisations
{
    public class OrganisationPostcodeSpecialistResource : AbstractTimeBoundedEntity
    {
        public long UKPRN { get; set; }

        public string Postcode { get; set; }

        public string SpecialistResources { get; set; }
    }
}
