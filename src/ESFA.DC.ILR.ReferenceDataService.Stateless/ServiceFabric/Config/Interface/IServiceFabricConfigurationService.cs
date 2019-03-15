using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.ServiceFabric.Config.Interface
{
    public interface IServiceFabricConfigurationService
    {
        IDictionary<string, string> GetConfigSectionAsDictionary(string sectionName);

        T GetConfigSectionAs<T>(string sectionName);

        IStatelessServiceConfiguration GetConfigSectionAsStatelessServiceConfiguration();
    }
}
