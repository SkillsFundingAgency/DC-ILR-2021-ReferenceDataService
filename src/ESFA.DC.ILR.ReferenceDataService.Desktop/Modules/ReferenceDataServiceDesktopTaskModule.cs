using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Modules;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Modules
{
    public class ReferenceDataServiceDesktopTaskModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterModule<MapperModule>();
            containerBuilder.RegisterModule<DesktopReferenceDataMapperModule>();
            containerBuilder.RegisterType<DesktopReferenceDataFileRetrievalService>().As<IDesktopReferenceDataFileRetrievalService>();
            containerBuilder.RegisterType<DesktopReferenceDataMapperService>().As<IReferenceDataPopulationService>();
        }
    }
}
