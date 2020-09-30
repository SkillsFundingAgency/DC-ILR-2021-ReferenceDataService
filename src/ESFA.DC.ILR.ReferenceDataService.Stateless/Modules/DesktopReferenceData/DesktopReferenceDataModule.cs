using Autofac;
using Autofac.Core;
using ESFA.DC.CsvService;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Model;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules.DesktopReferenceData
{
    public class DesktopReferenceDataModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<DesktopReferenceDataRepositoryServicesModule>();
            containerBuilder.RegisterType<DesktopReferenceDataSummaryReport>().As<IDesktopReferenceDataSummaryReport>();
            containerBuilder.RegisterType<DesktopReferenceDataPopulationService>().As<IDesktopReferenceDataPopulationService>();
            containerBuilder.RegisterType<DesktopReferenceDataFileService>().As<IDesktopReferenceDataFileService>();
            containerBuilder.RegisterType<DesktopReferenceDataSummaryFileService>().As<IDesktopReferenceDataSummaryFileService>();
            containerBuilder.RegisterType<DesktopReferenceDataFileNameService>().As<IDesktopReferenceDataFileNameService>();
            containerBuilder.RegisterType<CsvFileService>().As<ICsvFileService>();
            containerBuilder.RegisterType<ZipFileService>().As<IZipFileService>();
            containerBuilder.RegisterType<DesktopReferenceDataTask>().Keyed<ITask>(TaskKeys.DesktopReferenceData);
        }
    }
}
