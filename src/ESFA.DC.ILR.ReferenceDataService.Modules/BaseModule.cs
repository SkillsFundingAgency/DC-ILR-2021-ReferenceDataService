using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Providers;
using ESFA.DC.ILR.ReferenceDataService.Providers.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class BaseModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<MapperModule>();
            containerBuilder.RegisterType<MessageProvider>().As<IMessageProvider>();
            containerBuilder.RegisterType<ReferenceDataPopulationService>().As<IReferenceDataPopulationService>();
            containerBuilder.RegisterType<MessageMapperService>().As<IMessageMapperService>();
            containerBuilder.RegisterType<FilePersister>().As<IFilePersister>();
            containerBuilder.RegisterType<AcademicYearDataService>().As<IAcademicYearDataService>();
        }
    }
}
