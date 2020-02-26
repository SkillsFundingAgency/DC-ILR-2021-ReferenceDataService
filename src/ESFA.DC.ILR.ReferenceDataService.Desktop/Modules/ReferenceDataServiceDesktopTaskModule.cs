using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Modules;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Modules
{
    public class ReferenceDataServiceDesktopTaskModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterModule<MapperModule>();
            containerBuilder.RegisterType<DesktopContextReturnPeriodUpdateService>().As<IDesktopContextReturnPeriodUpdateService>();
            containerBuilder.RegisterType<ZipArchiveJsonFileService>().As<IZipArchiveFileService>();
            containerBuilder.RegisterType<DesktopReferenceDataPopulationService>().As<IReferenceDataPopulationService>();
            containerBuilder.RegisterType<DesktopReferenceDataMapperService>().As<IDesktopReferenceDataMapperService>();
            containerBuilder.RegisterType<LarsLearningDeliveryReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>>();
        }
    }
}
