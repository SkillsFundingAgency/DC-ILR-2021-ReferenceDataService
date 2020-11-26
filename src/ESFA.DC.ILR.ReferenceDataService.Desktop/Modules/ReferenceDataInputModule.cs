using Autofac;
using ESFA.DC.FileService;
using ESFA.DC.FileService.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Modules
{
    public class ReferenceDataInputModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<CommandLineMessengerService>().As<IMessengerService>().SingleInstance();
            containerBuilder.RegisterType<EFModelIdentityAssigner>().As<IEFModelIdentityAssigner>();
            containerBuilder.RegisterType<ReferenceInputEFMapper>().As<IReferenceInputEFMapper>();
            containerBuilder.RegisterType<ReferenceInputPersistence>().As<IReferenceInputPersistence>();

            containerBuilder.RegisterType<FileSystemFileService>().As<IFileService>();
            containerBuilder.RegisterType<ZipArchiveJsonFileService>().As<IZipArchiveFileService>();
            containerBuilder.RegisterType<ReferenceInputDataMapperService>().As<IReferenceInputDataMapperService>();
            containerBuilder.RegisterType<ReferenceInputTruncator>().As<IReferenceInputTruncator>();
            containerBuilder.RegisterType<ReferenceInputDataPopulationService>().As<IReferenceInputDataPopulationService>();
        }
    }
}
