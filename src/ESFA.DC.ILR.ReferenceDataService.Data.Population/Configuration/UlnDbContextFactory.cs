using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ReferenceData.ULN.Model;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class UlnDbContextFactory : IDbContextFactory<IUlnContext>
    {
        private readonly string _connectionString;

        public UlnDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IUlnContext Create()
        {
            DbContextOptions<UlnContext> options =
                new DbContextOptionsBuilder<UlnContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new UlnContext(options);
        }
    }
}
