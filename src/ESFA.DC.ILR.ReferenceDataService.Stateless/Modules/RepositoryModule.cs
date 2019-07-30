using Autofac;
using ESFA.DC.Data.AppsEarningsHistory.Model;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interface;
using ESFA.DC.EAS1920.EF;
using ESFA.DC.EAS1920.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
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
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var configHelper = new ConfigurationHelper();

            var referenceDataOptions = configHelper.GetSectionValues<ReferenceDataOptions>("ReferenceDataSection");
            containerBuilder.RegisterInstance(referenceDataOptions).As<IReferenceDataOptions>().SingleInstance();

            containerBuilder.Register(c =>
            {
                DbContextOptions<AppEarnHistoryContext> options = new DbContextOptionsBuilder<AppEarnHistoryContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().AppsEarningsHistoryConnectionString).Options;

                return new AppEarnHistoryContext(options);
            }).As<IAppEarnHistoryContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<EmployersContext> options = new DbContextOptionsBuilder<EmployersContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().EmployersConnectionString).Options;

                return new EmployersContext(options);
            }).As<IEmployersContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<EpaContext> options = new DbContextOptionsBuilder<EpaContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().EPAConnectionString).Options;

                return new EpaContext(options);
            }).As<IEpaContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<FcsContext> options = new DbContextOptionsBuilder<FcsContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().FCSConnectionString).Options;

                return new FcsContext(options);
            }).As<IFcsContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<LarsContext> options = new DbContextOptionsBuilder<LarsContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().LARSConnectionString).Options;

                return new LarsContext(options);
            }).As<ILARSContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<OrganisationsContext> options = new DbContextOptionsBuilder<OrganisationsContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().OrganisationsConnectionString).Options;

                return new OrganisationsContext(options);
            }).As<IOrganisationsContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<PostcodesContext> options = new DbContextOptionsBuilder<PostcodesContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().PostcodesConnectionString).Options;

                return new PostcodesContext(options);
            }).As<IPostcodesContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<UlnContext> options = new DbContextOptionsBuilder<UlnContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().ULNConnectionstring).Options;

                return new UlnContext(options);
            }).As<IUlnContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<IlrReferenceDataContext> options = new DbContextOptionsBuilder<IlrReferenceDataContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().IlrReferenceDataConnectionString).Options;

                return new IlrReferenceDataContext(options);
            }).As<IIlrReferenceDataContext>().InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                DbContextOptions<EasContext> options = new DbContextOptionsBuilder<EasContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().EasConnectionString).Options;

                return new EasContext(options);
            }).As<IEasdbContext>().InstancePerLifetimeScope();
        }
    }
}
