using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Providers;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class IlrMessageModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MessageProvider>().As<IMessageProvider>();
            containerBuilder.RegisterType<ReferenceDataPopulationService>().As<IReferenceDataPopulationService>();
            containerBuilder.RegisterType<MessageMapperService>().As<IMessageMapperService>();
            containerBuilder.RegisterType<IlrMessageTask>().Keyed<ITask>(TaskKeys.IlrMessage);

            containerBuilder.RegisterModule<MapperModule>();
            containerBuilder.RegisterModule<ReferenceDataRepositoryServicesModule>();
        }
    }
}
