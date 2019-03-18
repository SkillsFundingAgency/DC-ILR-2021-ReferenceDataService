using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.ULNs;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class ReferenceDataPopulationModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EmployersService>().As<IMessageMapper<IRetrievalService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>>>>();
            containerBuilder.RegisterType<EpaOrganisationsService>().As<IMessageMapper<IRetrievalService<IReadOnlyDictionary<string, EPAOrganisation>, IReadOnlyCollection<string>>>>();
            containerBuilder.RegisterType<FcsService>().As<IMessageMapper<IRetrievalService<IReadOnlyDictionary<int, FcsContractAllocation>, IReadOnlyCollection<int>>>>();
            containerBuilder.RegisterType<LarsStandardService>().As<IMessageMapper<IRetrievalService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>>>>();
            containerBuilder.RegisterType<LarsLearningDeliveryService>().As<IMessageMapper<IRetrievalService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>>>>();
            containerBuilder.RegisterType<MetaDataService>().As<IMetaDataRetrievalService>();
            containerBuilder.RegisterType<OrganisationsService>().As<IMessageMapper<IRetrievalService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>>>>();
            containerBuilder.RegisterType<PostcodesService>().As<IMessageMapper<IRetrievalService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>>>>();
            containerBuilder.RegisterType<UlnService>().As<IMessageMapper<IRetrievalService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>>>();
        }
    }
}
