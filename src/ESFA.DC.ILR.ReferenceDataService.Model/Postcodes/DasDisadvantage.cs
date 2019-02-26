using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Postcodes
{
    public class DasDisadvantage : AbstractTimeBoundedEntity
    {
        public decimal? Uplift { get; set; }
    }
}
