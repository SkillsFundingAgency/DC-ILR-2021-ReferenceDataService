﻿using System;
using Autofac;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.FileService.Config;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Config;
using ESFA.DC.ILR.ReferenceDataService.Modules;
using ESFA.DC.ILR.ReferenceDataService.Service.Clients;
using ESFA.DC.ILR.ReferenceDataService.Stateless.Config;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.ServiceFabric.Common.Config;
using ESFA.DC.ServiceFabric.Common.Modules;
using Polly;

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
            var desktopRefDataConfig = serviceFabricConfigurationService.GetConfigSectionAs<DesktopReferenceDataConfiguration>("DesktopReferenceDataSection");
            containerBuilder.RegisterInstance(desktopRefDataConfig).As<IDesktopReferenceDataConfiguration>();

            var featureConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<FeatureConfiguration>("FeatureConfiguration");
            containerBuilder.RegisterInstance(featureConfiguration).As<FeatureConfiguration>().SingleInstance();

            var apiSettings = serviceFabricConfigurationService.GetConfigSectionAs<ApiSettings>("ApiSettings");
            containerBuilder.RegisterInstance(apiSettings).As<ApiSettings>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var jitter = new Random();

                return Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                        + TimeSpan.FromMilliseconds(jitter.Next(0, 100)));
            }).As<IAsyncPolicy>().SingleInstance();

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