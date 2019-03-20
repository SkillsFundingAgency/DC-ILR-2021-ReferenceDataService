﻿using System.Collections.Generic;

namespace ESFA.DC.ILR.ServiceFabric.Common.Config.Interface
{
    public interface IServiceFabricConfigurationService
    {
        IDictionary<string, string> GetConfigSectionAsDictionary(string sectionName);

        T GetConfigSectionAs<T>(string sectionName);

        IStatelessServiceConfiguration GetConfigSectionAsStatelessServiceConfiguration();
    }
}
