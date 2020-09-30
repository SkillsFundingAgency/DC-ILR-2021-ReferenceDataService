using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Service;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class ReferenceDataRepositoryServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<IlrReferenceDataRepositoryService>().As<IIlrReferenceDataRepositoryService>();
            containerBuilder.RegisterType<ReferenceDataStatisticsService>().As<IReferenceDataStatisticsService>();
        }
    }
}
