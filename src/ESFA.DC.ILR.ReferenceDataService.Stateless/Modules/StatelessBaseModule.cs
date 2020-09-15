using Autofac;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.FileService.Config;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Modules;
using ESFA.DC.ILR.ReferenceDataService.Service.Clients;
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
            var referenceDataOptions = serviceFabricConfigurationService.GetConfigSectionAs<ReferenceDataOptions>("ReferenceDataSection");

            var featureConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<FeatureConfiguration>("FeatureConfiguration");
            containerBuilder.RegisterInstance(featureConfiguration).As<FeatureConfiguration>().SingleInstance();

            var apiSettings = serviceFabricConfigurationService.GetConfigSectionAs<ApiSettings>("ApiSettings");
            containerBuilder.RegisterInstance(apiSettings).As<ApiSettings>().SingleInstance();

            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterModule(new StatelessServiceModule(statelessServiceConfiguration));
            containerBuilder.RegisterModule(new IOModule(azureStorageFileServiceConfiguration, ioConfiguration));
            containerBuilder.RegisterModule(new RepositoryModule(referenceDataOptions));
            containerBuilder.RegisterModule<SerializationModule>();
            containerBuilder.RegisterModule<ReferenceDataRepositoryServicesModule>();
            containerBuilder.RegisterType<DateTimeProvider.DateTimeProvider>().As<IDateTimeProvider>();
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler<JobContextMessage>>();
        }
    }
}