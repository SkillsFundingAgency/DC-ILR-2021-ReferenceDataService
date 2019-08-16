using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.EAS1920.EF;
using ESFA.DC.EAS1920.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class MetaDataRetrievalService : IMetaDataRetrievalService
    {
        private const string EasLastUpdated = "EAS Last Updated";
        private const string EmployersVersionName = "Employers Version";
        private const string LarsVersionName = "LARS Version";
        private const string OrganisationsVersionName = "Organisations Version";
        private const string PostcodesVersionName = "Potcodes Version";

        private readonly IDbContextFactory<IEasdbContext> _easContextFactory;
        private readonly IDbContextFactory<IEmployersContext> _employersContextFactory;
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;
        private readonly IDbContextFactory<IOrganisationsContext> _organisationsContextFactory;
        private readonly IDbContextFactory<IPostcodesContext> _postcodesContextFactory;
        private readonly IIlrReferenceDataRepositoryService _ilReferenceDataRepositoryService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MetaDataRetrievalService(
            IDbContextFactory<IEasdbContext> easContextFactory,
            IDbContextFactory<IEmployersContext> employersContextFactory,
            IDbContextFactory<ILARSContext> larsContextFactory,
            IDbContextFactory<IOrganisationsContext> organisationsContextFactory,
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IIlrReferenceDataRepositoryService ilReferenceDataRepositoryService,
            IDateTimeProvider dateTimeProvider)
        {
            _easContextFactory = easContextFactory;
            _employersContextFactory = employersContextFactory;
            _larsContextFactory = larsContextFactory;
            _organisationsContextFactory = organisationsContextFactory;
            _postcodesContextFactory = postcodesContextFactory;
            _ilReferenceDataRepositoryService = ilReferenceDataRepositoryService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<MetaData> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            var metaData = await _ilReferenceDataRepositoryService.RetrieveIlrReferenceDataAsync(cancellationToken);
            var latestEAS = await RetrieveLatestEasAsync(ukprn, cancellationToken);

            metaData.DateGenerated = _dateTimeProvider.GetNowUtc();
            metaData.ReferenceDataVersions = new ReferenceDataVersion
            {
                Employers = await RetrieveEmployersVersionAsync(cancellationToken),
                LarsVersion = await RetrieveLarsVersionAsync(cancellationToken),
                OrganisationsVersion = await RetrieveOrganisationsVersionAsync(cancellationToken),
                PostcodesVersion = await RetrievePostcodesVersionAsync(cancellationToken),
                EasUploadDateTime = new EasUploadDateTime(latestEAS?.UpdatedOn),
            };

            return Validate(metaData);
        }

        private async Task<EasSubmission> RetrieveLatestEasAsync(int ukprn, CancellationToken cancellationToken)
        {
            using (var context = _easContextFactory.Create())
            {
                return await context.EasSubmissions.FirstOrDefaultAsync(v => v.Ukprn == ukprn.ToString(), cancellationToken);
            }
        }

        private async Task<EmployersVersion> RetrieveEmployersVersionAsync(CancellationToken cancellationToken)
        {
            using (var context = _employersContextFactory.Create())
            {
                return await context.LargeEmployerSourceFiles
                        .OrderByDescending(v => v.Id)
                        .Select(v => new EmployersVersion(v.Id.ToString()))
                        .FirstOrDefaultAsync(cancellationToken);
            }
        }

        private async Task<LarsVersion> RetrieveLarsVersionAsync(CancellationToken cancellationToken)
        {
            using (var context = _larsContextFactory.Create())
            {
                return await context.LARS_Versions
                        .OrderByDescending(v => v.MainDataSchemaName)
                        .Select(v => new LarsVersion(v.MainDataSchemaName))
                        .FirstOrDefaultAsync(cancellationToken);
            }
        }

        private async Task<OrganisationsVersion> RetrieveOrganisationsVersionAsync(CancellationToken cancellationToken)
        {
            using (var context = _organisationsContextFactory.Create())
            {
                return await context.OrgVersions
                        .OrderByDescending(v => v.MainDataSchemaName)
                        .Select(v => new OrganisationsVersion(v.MainDataSchemaName))
                        .FirstOrDefaultAsync(cancellationToken);
            }
        }

        private async Task<PostcodesVersion> RetrievePostcodesVersionAsync(CancellationToken cancellationToken)
        {
            using (var context = _postcodesContextFactory.Create())
            {
                return await context.VersionInfos
                        .OrderByDescending(v => v.VersionNumber)
                        .Select(v => new PostcodesVersion(v.VersionNumber))
                        .FirstOrDefaultAsync(cancellationToken);
            }
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
