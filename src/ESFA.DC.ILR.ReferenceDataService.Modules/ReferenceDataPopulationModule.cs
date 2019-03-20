using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population;
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
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>>>().As<IReferenceDataService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>>>().As<IReferenceDataService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyDictionary<string, FcsContractAllocation>, int>>().As<IReferenceDataService<IReadOnlyDictionary<string, FcsContractAllocation>, int>>();
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>>>().As<IReferenceDataService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>>>().As<IReferenceDataService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<ReferenceMetaDataService>().As<IReferenceMetaDataService>();
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>>>().As<IReferenceDataService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>>>().As<IReferenceDataService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<ReferenceDataService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>>().As<IReferenceDataService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>>();

            containerBuilder.RegisterType<EmployersService>().As<IRetrievalService<IReadOnlyDictionary<int, Employer>, IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<EpaOrganisationsService>().As<IRetrievalService<IReadOnlyDictionary<string, List<EPAOrganisation>>, IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<FcsService>().As<IRetrievalService<IReadOnlyDictionary<string, FcsContractAllocation>, int>>();
            containerBuilder.RegisterType<LarsStandardService>().As<IRetrievalService<IReadOnlyDictionary<int, LARSStandard>, IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<LarsLearningDeliveryService>().As<IRetrievalService<IReadOnlyDictionary<string, LARSLearningDelivery>, IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<MetaDataService>().As<IMetaDataRetrievalService>();
            containerBuilder.RegisterType<OrganisationsService>().As<IRetrievalService<IReadOnlyDictionary<int, Organisation>, IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<PostcodesService>().As<IRetrievalService<IReadOnlyDictionary<string, Postcode>, IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<UlnService>().As<IRetrievalService<IReadOnlyCollection<ULN>, IReadOnlyCollection<long>>>();
        }
    }
}
