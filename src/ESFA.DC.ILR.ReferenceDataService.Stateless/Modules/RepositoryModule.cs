using Autofac;
using ESFA.DC.Data.AppsEarningsHistory.Model.Interface;
using ESFA.DC.EAS1920.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Constants;
using ESFA.DC.ILR1819.DataStore.EF.Valid.Interface;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.EPA.Model.Interface;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using ESFA.DC.ReferenceData.ULN.Model.Interface;

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

            containerBuilder.RegisterType<AppsEarningsHistoryDbContextFactory>().As<IDbContextFactory<IAppEarnHistoryContext>>()
              .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.AppsEarningsHistoryConnectionString);

            containerBuilder.RegisterType<EasDbContextFactory>().As<IDbContextFactory<IEasdbContext>>()
              .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.EasConnectionString);

            containerBuilder.RegisterType<EmployersDbContextFactory>().As<IDbContextFactory<IEmployersContext>>()
              .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.EmployersConnectionString);

            containerBuilder.RegisterType<EpaDbContextFactory>().As<IDbContextFactory<IEpaContext>>()
             .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.EPAConnectionString);

            containerBuilder.RegisterType<FcsDbContextFactory>().As<IDbContextFactory<IFcsContext>>()
               .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.FCSConnectionString);

            containerBuilder.RegisterType<IlrReferenceDataDbContextFactory>().As<IDbContextFactory<IIlrReferenceDataContext>>()
               .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.IlrReferenceDataConnectionString);

            containerBuilder.RegisterType<LarsDbContextFactory>().As<IDbContextFactory<ILARSContext>>()
               .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.LARSConnectionString);

            containerBuilder.RegisterType<OrganisationsDbContextFactory>().As<IDbContextFactory<IOrganisationsContext>>()
               .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.OrganisationsConnectionString);

            containerBuilder.RegisterType<PostcodesDbContextFactory>().As<IDbContextFactory<IPostcodesContext>>()
               .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.PostcodesConnectionString);

            containerBuilder.RegisterType<UlnDbContextFactory>().As<IDbContextFactory<IUlnContext>>()
               .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.ULNConnectionstring);

            containerBuilder.RegisterType<IlrDbContextFactory>().As<IDbContextFactory<IILR1819_DataStoreEntitiesValid>>()
                .WithParameter(StatelessConstants.ConnectionString, _referenceDataOptions.Ilr1819ConnectionString);
        }
    }
}
