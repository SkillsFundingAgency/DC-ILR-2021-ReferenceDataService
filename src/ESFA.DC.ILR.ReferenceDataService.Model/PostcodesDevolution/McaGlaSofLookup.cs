using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution
{
    public class McaGlaSofLookup : AbstractTimeBoundedEntity
    {
        public string SofCode { get; set; }

        public string McaGlaShortCode { get; set; }

        public string McaGlaFullName { get; set; }
    }
}
