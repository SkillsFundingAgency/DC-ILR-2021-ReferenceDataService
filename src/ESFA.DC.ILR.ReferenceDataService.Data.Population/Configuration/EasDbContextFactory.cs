using ESFA.DC.EAS1920.EF;
using ESFA.DC.EAS1920.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class EasDbContextFactory : IDbContextFactory<IEasdbContext>
    {
        private readonly string _connectionString;

        public EasDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEasdbContext Create()
        {
            DbContextOptions<EasContext> options =
                new DbContextOptionsBuilder<EasContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new EasContext(options);
        }
    }
}
