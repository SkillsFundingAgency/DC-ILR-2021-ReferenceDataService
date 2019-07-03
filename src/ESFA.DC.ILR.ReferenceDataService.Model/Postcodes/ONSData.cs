using System;
using ESFA.DC.ILR.ReferenceDataService.Model.Abstract;

namespace ESFA.DC.ILR.ReferenceDataService.Model.Postcodes
{
    public class ONSData : AbstractTimeBoundedEntity
    {
        public DateTime? Termination { get; set; }

        public string LocalAuthority { get; set; }

        public string Lep1 { get; set; }

        public string Lep2 { get; set; }

        public string Nuts { get; set; }
    }
}
