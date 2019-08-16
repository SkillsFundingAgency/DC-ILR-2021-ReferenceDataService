using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktoptopReferenceData.Repository
{
    public class DesktopDevolvedPostcodesRepositoryService : IDesktopReferenceDataRepositoryService<DevolvedPostcodes>
    {
        private readonly IDbContextFactory<IPostcodesContext> _postcodesContextFactory;
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IJsonSerializationService _jsonSerializationService;

        public DesktopDevolvedPostcodesRepositoryService(
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IReferenceDataOptions referenceDataOptions,
            IJsonSerializationService jsonSerializationService)
        {
            _postcodesContextFactory = postcodesContextFactory;
            _referenceDataOptions = referenceDataOptions;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<DevolvedPostcodes> RetrieveAsync(CancellationToken cancellationToken)
        {
            using (var context = _postcodesContextFactory.Create())
            {
                var mcaGlaSofLookups = await RetrieveMcaGlaLookups(context, cancellationToken);

                var devolvedPostcodesList = await RetrieveDevolvedPostcodes(cancellationToken);

                return new DevolvedPostcodes
                {
                    McaGlaSofLookups = mcaGlaSofLookups,
                    Postcodes = devolvedPostcodesList
                };
            }
        }

        public async Task<List<McaGlaSofLookup>> RetrieveMcaGlaLookups(IPostcodesContext context, CancellationToken cancellationToken)
        {
            var mcaGlaFullNames = await context.McaglaFullNames?
                            .Where(e => e.EffectiveTo == null)
                            .ToDictionaryAsync(
                            k => k.McaglaShortCode,
                            v => v.FullName,
                            StringComparer.OrdinalIgnoreCase,
                            cancellationToken);

            var mcaGlaSofCodes = await context.McaglaSofs?.ToListAsync(cancellationToken);

            return mcaGlaSofCodes.Select(m => new McaGlaSofLookup
            {
                SofCode = m.SofCode,
                McaGlaShortCode = m.McaglaShortCode,
                McaGlaFullName = mcaGlaFullNames.TryGetValue(m.McaglaShortCode, out var fullname) ? fullname : string.Empty,
                EffectiveFrom = m.EffectiveFrom,
                EffectiveTo = m.EffectiveTo
            }).ToList();
        }

        public async Task<List<DevolvedPostcode>> RetrieveDevolvedPostcodes(CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT
                                       D.[Postcode]
                                      ,D.[Area]
                                      ,D.[SourceOfFunding]
                                      ,D.[EffectiveFrom]
                                      ,D.[EffectiveTo]
                                FROM [dbo].[DevolvedPostcodesDataset] D 
                                WHERE D.[SourceOfFunding] IS NOT NULL";

            var postcodes = await ExectueSqlAsync<DevolvedPostcode>(sqlSfaAreaCost, cancellationToken);

            return postcodes.ToList();
        }

        public virtual async Task<IEnumerable<T>> ExectueSqlAsync<T>(string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.PostcodesConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<T>(commandDefinition);
            }
        }
    }
}
