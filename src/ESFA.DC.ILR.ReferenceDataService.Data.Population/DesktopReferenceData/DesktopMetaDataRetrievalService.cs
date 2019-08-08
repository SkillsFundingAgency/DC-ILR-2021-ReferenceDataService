using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData
{
    public class DesktopMetaDataRetrievalService : IDesktopMetaDataRetrievalService
    {
        private const string EmployersVersionName = "Employers Version";
        private const string LarsVersionName = "LARS Version";
        private const string OrganisationsVersionName = "Organisations Version";
        private const string PostcodesVersionName = "Potcodes Version";

        private readonly IEmployersContext _employersContext;
        private readonly ILARSContext _larsContext;
        private readonly IOrganisationsContext _organisationsContext;
        private readonly IPostcodesContext _postcodesContext;
        private readonly IIlrReferenceDataRepositoryService _ilReferenceDataRepositoryService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DesktopMetaDataRetrievalService(
            IEmployersContext employersContext,
            ILARSContext larsContext,
            IOrganisationsContext organisationsContext,
            IPostcodesContext postcodesContext,
            IIlrReferenceDataRepositoryService ilReferenceDataRepositoryService,
            IDateTimeProvider dateTimeProvider)
        {
            _employersContext = employersContext;
            _larsContext = larsContext;
            _organisationsContext = organisationsContext;
            _postcodesContext = postcodesContext;
            _ilReferenceDataRepositoryService = ilReferenceDataRepositoryService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<MetaData> RetrieveAsync(CancellationToken cancellationToken)
        {
            var metaData = new MetaData
            {
                DateGenerated = _dateTimeProvider.GetNowUtc(),
                ReferenceDataVersions = new ReferenceDataVersion
                {
                    Employers =
                        await _employersContext.LargeEmployerSourceFiles
                        .OrderByDescending(v => v.Id)
                        .Select(v => new EmployersVersion(v.Id.ToString()))
                        .FirstOrDefaultAsync(cancellationToken),
                    LarsVersion = await _larsContext.LARS_Versions
                        .OrderByDescending(v => v.MainDataSchemaName)
                        .Select(v => new LarsVersion(v.MainDataSchemaName))
                        .FirstOrDefaultAsync(cancellationToken),
                    OrganisationsVersion = await _organisationsContext.OrgVersions
                        .OrderByDescending(v => v.MainDataSchemaName)
                        .Select(v => new OrganisationsVersion(v.MainDataSchemaName))
                        .FirstOrDefaultAsync(cancellationToken),
                    PostcodesVersion = await _postcodesContext.VersionInfos
                        .OrderByDescending(v => v.VersionNumber)
                        .Select(v => new PostcodesVersion(v.VersionNumber))
                        .FirstOrDefaultAsync(cancellationToken),
                },
                ValidationErrors = await _ilReferenceDataRepositoryService.RetrieveValidationErrorsAsync(cancellationToken),
                ValidationRules = await _ilReferenceDataRepositoryService.RetrieveValidationRulesAsync(cancellationToken),
                Lookups = await _ilReferenceDataRepositoryService.RetrieveLookupsAsync(cancellationToken),
                CollectionDates = _ilReferenceDataRepositoryService.RetrieveCollectionDates(),
            };

            return Validate(metaData);
        }

        private MetaData Validate(MetaData metaData)
        {
            if (
                metaData.ReferenceDataVersions.Employers.Version != null
               && metaData.ReferenceDataVersions.LarsVersion.Version != null
               && metaData.ReferenceDataVersions.OrganisationsVersion.Version != null
               && metaData.ValidationErrors.Any()
               && metaData.ValidationRules.Any()
               && metaData.Lookups.Any())
            {
                return metaData;
            }
            else
            {
                throw new ArgumentOutOfRangeException("MetaData Retrieval Error - Reference Dataset incomplete");
            }
        }
    }
}
