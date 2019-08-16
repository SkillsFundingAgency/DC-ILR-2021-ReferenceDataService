using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class PostcodesDbContextFactory : IDbContextFactory<IPostcodesContext>
    {
        private readonly string _connectionString;

        public PostcodesDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IPostcodesContext Create()
        {
            DbContextOptions<PostcodesContext> options =
                new DbContextOptionsBuilder<PostcodesContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new PostcodesContext(options);
        }
    }
}
