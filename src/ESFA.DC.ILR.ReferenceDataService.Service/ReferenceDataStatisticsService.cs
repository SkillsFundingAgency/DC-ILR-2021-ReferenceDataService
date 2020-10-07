using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.Internal;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public class ReferenceDataStatisticsService : IReferenceDataStatisticsService
    {
        private List<ReferenceDataSummaryStatistics> _desktopReferenceDataSummaryReports = new List<ReferenceDataSummaryStatistics>();

        public void AddRecordCount(string name, int count)
        {
            _desktopReferenceDataSummaryReports.Add(new ReferenceDataSummaryStatistics()
            {
                DataSource = name,
                NumberOfRecords = count,
            });
        }

        public IEnumerable<ReferenceDataSummaryStatistics> GetStatistics()
        {
            return _desktopReferenceDataSummaryReports.OrderBy(x => x.DataSource);
        }
    }
}
