using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class FcsDbContextFactory : IDbContextFactory<IFcsContext>
    {
        private readonly string _connectionString;

        public FcsDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IFcsContext Create()
        {
            DbContextOptions<FcsContext> options =
                new DbContextOptionsBuilder<FcsContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new FcsContext(options);
        }
    }
}
