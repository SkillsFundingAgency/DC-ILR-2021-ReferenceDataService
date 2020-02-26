using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Modules
{
    public class ReferenceDataInputModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ZipArchiveJsonFileService>().As<IZipArchiveFileService>();
            containerBuilder.RegisterType<ReferenceInputDataMapperService>().As<IReferenceInputDataMapperService>();
        }
    }
}
