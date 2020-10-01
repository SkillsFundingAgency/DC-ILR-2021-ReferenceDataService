using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Service.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ReferenceDataStatisticsService : IReferenceDataStatisticsService
    {
        private List<DesktopReferenceDataSummaryReport> _desktopReferenceDataSummaryReports = new List<DesktopReferenceDataSummaryReport>();

        public void AddRecordCount(string name, int count)
        {
            _desktopReferenceDataSummaryReports.Add(new DesktopReferenceDataSummaryReport()
            {
                DataSource = name,
                NumberOfRecords = count,
            });
        }

        public IEnumerable<IReferenceDataSummaryStatistics> GetStatistics()
        {
            return _desktopReferenceDataSummaryReports.OrderBy(x => x.DataSource);
        }
    }
}
