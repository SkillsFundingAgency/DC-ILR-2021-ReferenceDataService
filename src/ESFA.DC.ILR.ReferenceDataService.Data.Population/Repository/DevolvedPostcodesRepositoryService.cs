using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class DevolvedPostcodesRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<string>, DevolvedPostcodes>
    {
        private readonly IDbContextFactory<IPostcodesContext> _postcodesContextFactory;
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IJsonSerializationService _jsonSerializationService;

        public DevolvedPostcodesRepositoryService(
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IReferenceDataOptions referenceDataOptions,
            IJsonSerializationService jsonSerializationService)
        {
            _postcodesContextFactory = postcodesContextFactory;
            _referenceDataOptions = referenceDataOptions;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task<DevolvedPostcodes> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken)
        {
            using (var context = _postcodesContextFactory.Create())
            {
                var jsonParams = _jsonSerializationService.Serialize(input);

                var mcaGlaSofLookups = await RetrieveMcaGlaLookups(context, cancellationToken);

                var devolvedPostcodesList = await RetrieveDevolvedPostcodes(jsonParams, cancellationToken);

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

        public async Task<List<DevolvedPostcode>> RetrieveDevolvedPostcodes(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT
                                       D.[Postcode]
                                      ,D.[Area]
                                      ,D.[SourceOfFunding]
                                      ,D.[EffectiveFrom]
                                      ,D.[EffectiveTo]
                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                INNER JOIN [dbo].[DevolvedPostcodesDataset] D 
                                ON D.[Postcode] = P.[Postcode]
                                AND D.[SourceOfFunding] IS NOT NULL";

            var postcodes = await ExectueSqlAsync<DevolvedPostcode>(jsonParams, sqlSfaAreaCost, cancellationToken);

            return postcodes.ToList();
        }

        public virtual async Task<IEnumerable<T>> ExectueSqlAsync<T>(string jsonParams, string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.PostcodesConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, new { jsonParams }, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<T>(commandDefinition);
            }
        }
    }
}
