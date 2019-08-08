using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Keys;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Mappers;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Modules
{
    internal class DesktopReferenceDataMapperModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MetaDataReferenceDataMapper>().As<IDesktopReferenceMetaDataMapper>();
            containerBuilder.RegisterType<DevolvedPostcodesReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<string>, DevolvedPostcodes>>();
            containerBuilder.RegisterType<EmployersReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>>>();
            containerBuilder.RegisterType<EpaOrganisationsReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<EPAOrganisation>>>();
            containerBuilder.RegisterType<LarsStandardReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<LARSStandard>>>();
            containerBuilder.RegisterType<LarsLearningDeliveryReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<LARSLearningDeliveryKey>, IReadOnlyCollection<LARSLearningDelivery>>>();
            containerBuilder.RegisterType<OrganisationsReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<int>, IReadOnlyCollection<Organisation>>>();
            containerBuilder.RegisterType<PostcodesReferenceDataMapper>().As<IDesktopReferenceDataMapper<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>>>();
        }
    }
}
