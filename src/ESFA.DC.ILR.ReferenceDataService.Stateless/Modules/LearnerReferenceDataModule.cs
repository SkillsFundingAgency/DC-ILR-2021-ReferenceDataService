using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class LearnerReferenceDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LearnerReferenceDataTask>().Keyed<ITask>(TaskKeys.LearnerReferenceData);

            builder.RegisterType<LearnerDataRetrievalService>().As<ILearnerDataRetrievalService>();
            builder.RegisterType<LearnerReferenceDataRepositoryService>().As<ILearnerReferenceDataRepositoryService>();
        }
    }
}
