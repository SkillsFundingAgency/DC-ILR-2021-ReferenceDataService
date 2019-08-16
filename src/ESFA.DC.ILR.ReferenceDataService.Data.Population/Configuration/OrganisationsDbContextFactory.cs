using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration
{
    public class OrganisationsDbContextFactory : IDbContextFactory<IOrganisationsContext>
    {
        private readonly string _connectionString;

        public OrganisationsDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IOrganisationsContext Create()
        {
            DbContextOptions<OrganisationsContext> options =
                new DbContextOptionsBuilder<OrganisationsContext>()
                .UseSqlServer(_connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            return new OrganisationsContext(options);
        }
    }
}
