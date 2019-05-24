using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Desktop.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules.Desktop
{
    public class DesktopReferenceDataModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<DesktopReferenceDataRepositoryServicesModule>();
            containerBuilder.RegisterType<DesktopReferenceDataPopulationService>().As<IDesktopReferenceDataPopulationService>();
            containerBuilder.RegisterType<DesktopReferenceDataTask>().Keyed<ITask>(TaskKeys.DesktopReferenceData);
        }
    }
}
