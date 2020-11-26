using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR1920.DataStore.EF.Valid;
using ESFA.DC.ILR1920.DataStore.EF.Valid.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class IlrDbContextFactory : IDbContextFactory<IILR1920_DataStoreEntitiesValid>
    {
        private readonly string _connectionString;

        public IlrDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IILR1920_DataStoreEntitiesValid Create()
        {
            DbContextOptions<ILR1920_DataStoreEntitiesValid> options =
                new DbContextOptionsBuilder<ILR1920_DataStoreEntitiesValid>()
                    .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new ILR1920_DataStoreEntitiesValid(options);
        }
    }
}
