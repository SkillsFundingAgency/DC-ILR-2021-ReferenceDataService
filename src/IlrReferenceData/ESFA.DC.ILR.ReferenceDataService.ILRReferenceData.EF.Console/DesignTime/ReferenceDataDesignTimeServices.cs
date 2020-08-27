using ESFA.DC.Data.EF.DesignTime;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.EF.Console.DesignTime
{
    public class ReferenceDataDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPluralizer, DefaultPluralizer>();
        }
    }
}
