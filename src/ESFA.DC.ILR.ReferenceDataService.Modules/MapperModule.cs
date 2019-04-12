using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EmpIdMapper>().As<IEmpIdMapper>();
            containerBuilder.RegisterType<EpaOrgIdMapper>().As<IEpaOrgIdMapper>();
            containerBuilder.RegisterType<FM36UlnMapper>().As<IFM36UlnMapper>();
            containerBuilder.RegisterType<LARSLearningDeliveryKeyMapper>().As<ILARSLearningDeliveryKeyMapper>();
            containerBuilder.RegisterType<LearningProviderUkprnMapper>().As<ILearningProviderUkprnMapper>();
            containerBuilder.RegisterType<PostcodesMapper>().As<IPostcodesMapper>();
            containerBuilder.RegisterType<StandardCodesMapper>().As<IStandardCodesMapper>();
            containerBuilder.RegisterType<UkprnsMapper>().As<IUkprnsMapper>();
            containerBuilder.RegisterType<UlnMapper>().As<IUlnMapper>();
        }
    }
}
