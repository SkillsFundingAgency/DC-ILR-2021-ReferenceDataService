using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Postcodes_Postcode
    {
        public Postcodes_Postcode()
        {
            Postcodes_DasDisadvantages = new HashSet<Postcodes_DasDisadvantage>();
            Postcodes_EfaDisadvantages = new HashSet<Postcodes_EfaDisadvantage>();
            Postcodes_McaglaSOFs = new HashSet<Postcodes_McaglaSOF>();
            Postcodes_ONSDatas = new HashSet<Postcodes_ONSData>();
            Postcodes_SfaAreaCosts = new HashSet<Postcodes_SfaAreaCost>();
            Postcodes_SfaDisadvantages = new HashSet<Postcodes_SfaDisadvantage>();
        }

        public int Id { get; set; }
        public string PostCode { get; set; }

        public virtual ICollection<Postcodes_DasDisadvantage> Postcodes_DasDisadvantages { get; set; }
        public virtual ICollection<Postcodes_EfaDisadvantage> Postcodes_EfaDisadvantages { get; set; }
        public virtual ICollection<Postcodes_McaglaSOF> Postcodes_McaglaSOFs { get; set; }
        public virtual ICollection<Postcodes_ONSData> Postcodes_ONSDatas { get; set; }
        public virtual ICollection<Postcodes_SfaAreaCost> Postcodes_SfaAreaCosts { get; set; }
        public virtual ICollection<Postcodes_SfaDisadvantage> Postcodes_SfaDisadvantages { get; set; }
    }
}
