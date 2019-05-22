using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Organisations
{
    public class OrganisationCoFRemoval : AbstractTimeBoundedEntity
    {
        public decimal CoFRemoval { get; set; }
    }
}
