using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Constants;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ReferenceData.Postcodes.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Repository
{
    public class DesktopPostcodesRepositoryService : IDesktopReferenceDataRepositoryService<IReadOnlyCollection<Postcode>>
    {
        private readonly IReferenceDataOptions _referenceDataOptions;
        private readonly IPostcodesEntityModelMapper _postcodesEntityModelMapper;
        private readonly IReferenceDataStatisticsService _referenceDataStatisticsService;

        public DesktopPostcodesRepositoryService(
            IReferenceDataOptions referenceDataOptions,
            IPostcodesEntityModelMapper postcodesEntityModelMapper,
            IReferenceDataStatisticsService referenceDataStatisticsService)
        {
            _referenceDataOptions = referenceDataOptions;
            _postcodesEntityModelMapper = postcodesEntityModelMapper;
            _referenceDataStatisticsService = referenceDataStatisticsService;
        }

        public async Task<IReadOnlyCollection<Postcode>> RetrieveAsync(CancellationToken cancellationToken)
        {
            var masterPostcodes = RetrieveMasterPostcodes(cancellationToken);
            var sfaAreaCosts = RetrieveSfaAreaCosts(cancellationToken);
            var sfaPostcodeDisadvantages = RetrieveSfaPostcodeDisadvantages(cancellationToken);
            var efaPostcodeDisadvantages = RetrieveEfaPostcodeDisadvantages(cancellationToken);
            var dasPostcodeDisadvantages = RetrieveDasPostcodeDisadvantages(cancellationToken);
            var onsData = RetrieveOnsData(cancellationToken);

            _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.PostcodesONSData, onsData.Result.SelectMany(x => x.Value).Count());
            _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.PostcodesDASDisadvantages, dasPostcodeDisadvantages.Result.SelectMany(x => x.Value).Count());
            _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.PostcodesEFADisadvantages, efaPostcodeDisadvantages.Result.SelectMany(x => x.Value).Count());
            _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.PostcodesSFADisadvantages, sfaPostcodeDisadvantages.Result.SelectMany(x => x.Value).Count());
            _referenceDataStatisticsService.AddRecordCount(ReferenceDataSummaryConstants.PostcodesSFAAreaCosts, sfaAreaCosts.Result.SelectMany(x => x.Value).Count());

            var taskList = new List<Task>
            {
                masterPostcodes,
                sfaAreaCosts,
                sfaPostcodeDisadvantages,
                efaPostcodeDisadvantages,
                dasPostcodeDisadvantages,
                onsData
            };

            await Task.WhenAll(taskList);

            return masterPostcodes.Result
                .Select(postcode => new Postcode()
                {
                    PostCode = postcode,
                    SfaDisadvantages = sfaPostcodeDisadvantages.Result.TryGetValue(postcode, out var sfaDisadValued)
                        ? sfaDisadValued
                        : null,
                    SfaAreaCosts = sfaAreaCosts.Result.TryGetValue(postcode, out var sfaAreaCostValue)
                        ? sfaAreaCostValue
                        : null,
                    DasDisadvantages = dasPostcodeDisadvantages.Result.TryGetValue(postcode, out var dasDisadValue)
                        ? dasDisadValue
                        : null,
                    EfaDisadvantages = efaPostcodeDisadvantages.Result.TryGetValue(postcode, out var efaDisadValue)
                        ? efaDisadValue
                        : null,
                    ONSData = onsData.Result.TryGetValue(postcode, out var onsValue) ? onsValue : null,
                }).ToList();
        }

        private async Task<List<string>> RetrieveMasterPostcodes(CancellationToken cancellationToken)
        {
            var sqlPostcodes = $@"SELECT UPPER([Postcode]) AS Postcode FROM [dbo].[MasterPostcodes] order by Postcode";

            var postcodes = await RetrieveAsync<string>(sqlPostcodes, cancellationToken);

            return postcodes.ToList();
        }

        private async Task<IDictionary<string, List<SfaAreaCost>>> RetrieveSfaAreaCosts(CancellationToken cancellationToken)
        {
            var sqlSfaAreaCost = $@"SELECT UPPER([Postcode]) AS Postcode, [AreaCostFactor], [EffectiveFrom], [EffectiveTo] 
                                                    FROM [dbo].[SFA_PostcodeAreaCost]";

            var sfaAreaCosts = await RetrieveAsync<SfaPostcodeAreaCost>(sqlSfaAreaCost, cancellationToken);

            return sfaAreaCosts
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.SfaAreaCostsToEntity).ToList());
        }

        private async Task<IDictionary<string, List<SfaDisadvantage>>> RetrieveSfaPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlSfaPostcodeDisadvantage = $@"SELECT UPPER([Postcode]) AS Postcode, [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[SFA_PostcodeDisadvantage]";

            var sfaDisadvantages = await RetrieveAsync<SfaPostcodeDisadvantage>(sqlSfaPostcodeDisadvantage, cancellationToken);

            return sfaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.SfaPostcodeDisadvantagesToEntity).ToList());
        }

        private async Task<IDictionary<string, List<EfaDisadvantage>>> RetrieveEfaPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlEfaPostcodeDisadvantage = $@"SELECT UPPER([Postcode]) AS Postcode, [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[EFA_PostcodeDisadvantage]";

            var efaDisadvantages = await RetrieveAsync<EfaPostcodeDisadvantage>(sqlEfaPostcodeDisadvantage, cancellationToken);

            return efaDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.EfaPostcodeDisadvantagesToEntity).ToList());
        }

        private async Task<IDictionary<string, List<DasDisadvantage>>> RetrieveDasPostcodeDisadvantages(CancellationToken cancellationToken)
        {
            var sqlDasPostcodeDisadvantage = $@"SELECT UPPER([Postcode]) AS Postcode, [Uplift], [EffectiveFrom], [EffectiveTo] 
                                                                FROM [dbo].[DAS_PostcodeDisadvantage]";

            var dasDisadvantages = await RetrieveAsync<DasPostcodeDisadvantage>(sqlDasPostcodeDisadvantage, cancellationToken);

            return dasDisadvantages
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.DasPostcodeDisadvantagesToEntity).ToList());
        }

        private async Task<IDictionary<string, List<ONSData>>> RetrieveOnsData(CancellationToken cancellationToken)
        {
            var sqlOnsData = $@"SELECT UPPER([Postcode]) AS Postcode, [Introduction], [Termination], [LocalAuthority], [Lep1], [Lep2], 
                                                [EffectiveFrom], [EffectiveTo], [Nuts]
                                                FROM [dbo].[ONS_Postcodes]";

            var onsData = await RetrieveAsync<OnsPostcode>(sqlOnsData, cancellationToken);

            return onsData
                .GroupBy(p => p.Postcode)
                .ToDictionary(k => k.Key, p => p.Select(_postcodesEntityModelMapper.ONSDataToEntity).ToList());
        }

        private async Task<IEnumerable<T>> RetrieveAsync<T>(string sql, CancellationToken cancellationToken)
        {
            using (var sqlConnection = new SqlConnection(_referenceDataOptions.PostcodesConnectionString))
            {
                var commandDefinition = new CommandDefinition(sql, cancellationToken: cancellationToken);
                return await sqlConnection.QueryAsync<T>(commandDefinition);
            }
        }
    }
}
