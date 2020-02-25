using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Transactions;
using ESFA.DC.ILR.ReferenceDataService.Service;
using ESFA.DC.ILR.ReferenceDataService.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Service.Tasks;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class ValidationMessagesTaskModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ValidationMessagesTask>().Keyed<ITask>(TaskKeys.ValidationMessages);
            containerBuilder.RegisterType<CsvRetrievalService>().As<ICsvRetrievalService>();
            containerBuilder.RegisterType<ValidationMessagesTransaction>().As<IValidationMessagesTransaction>();
            containerBuilder.RegisterType<BulkInsert>().As<IBulkInsert>();
        }
    }
}
