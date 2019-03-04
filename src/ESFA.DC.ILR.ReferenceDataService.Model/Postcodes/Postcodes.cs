using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Postcodes
{
    public class Postcodes
    {
        public string Postcode { get; set; }

        public List<ONSData> ONSData { get; set; }

        public List<CareerLearningPilot> CareerLearningPilots { get; set; }

        public List<DasDisadvantage> DasDisadvantages { get; set; }

        public List<EfaDisadvantage> EfaDisadvantages { get; set; }

        public List<SfaAreaCost> SfaAreaCosts { get; set; }

        public List<SfaDisadvantage> SfaDisadvantages { get; set; }
    }
}
