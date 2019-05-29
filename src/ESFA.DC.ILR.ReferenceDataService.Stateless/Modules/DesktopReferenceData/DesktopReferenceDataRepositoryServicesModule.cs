using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Modules.DesktopReferenceData
{
    public class DesktopReferenceDataRepositoryServicesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DesktopEmployersRepositoryService>().As<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Employer>>>();
            containerBuilder.RegisterType<DesktopEpaOrganisationsRepositoryService>().As<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<EPAOrganisation>>>();
            containerBuilder.RegisterType<DesktopLarsStandardRepositoryService>().As<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSStandard>>>();
            containerBuilder.RegisterType<DesktopLarsLearningDeliveryRepositoryService>().As<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSLearningDelivery>>>();
            containerBuilder.RegisterType<DesktopLarsFrameworkRepositoryService>().As<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<LARSFramework>>>();
            containerBuilder.RegisterType<DesktopOrganisationsRepositoryService>().As<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Organisation>>>();
            containerBuilder.RegisterType<DesktopPostcodesRepositoryService>().As<IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>>>();
        }
    }
}
