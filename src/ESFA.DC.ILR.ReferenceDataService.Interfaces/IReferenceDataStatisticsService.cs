using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IReferenceDataStatisticsService
    {
        void AddRecordCount(string name, int count);

        IEnumerable<IDesktopReferenceDataSummaryReport> GetStatistics();
    }
}
