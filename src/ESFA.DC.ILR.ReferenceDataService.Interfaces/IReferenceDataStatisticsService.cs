using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Model.Internal;

namespace ESFA.DC.ILR.ReferenceDataService.Interfaces
{
    public interface IReferenceDataStatisticsService
    {
        void AddRecordCount(string name, int count);

        IEnumerable<ReferenceDataSummaryStatistics> GetStatistics();
    }
}
