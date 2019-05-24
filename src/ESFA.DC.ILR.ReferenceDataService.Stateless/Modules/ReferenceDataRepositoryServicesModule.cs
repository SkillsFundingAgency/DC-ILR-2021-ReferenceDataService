using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class ReferenceDataRepositoryServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MetaDataRetrievalService>().As<IMetaDataRetrievalService>();
            containerBuilder.RegisterType<IlrReferenceDataRepositoryService>().As<IIlrReferenceDataRepositoryService>();
        }
    }
}
