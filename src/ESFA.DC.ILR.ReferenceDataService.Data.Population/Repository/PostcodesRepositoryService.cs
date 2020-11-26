using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class PostcodesRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<string>, IReadOnlyCollection<Postcode>>
    {
        private readonly IDbContextFactory<IPostcodesContext> _postcodesContextFactory;
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IJsonSerializationService _jsonSerializationService;
        private readonly IPostcodesEntityModelMapper _postcodesEntityModelMapper;

        public PostcodesRepositoryService(
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IReferenceDataOptions referenceDataOptions,
            IJsonSerializationService jsonSerializationService,
            IPostcodesEntityModelMapper postcodesEntityModelMapper)
        {
            _postcodesContextFactory = postcodesContextFactory;
            _referenceDataOptions = referenceDataOptions;
            _jsonSerializationService = jsonSerializationService;
            _postcodesEntityModelMapper = postcodesEntityModelMapper;
        }

        public async Task<IReadOnlyCollection<Postcode>> RetrieveAsync(IReadOnlyCollection<string> input, CancellationToken cancellationToken)
        {
            var jsonParams = _jsonSerializationService.Serialize(input);

            using (var context = _postcodesContextFactory.Create())
            {
                var masterPostcodes = await RetrieveMasterPostcodes(jsonParams, cancellationToken);
                var sfaAreaCosts = await RetrieveSfaAreaCosts(jsonParams, cancellationToken);
                var sfaPostcodeDisadvantages = await RetrieveSfaPostcodeDisadvantages(jsonParams, cancellationToken);
                var efaPostcodeDisadvantages = await RetrieveEfaPostcodeDisadvantages(jsonParams, cancellationToken);
                var dasPostcodeDisadvantages = await RetrieveDasPostcodeDisadvantages(jsonParams, cancellationToken);
                var onsData = await RetrieveOnsData(jsonParams, cancellationToken);

                return masterPostcodes
                    .Select(postcode => new Postcode()
                    {
                        PostCode = postcode,
                        SfaDisadvantages = sfaPostcodeDisadvantages.TryGetValue(postcode, out var sfaDisad) ? sfaPostcodeDisadvantages[postcode] : null,
                        SfaAreaCosts = sfaAreaCosts.TryGetValue(postcode, out var sfaAreaCost) ? sfaAreaCosts[postcode] : null,
                        DasDisadvantages = dasPostcodeDisadvantages.TryGetValue(postcode, out var dasDisad) ? dasPostcodeDisadvantages[postcode] : null,
                        EfaDisadvantages = efaPostcodeDisadvantages.TryGetValue(postcode, out var efaDisad) ? efaPostcodeDisadvantages[postcode] : null,
                        ONSData = onsData.TryGetValue(postcode, out var ons) ? onsData[postcode] : null,
                    }).ToList();
            }
        }

        public async Task<List<string>> RetrieveMasterPostcodes(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT UPPER(P.[Postcode]) AS Postcode FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                    INNER JOIN [dbo].[MasterPostcodes] J ON J.[Postcode] = P.[Postcode]";

            var postcodes = await RetrieveAsync<MasterPostcode>(jsonParams, sqlSfaAreaCost, cancellationToken);

            return postcodes
                  .Select(p => p.Postcode)
                  .ToList();
        }

        public async Task<IDictionary<string, List<SfaAreaCost>>> RetrieveSfaAreaCosts(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT UPPER(P.[Postcode]) AS Postcode, J.[AreaCostFactor], J.[EffectiveFrom], J.[EffectiveTo] 
                                                    FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                    INNER JOIN [dbo].[SFA_PostcodeAreaCost] J ON J.[Postcode] = P.[Postcode]";

            var sfaAReaCosts = await RetrieveAsync<SfaPostcodeAreaCost>(jsonParams, sqlSfaAreaCost, cancellationToken);

            return sfaAReaCosts
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.SfaAreaCostsToEntity).ToList());
        }

        public async Task<IDictionary<string, List<SfaDisadvantage>>> RetrieveSfaPostcodeDisadvantages(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlSfaPostcodeDisadvantage = $@"SELECT UPPER(P.[Postcode]) AS Postcode, J.[Uplift], J.[EffectiveFrom], J.[EffectiveTo] 
                                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                                INNER JOIN [dbo].[SFA_PostcodeDisadvantage] J ON J.[Postcode] = P.[Postcode]";

            var sfaDisadvantages = await RetrieveAsync<SfaPostcodeDisadvantage>(jsonParams, sqlSfaPostcodeDisadvantage, cancellationToken);

            return sfaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.SfaPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<EfaDisadvantage>>> RetrieveEfaPostcodeDisadvantages(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlEfaPostcodeDisadvantage = $@"SELECT UPPER(P.[Postcode]) AS Postcode, J.[Uplift], J.[EffectiveFrom], J.[EffectiveTo] 
                                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                                INNER JOIN [dbo].[EFA_PostcodeDisadvantage] J ON J.[Postcode] = P.[Postcode]";

            var efaDisadvantages = await RetrieveAsync<EfaPostcodeDisadvantage>(jsonParams, sqlEfaPostcodeDisadvantage, cancellationToken);

            return efaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.EfaPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<DasDisadvantage>>> RetrieveDasPostcodeDisadvantages(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlDasPostcodeDisadvantage = $@"SELECT UPPER(P.[Postcode]) AS Postcode, J.[Uplift], J.[EffectiveFrom], J.[EffectiveTo] 
                                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                                INNER JOIN [dbo].[DAS_PostcodeDisadvantage] J ON J.[Postcode] = P.[Postcode]";

            var dasDisadvantages = await RetrieveAsync<DasPostcodeDisadvantage>(jsonParams, sqlDasPostcodeDisadvantage, cancellationToken);

            return dasDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.DasPostcodeDisadvantagesToEntity).ToList());
        }

        public async Task<IDictionary<string, List<ONSData>>> RetrieveOnsData(string jsonParams, CancellationToken cancellationToken)
        {
            var sqlOnsData = $@"SELECT UPPER(P.[Postcode]) AS Postcode, J.[Introduction], J.[Termination], J.[LocalAuthority], J.[Lep1], J.[Lep2], 
                                                J.[EffectiveFrom], J.[EffectiveTo], J.[Nuts]
                                                FROM OPENJSON(@jsonParams) WITH (Postcode nvarchar(8) '$') P 
                                                INNER JOIN [dbo].[ONS_Postcodes] J ON J.[Postcode] = P.[Postcode]";

            var onsData = await RetrieveAsync<OnsPostcode>(jsonParams, sqlOnsData, cancellationToken);

            return onsData
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.ONSDataToEntity).ToList());
        }

        public virtual async Task<IEnumerable<T>> RetrieveAsync<T>(string jsonParams, string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.PostcodesConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, new { jsonParams }, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<T>(commandDefinition);
            }
        }
    }
}
