﻿using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class IlrMessageTaskModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<IlrMessageReferenceDataRepositoryServicesModule>();
            containerBuilder.RegisterType<IlrMessageTask>().Keyed<ITask>(TaskKeys.IlrMessage);
        }
    }
}