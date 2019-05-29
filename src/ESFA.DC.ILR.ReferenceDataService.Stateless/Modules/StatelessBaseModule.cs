using Autofac;
using ESFA.DC.FileService.Config;
using ESFA.DC.ILR.ReferenceDataService.Modules;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Config;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.ServiceFabric.Common.Config;
using ESFA.DC.ServiceFabric.Common.Modules;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class StatelessBaseModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var serviceFabricConfigurationService = new ServiceFabricConfigurationService();

            var statelessServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAsStatelessServiceConfiguration();
            var azureStorageFileServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<AzureStorageFileServiceConfiguration>("AzureStorageFileServiceConfiguration");
            var ioConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<IOConfiguration>("IOConfiguration");

            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterModule(new StatelessServiceModule(statelessServiceConfiguration));
            containerBuilder.RegisterModule(new IOModule(azureStorageFileServiceConfiguration, ioConfiguration));
            containerBuilder.RegisterModule<RepositoryModule>();
            containerBuilder.RegisterModule<SerializationModule>();
            containerBuilder.RegisterModule<ReferenceDataRepositoryServicesModule>();
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler<JobContextMessage>>();
        }
    }
}
