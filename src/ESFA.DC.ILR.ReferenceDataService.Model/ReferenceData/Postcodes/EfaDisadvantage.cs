using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.ReferenceData.Postcodes
{
    public class EfaDisadvantage : AbstractTimeBoundedEntity
    {
        public decimal? Uplift { get; set; }
    }
}
