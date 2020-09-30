using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Service.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ReferenceDataStatisticsService : IReferenceDataStatisticsService
    {
        private List<IDesktopReferenceDataSummaryReport> _desktopReferenceDataSummaryReports = new List<IDesktopReferenceDataSummaryReport>();

        public void AddRecordCount(string name, int count)
        {
            _desktopReferenceDataSummaryReports.Add(new DesktopReferenceDataSummaryReport()
            {
                DataSource = name,
                NumberOfRecords = count,
            });
        }

        public IEnumerable<IDesktopReferenceDataSummaryReport> GetStatistics()
        {
            return _desktopReferenceDataSummaryReports;
        }
    }
}
