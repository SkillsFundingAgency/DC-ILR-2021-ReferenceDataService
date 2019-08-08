using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.AppEarningsHistory;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.EAS;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules
{
    public class IlrMessageReferenceDataRepositoryServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MetaDataRetrievalService>().As<IMetaDataRetrievalService>();
            containerBuilder.RegisterType<AppEarningsHistoryRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<ApprenticeshipEarningsHistory>>>();
            containerBuilder.RegisterType<EasRepositoryService>().As<IReferenceDataRetrievalService<int, IReadOnlyCollection<EasFundingLine>>>();
            containerBuilder.RegisterType<DevolvedPostcodesRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<string>, DevolvedPostcodes>>();
            containerBuilder.RegisterType<EmployersRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>>>();
            containerBuilder.RegisterType<EpaOrganisationsRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>>>();
            containerBuilder.RegisterType<FcsRepositoryService>().As<IReferenceDataRetrievalService<int, IReadOnlyCollection<FcsContractAllocation>>>();
            containerBuilder.RegisterType<LarsStandardRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>>>();
            containerBuilder.RegisterType<LarsLearningDeliveryRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>>();
            containerBuilder.RegisterType<OrganisationsRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>>>();
            containerBuilder.RegisterType<PostcodesRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>>>();
            containerBuilder.RegisterType<UlnRepositoryService>().As<IReferenceDataRetrievalService<IReadOnlyCollection<long>, IReadOnlyCollection<long>>>();
        }
    }
}
