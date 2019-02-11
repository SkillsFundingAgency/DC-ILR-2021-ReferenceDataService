using System;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Postcodes
{
    public class ONSData
    {
       public DateTime? Termination { get; set; }

       public string LocalAuthority { get; set; }

        public string Lep1 { get; set; }

        public string Lep2 { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public string Nuts { get; set; }
    }
}
