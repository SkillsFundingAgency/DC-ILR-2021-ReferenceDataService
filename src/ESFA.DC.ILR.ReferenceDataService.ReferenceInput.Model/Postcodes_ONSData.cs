using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class Postcodes_ONSData
    {
        public int Id { get; set; }
        public DateTime? Termination { get; set; }
        public string LocalAuthority { get; set; }
        public string Lep1 { get; set; }
        public string Lep2 { get; set; }
        public string Nuts { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? Postcodes_Postcode_Id { get; set; }

        public virtual Postcodes_Postcode Postcodes_Postcode_ { get; set; }
    }
}
