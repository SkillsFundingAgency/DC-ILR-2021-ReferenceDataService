using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class MetaDataService : IMetaDataRetrievalService
    {
        private const string EmployersVersionName = "Employers Version";
        private const string LarsVersionName = "LARS Version";
        private const string OrganisationsVersionName = "Organisations Version";
        private const string PostcodesVersionName = "Potcodes Version";

        private readonly IEmployersContext _employersContext;
        private readonly ILARSContext _larsContext;
        private readonly IOrganisationsContext _organisationsContext;
        private readonly IPostcodesContext _postcodesContext;

        public MetaDataService(
            IEmployersContext employersContext,
            ILARSContext larsContext,
            IOrganisationsContext organisationsContext,
            IPostcodesContext postcodesContext)
        {
            _employersContext = employersContext;
            _larsContext = larsContext;
            _organisationsContext = organisationsContext;
            _postcodesContext = postcodesContext;
        }

        public async Task<MetaData> RetrieveAsync(CancellationToken cancellationToken)
        {
            var referenceDataVersions = new List<ReferenceDataVersion>();

            var versionsDictionary = await _larsContext.LARS_Versions.OrderByDescending(v => v.MainDataSchemaName).Select(v => new ReferenceDataVersion(LarsVersionName, v.MainDataSchemaName))
                   .Union(_organisationsContext.OrgVersions.OrderByDescending(v => v.MainDataSchemaName).Select(v => new ReferenceDataVersion(OrganisationsVersionName, v.MainDataSchemaName)))
                   .Union(_postcodesContext.VersionInfos.OrderByDescending(v => v.VersionNumber).Select(v => new ReferenceDataVersion(PostcodesVersionName, v.VersionNumber)))
                   .Union(_employersContext.LargeEmployerSourceFiles.OrderByDescending(v => v.Id).Select(v => new ReferenceDataVersion(EmployersVersionName, v.Id.ToString())))
                   .GroupBy(n => n.Name)
                   .ToDictionaryAsync(k => k.Key, v => v, cancellationToken)

            foreach (var key in versionsDictionary)
            {
                referenceDataVersions.Add(key.Value.OrderByDescending(v => v.Version).FirstOrDefault());
            }

            return new MetaData
            {
                ReferenceDataVersions = referenceDataVersions
            };
        }
    }
}
