using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ValidationService.Data.Population.Configuration;
using ESFA.DC.ReferenceData.Organisations.Model;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ServiceFabric.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var configHelper = new ConfigurationHelper();

            var referenceDataOptions = configHelper.GetSectionValues<ReferenceDataOptions>("ReferenceDataSection");
            containerBuilder.RegisterInstance(referenceDataOptions).As<IReferenceDataOptions>().SingleInstance();

            containerBuilder.Register(c =>
            {
                DbContextOptions<OrganisationsContext> options = new DbContextOptionsBuilder<OrganisationsContext>()
                    .UseSqlServer(c.Resolve<IReferenceDataOptions>().OrganisationsConnectionString).Options;

                return new OrganisationsContext(options);
            }).As<IOrganisationsContext>().InstancePerLifetimeScope();
        }
    }
}
