using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Constants;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Abstract
{
    public abstract class AbstractMetaDataRetrievalService
    {
        private readonly IDbContextFactory<IEmployersContext> _employersContextFactory;
        private readonly IDbContextFactory<ILARSContext> _larsContextFactory;
        private readonly IDbContextFactory<IOrganisationsContext> _organisationsContextFactory;
        private readonly IDbContextFactory<IPostcodesContext> _postcodesContextFactory;
        private readonly IIlrReferenceDataRepositoryService _ilReferenceDataRepositoryService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AbstractMetaDataRetrievalService(
            IDbContextFactory<IEmployersContext> employersContextFactory,
            IDbContextFactory<ILARSContext> larsContextFactory,
            IDbContextFactory<IOrganisationsContext> organisationsContextFactory,
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IIlrReferenceDataRepositoryService ilReferenceDataRepositoryService,
            IDateTimeProvider dateTimeProvider)
        {
            _employersContextFactory = employersContextFactory;
            _larsContextFactory = larsContextFactory;
            _organisationsContextFactory = organisationsContextFactory;
            _postcodesContextFactory = postcodesContextFactory;
            _ilReferenceDataRepositoryService = ilReferenceDataRepositoryService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<MetaData> RetrieveAsync(CancellationToken cancellationToken)
        {
            var metaData = await _ilReferenceDataRepositoryService.RetrieveIlrReferenceDataAsync(cancellationToken);
            var orgRefDataVersions = await RetrieveOrganisationsReferenceDataVersionsAsync(cancellationToken);
            var postcodeRefDataVersions = await RetrievePostcodesReferenceDataVersionsAsync(cancellationToken);

            metaData.DateGenerated = _dateTimeProvider.GetNowUtc();
            metaData.ReferenceDataVersions = new ReferenceDataVersion
            {
                CoFVersion = orgRefDataVersions.CoFVersion,
                CampusIdentifierVersion = orgRefDataVersions.CampusIdentifierVersion,
                Employers = await RetrieveEmployersVersionAsync(cancellationToken),
                LarsVersion = await RetrieveLarsVersionAsync(cancellationToken),
                OrganisationsVersion = orgRefDataVersions.OrganisationsVersion,
                PostcodesVersion = postcodeRefDataVersions.PostcodesVersion,
                DevolvedPostcodesVersion = postcodeRefDataVersions.DevolvedPostcodesVersion,
                HmppPostcodesVersion = postcodeRefDataVersions.HmppPostcodesVersion,
                PostcodeFactorsVersion = postcodeRefDataVersions.PostcodeFactorsVersion,
            };
            metaData.SchemaVersion = GetAssemblyVersion();

            return Validate(metaData);
        }

        private string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetReferencedAssemblies().First(a => a.Name == "ESFA.DC.ILR.ReferenceDataService.Model").Version.ToString(3);
        }

        private async Task<EmployersVersion> RetrieveEmployersVersionAsync(CancellationToken cancellationToken)
        {
            using (var context = _employersContextFactory.Create())
            {
                return await context.LargeEmployerSourceFiles
                        .OrderByDescending(v => v.Id)
                        .Select(v => new EmployersVersion { Version = v.Id.ToString() })
                        .FirstOrDefaultAsync(cancellationToken) ?? new EmployersVersion();
            }
        }

        private async Task<LarsVersion> RetrieveLarsVersionAsync(CancellationToken cancellationToken)
        {
            using (var context = _larsContextFactory.Create())
            {
                return await context.LARS_Versions
                        .OrderByDescending(v => v.MainDataSchemaName)
                        .Select(v => new LarsVersion { Version = v.MainDataSchemaName })
                        .FirstOrDefaultAsync(cancellationToken) ?? new LarsVersion();
            }
        }

        private async Task<ReferenceDataVersion> RetrieveOrganisationsReferenceDataVersionsAsync(CancellationToken cancellationToken)
        {
            using (var context = _organisationsContextFactory.Create())
            {
                var orgVersion = (await context.Versions.FirstOrDefaultAsync(x => x.Source.CaseInsensitiveEquals(VersionSourceConstants.Organisation)))?.VersionNumber;
                var campusIdVersion = (await context.Versions.FirstOrDefaultAsync(x => x.Source.CaseInsensitiveEquals(VersionSourceConstants.CampusIdentifier)))?.VersionNumber;
                var cofVersion = (await context.Versions.FirstOrDefaultAsync(x => x.Source.CaseInsensitiveEquals(VersionSourceConstants.ConditionOfFunding)))?.VersionNumber;

                return new ReferenceDataVersion
                {
                    OrganisationsVersion = new OrganisationsVersion { Version = orgVersion },
                    CampusIdentifierVersion = new CampusIdentifierVersion { Version = campusIdVersion },
                    CoFVersion = new CoFVersion { Version = cofVersion },
                };
            }
        }

        private async Task<ReferenceDataVersion> RetrievePostcodesReferenceDataVersionsAsync(CancellationToken cancellationToken)
        {
            using (var context = _postcodesContextFactory.Create())
            {
                var postcodesVersion = (await context.VersionInfos.FirstOrDefaultAsync(x => x.DataSource.CaseInsensitiveEquals(VersionSourceConstants.OnsPostcodes)))?.VersionNumber;
                var hmppPostcodesVersion = (await context.VersionInfos.FirstOrDefaultAsync(x => x.DataSource.CaseInsensitiveEquals(VersionSourceConstants.HmppPostcodes)))?.VersionNumber;
                var devolvedPostcodesVersion = (await context.VersionInfos.FirstOrDefaultAsync(x => x.DataSource.CaseInsensitiveEquals(VersionSourceConstants.DevolvedPostcodes)))?.VersionNumber;
                var postcodeFactorsVersion = (await context.VersionInfos.FirstOrDefaultAsync(x => x.DataSource.CaseInsensitiveEquals(VersionSourceConstants.PostcodeFactors)))?.VersionNumber;

                return new ReferenceDataVersion
                {
                    PostcodesVersion = new PostcodesVersion { Version = postcodesVersion },
                    PostcodeFactorsVersion = new PostcodeFactorsVersion { Version = postcodeFactorsVersion },
                    HmppPostcodesVersion = new HmppPostcodesVersion { Version = hmppPostcodesVersion },
                    DevolvedPostcodesVersion = new DevolvedPostcodesVersion { Version = devolvedPostcodesVersion }
                };
            }
        }

        private MetaData Validate(MetaData metaData)
        {
            var referenceDataSources = new List<(string, bool)>
            {
                (DataSourceConstants.Employers, metaData.ReferenceDataVersions.Employers.Version != null),
                (DataSourceConstants.Lars, metaData.ReferenceDataVersions.LarsVersion.Version != null),
                (DataSourceConstants.Organisations, metaData.ReferenceDataVersions.OrganisationsVersion.Version != null),
                (DataSourceConstants.Postcodes, metaData.ReferenceDataVersions.PostcodesVersion.Version != null),
                (DataSourceConstants.DevolvedPostcodes, metaData.ReferenceDataVersions.DevolvedPostcodesVersion.Version != null),
                (DataSourceConstants.PostcodeFactors, metaData.ReferenceDataVersions.PostcodeFactorsVersion.Version != null),
                (DataSourceConstants.HmppPostcodes, metaData.ReferenceDataVersions.HmppPostcodesVersion.Version != null),
                (DataSourceConstants.CampusIdentifiers, metaData.ReferenceDataVersions.CampusIdentifierVersion.Version != null),
                (DataSourceConstants.CoF, metaData.ReferenceDataVersions.CoFVersion.Version != null),
                (DataSourceConstants.ValidationErrors, metaData.ValidationErrors.Any()),
                (DataSourceConstants.ValidationRules, metaData.ValidationRules.Any()),
                (DataSourceConstants.ValidationLookups, metaData.Lookups.Any()),
            };

            if (!referenceDataSources.Any(x => x.Item2 == false))
            {
                return metaData;
            }
            else
            {
                var sources = referenceDataSources.Where(x => x.Item2 == false).Select(x => x.Item1).Aggregate((x, y) => x + ", " + y);

                throw new ArgumentOutOfRangeException("MetaData Retrieval Error - Reference Dataset incomplete. Cannot find version information for dataset specified. The dataset may be incomplete. Please check the following data sources: " + sources);
            }
        }
    }
}
