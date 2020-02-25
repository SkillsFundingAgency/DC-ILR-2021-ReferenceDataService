using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class LarsDbContextFactory : IDbContextFactory<ILARSContext>
    {
        private readonly string _connectionString;

        public LarsDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILARSContext Create()
        {
            DbContextOptions<LarsContext> options =
                new DbContextOptionsBuilder<LarsContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new LarsContext(options);
        }
    }
}
