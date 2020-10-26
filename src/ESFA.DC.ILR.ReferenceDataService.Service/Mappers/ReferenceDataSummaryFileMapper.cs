using CsvHelper.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Model.Internal;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Mappers
{
    public class ReferenceDataSummaryFileMapper : ClassMap<ReferenceDataSummaryStatistics>
    {
        public ReferenceDataSummaryFileMapper()
        {
            Map(x => x.DataSource).Name("Data Source");
            Map(x => x.NumberOfRecords).Name("Number Of Records");
        }
    }
}
