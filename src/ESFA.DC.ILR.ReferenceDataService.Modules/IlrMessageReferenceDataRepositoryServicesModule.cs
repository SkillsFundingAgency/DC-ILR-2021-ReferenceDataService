using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class IlrMessageReferenceDataRepositoryServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AppEarningsHistoryRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<long>, IReadOnlyCollection<ApprenticeshipEarningsHistory>>>();
            containerBuilder.RegisterType<EmployersRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>>>();
            containerBuilder.RegisterType<EpaOrganisationsRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>>>();
            containerBuilder.RegisterType<FcsRepositoryService>().As<IReferenceDataRepositoryService<int, IReadOnlyCollection<FcsContractAllocation>>>();
            containerBuilder.RegisterType<LarsStandardRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>>>();
            containerBuilder.RegisterType<LarsLearningDeliveryRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>>();
            containerBuilder.RegisterType<OrganisationsRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>>>();
            containerBuilder.RegisterType<PostcodesRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>>>();
            containerBuilder.RegisterType<UlnRepositoryService>().As<IReferenceDataRepositoryService<IReadOnlyCollection<long>, IReadOnlyCollection<long>>>();
        }
    }
}
