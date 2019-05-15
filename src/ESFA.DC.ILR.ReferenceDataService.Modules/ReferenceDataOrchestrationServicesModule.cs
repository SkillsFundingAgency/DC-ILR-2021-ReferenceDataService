using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers;
using ESFA.DC.ILR.ReferenceDataService.Service;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class ReferenceDataOrchestrationServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ReferenceDataOrchestrationService>().As<IReferenceDataOrchestrationService>();
            containerBuilder.RegisterType<ReferenceDataOutputService>().As<IReferenceDataOutputService>();
            containerBuilder.RegisterType<IlrMessageTaskProvider>().As<IIlrMessageTaskProvider>();
            containerBuilder.RegisterType<IlrMessageTask>().As<IIlrMessageTask>();
            containerBuilder.RegisterType<MessageProvider>().As<IMessageProvider>();
            containerBuilder.RegisterType<ReferenceDataPopulationService>().As<IReferenceDataPopulationService>();
            containerBuilder.RegisterType<MessageMapperService>().As<IMessageMapperService>();
        }
    }
}
