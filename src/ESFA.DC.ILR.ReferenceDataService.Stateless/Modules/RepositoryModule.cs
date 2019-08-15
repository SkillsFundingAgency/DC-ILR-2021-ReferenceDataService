using Autofac;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interface;
using ESFA.DC.EAS1920.EF;
using ESFA.DC.EAS1920.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Constants;
using ESFA.DC.ReferenceData.Employers.Model;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.EPA.Model;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using ESFA.DC.ReferenceData.ULN.Model;
using ESFA.DC.ReferenceData.ULN.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class RepositoryModule : Module
    {
        private readonly IReferenceDataOptions _referenceDataOptions;

        public RepositoryModule(IReferenceDataOptions referenceDataOptions)
        {
            _referenceDataOptions = referenceDataOptions;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {

            containerBuilder.RegisterInstance(_referenceDataOptions).As<IReferenceDataOptions>();
            containerBuilder.RegisterType<PostcodesDbContextFactory>().As<IDbContextFactory<IPostcodesContext>>()
               .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.PostcodesConnectionString);

            containerBuilder.Register(c =>
            {
                DbContextOptions<AppEarnHistoryContext> options = new DbContextOptionsBuilder<AppEarnHistoryContext>()
                    .UseSqlServer(_referenceDataOptions.AppsEarningsHistoryConnectionString).Options;

                return new AppEarnHistoryContext(options);
            }).As<IAppEarnHistoryContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<EmployersContext> options = new DbContextOptionsBuilder<EmployersContext>()
                    .UseSqlServer(_referenceDataOptions.EmployersConnectionString).Options;

                return new EmployersContext(options);
            }).As<IEmployersContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<EpaContext> options = new DbContextOptionsBuilder<EpaContext>()
                    .UseSqlServer(_referenceDataOptions.EPAConnectionString).Options;

                return new EpaContext(options);
            }).As<IEpaContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<FcsContext> options = new DbContextOptionsBuilder<FcsContext>()
                    .UseSqlServer(_referenceDataOptions.FCSConnectionString).Options;

                return new FcsContext(options);
            }).As<IFcsContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<LarsContext> options = new DbContextOptionsBuilder<LarsContext>()
                    .UseSqlServer(_referenceDataOptions.LARSConnectionString).Options;

                return new LarsContext(options);
            }).As<ILARSContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<OrganisationsContext> options = new DbContextOptionsBuilder<OrganisationsContext>()
                    .UseSqlServer(_referenceDataOptions.OrganisationsConnectionString).Options;

                return new OrganisationsContext(options);
            }).As<IOrganisationsContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<PostcodesContext> options = new DbContextOptionsBuilder<PostcodesContext>()
                    .UseSqlServer(_referenceDataOptions.PostcodesConnectionString).Options;

                return new PostcodesContext(options);
            }).As<IPostcodesContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<UlnContext> options = new DbContextOptionsBuilder<UlnContext>()
                    .UseSqlServer(_referenceDataOptions.ULNConnectionstring).Options;

                return new UlnContext(options);
            }).As<IUlnContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<IlrReferenceDataContext> options = new DbContextOptionsBuilder<IlrReferenceDataContext>()
                    .UseSqlServer(_referenceDataOptions.IlrReferenceDataConnectionString).Options;

                return new IlrReferenceDataContext(options);
            }).As<IIlrReferenceDataContext>();

            containerBuilder.Register(c =>
            {
                DbContextOptions<EasContext> options = new DbContextOptionsBuilder<EasContext>()
                    .UseSqlServer(_referenceDataOptions.EasConnectionString).Options;

                return new EasContext(options);
            }).As<IEasdbContext>();
        }
    }
}
