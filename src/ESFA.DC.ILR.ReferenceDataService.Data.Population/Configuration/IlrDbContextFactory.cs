using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR1819.DataStore.EF.Valid;
using ESFA.DC.ILR1819.DataStore.EF.Valid.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class IlrDbContextFactory : IDbContextFactory<IILR1819_DataStoreEntitiesValid>
    {
        private readonly string _connectionString;

        public IlrDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IILR1819_DataStoreEntitiesValid Create()
        {
            DbContextOptions<ILR1819_DataStoreEntitiesValid> options =
                new DbContextOptionsBuilder<ILR1819_DataStoreEntitiesValid>()
                    .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new ILR1819_DataStoreEntitiesValid(options);
        }
    }
}
