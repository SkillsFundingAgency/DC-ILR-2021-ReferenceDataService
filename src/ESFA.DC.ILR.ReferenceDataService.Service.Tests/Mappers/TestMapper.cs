using CsvHelper.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Service.Tests.Models;

namespace ESFA.DC.ILR.ReferenceDataService.Service.Tests.Mappers
{
    public sealed class TestMapper : ClassMap<TestModel>
    {
        public TestMapper()
        {
            Map(m => m.Id);
            Map(m => m.Amount);
            Map(m => m.Name);
        }
    }
}
