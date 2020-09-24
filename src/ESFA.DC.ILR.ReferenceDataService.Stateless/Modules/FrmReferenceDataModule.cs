using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class FrmReferenceDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FrmReferenceDataTask>().Keyed<ITask>(TaskKeys.FrmReferenceData);

            builder.RegisterType<FrmDataRetrievalService>().As<IFrmDataRetrievalService>();
            builder.RegisterType<FrmReferenceDataRepositoryService>().As<IFrmReferenceDataRepositoryService>();
        }
    }
}
