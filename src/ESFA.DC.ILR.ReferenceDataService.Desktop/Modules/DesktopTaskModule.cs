using Autofac;
using ESFA.DC.ILR.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Modules;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Modules
{
    public class DesktopTaskModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterModule<MapperModule>();
            containerBuilder.RegisterType<DesktopReferenceDataFileRetrievalService>().As<IDesktopReferenceDataFileRetrievalService>();
            containerBuilder.RegisterType<DesktopReferenceDataMapperService>().As<IReferenceDataPopulationService>();
            containerBuilder.RegisterType<DesktopTask>().Keyed<IDesktopTask>(TaskKeys.DesktopMessageTask);
        }
    }
}
