using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class ReferenceDataRepositoryServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AppEarningsHistoryRepositoryService>().As<IAppEarningsHistoryRepositoryService>();
            containerBuilder.RegisterType<EmployersRepositoryService>().As<IEmployersRepositoryService>();
            containerBuilder.RegisterType<EpaOrganisationsRepositoryService>().As<IEpaOrganisationsRepositoryService>();
            containerBuilder.RegisterType<FcsRepositoryService>().As<IFcsRepositoryService>();
            containerBuilder.RegisterType<LarsStandardRepositoryService>().As<ILarsStandardRepositoryService>();
            containerBuilder.RegisterType<LarsLearningDeliveryRepositoryService>().As<ILarsLearningDeliveryRepositoryService>();
            containerBuilder.RegisterType<MetaDataRetrievalService>().As<IMetaDataRetrievalService>();
            containerBuilder.RegisterType<OrganisationsRepositoryService>().As<IOrganisationsRepositoryService>();
            containerBuilder.RegisterType<PostcodesRepositoryService>().As<IPostcodesRepositoryService>();
            containerBuilder.RegisterType<UlnRepositoryService>().As<IUlnRepositoryService>();
        }
    }
}
