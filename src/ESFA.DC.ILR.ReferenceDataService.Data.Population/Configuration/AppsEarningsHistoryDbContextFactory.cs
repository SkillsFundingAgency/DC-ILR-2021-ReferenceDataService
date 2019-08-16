using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class AppsEarningsHistoryDbContextFactory : IDbContextFactory<IAppEarnHistoryContext>
    {
        private readonly string _connectionString;

        public AppsEarningsHistoryDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IAppEarnHistoryContext Create()
        {
            DbContextOptions<AppEarnHistoryContext> options =
                new DbContextOptionsBuilder<AppEarnHistoryContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new AppEarnHistoryContext(options);
        }
    }
}
