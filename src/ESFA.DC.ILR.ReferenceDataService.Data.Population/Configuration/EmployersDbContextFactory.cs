using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ReferenceData.Employers.Model;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class EmployersDbContextFactory : IDbContextFactory<IEmployersContext>
    {
        private readonly string _connectionString;

        public EmployersDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEmployersContext Create()
        {
            DbContextOptions<EmployersContext> options =
                new DbContextOptionsBuilder<EmployersContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new EmployersContext(options);
        }
    }
}
