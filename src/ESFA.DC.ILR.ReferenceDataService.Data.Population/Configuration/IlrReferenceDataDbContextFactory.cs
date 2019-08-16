using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class IlrReferenceDataDbContextFactory : IDbContextFactory<IIlrReferenceDataContext>
    {
        private readonly string _connectionString;

        public IlrReferenceDataDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IIlrReferenceDataContext Create()
        {
            DbContextOptions<IlrReferenceDataContext> options =
                new DbContextOptionsBuilder<IlrReferenceDataContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new IlrReferenceDataContext(options);
        }
    }
}
