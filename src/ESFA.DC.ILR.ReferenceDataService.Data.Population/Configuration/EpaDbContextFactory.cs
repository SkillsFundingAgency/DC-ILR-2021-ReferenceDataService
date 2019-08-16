using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class EpaDbContextFactory : IDbContextFactory<IEpaContext>
    {
        private readonly string _connectionString;

        public EpaDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEpaContext Create()
        {
            DbContextOptions<EpaContext> options =
                new DbContextOptionsBuilder<EpaContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new EpaContext(options);
        }
    }
}
