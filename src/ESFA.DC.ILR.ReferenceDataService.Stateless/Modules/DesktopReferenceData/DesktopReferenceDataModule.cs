using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules.DesktopReferenceData
{
    public class DesktopReferenceDataModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<DesktopReferenceDataRepositoryServicesModule>();
            containerBuilder.RegisterType<DesktopReferenceDataPopulationService>().As<IDesktopReferenceDataPopulationService>();
            containerBuilder.RegisterType<DesktopReferenceDataFileService>().As<IDesktopReferenceDataFileService>();
            containerBuilder.RegisterType<DesktopReferenceDataFileNameService>().As<IDesktopReferenceDataFileNameService>();
            containerBuilder.RegisterType<ZipFileService>().As<IZipFileService>();
            containerBuilder.RegisterType<DesktopReferenceDataTask>().Keyed<ITask>(TaskKeys.DesktopReferenceData);
        }
    }
}
