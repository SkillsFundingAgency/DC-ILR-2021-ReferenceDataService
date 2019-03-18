using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.FileService.Config;
using ESFA.DC.ILR.ReferenceDataService.Modules;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Config;
using ESFA.DC.ILR.ReferenceDataService.Stateless.ServiceFabric.Config;
using ESFA.DC.ILR.ReferenceDataService.Stateless.ServiceFabric.Modules;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                var builder = BuildContainerBuilder();

                builder.RegisterServiceFabricSupport();

                builder.RegisterStatelessService<ServiceFabric.Stateless>("ESFA.DC.ILR.ReferenceDataService.StatelessType");

                using (var container = builder.Build())
                {
                    var manager = container.Resolve<IJobContextManager<JobContextMessage>>();

                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ServiceFabric.Stateless).Name);

                    // Prevents this host process from terminating so services keep running.
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ContainerBuilder BuildContainerBuilder()
        {
            var containerBuilder = new ContainerBuilder();

            var serviceFabricConfigurationService = new ServiceFabricConfigurationService();

            var statelessServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAsStatelessServiceConfiguration();
            var azureStorageFileServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<AzureStorageFileServiceConfiguration>("AzureStorageFileServiceConfiguration");
            var ioConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<IOConfiguration>("IOConfiguration");

            containerBuilder.RegisterModule(new StatelessServiceModule(statelessServiceConfiguration));
            containerBuilder.RegisterModule<SerializationModule>();

            //containerBuilder.RegisterModule<ReferenceDataOrchestrationServicesModule>();
            containerBuilder.RegisterModule(new IOModule(azureStorageFileServiceConfiguration, ioConfiguration));
            containerBuilder.RegisterModule<RepositoryModule>();
            containerBuilder.RegisterModule<ReferenceDataPopulationModule>();
            containerBuilder.RegisterModule<MapperModule>();

            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler<JobContextMessage>>();

            return containerBuilder;
        }
    }
}
