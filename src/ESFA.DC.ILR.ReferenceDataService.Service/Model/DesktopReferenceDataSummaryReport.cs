using ESFA.DC.ILR.ReferenceDataService.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Model
{
    public class DesktopReferenceDataSummaryReport : IReferenceDataSummaryStatistics
    {
        public string DataSource { get; set; }

        public int NumberOfRecords { get; set; }
    }
}
