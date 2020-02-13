using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class ReferenceDataOptions : IReferenceDataOptions
    {
        public string LARSConnectionString { get; set; }

        public string OrganisationsConnectionString { get; set;  }

        public string PostcodesConnectionString { get; set; }

        public string ULNConnectionstring { get; set; }

        public string FCSConnectionString { get; set; }

        public string EPAConnectionString { get; set; }

        public string EmployersConnectionString { get; set; }

        public string AppsEarningsHistoryConnectionString { get; set; }

        public string IlrReferenceDataConnectionString { get; set; }

        public string EasConnectionString { get; set; }

        public string Ilr1819ConnectionString { get; set; }
    }
}
